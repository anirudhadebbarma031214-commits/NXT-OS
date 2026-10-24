using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace NXT.Core;
public sealed class VmManager : IDisposable
{
    private Process? vm;
    public bool IsRunning => vm is { HasExited: false };

    public void Start(string runtimePath)
    {
        if (IsRunning) return;
        var qemu = Path.Combine(runtimePath, "qemu", "qemu-system-x86_64.exe");
        var android = Path.Combine(runtimePath, "android");
        var kernel = Path.Combine(android, "kernel");
        var ramdisk = Path.Combine(android, "ramdisk.img");
        var system = Path.Combine(android, "system.img");
        var data = Path.Combine(android, "userdata.qcow2");
        foreach (var file in new[] { qemu, kernel, ramdisk, system, data })
            if (!File.Exists(file)) throw new FileNotFoundException("Missing required runtime file", file);

        var info = new ProcessStartInfo(qemu)
        {
            WorkingDirectory = Path.GetDirectoryName(qemu)!,
            UseShellExecute = false,
            CreateNoWindow = true
        };
        void Add(params string[] args) { foreach (var a in args) info.ArgumentList.Add(a); }
        Add("-machine", "q35,accel=whpx");
        Add("-cpu", "max");
        Add("-smp", "4");
        Add("-m", "4096");
        Add("-kernel", kernel);
        Add("-initrd", ramdisk);
        // Guest-specific boot parameters: adjust to match the Android distribution.
        Add("-append", "console=ttyS0 androidboot.hardware=nxtos");
        Add("-drive", $"file={system},format=raw,if=none,id=system");
        Add("-device", "virtio-blk-pci,drive=system");
        Add("-drive", $"file={data},format=qcow2,if=none,id=data");
        Add("-device", "virtio-blk-pci,drive=data");
        Add("-device", "virtio-vga");
        Add("-display", "sdl");
        Add("-device", "qemu-xhci");
        Add("-device", "usb-tablet");
        Add("-device", "usb-kbd");
        Add("-netdev", "user,id=net0,hostfwd=tcp:127.0.0.1:5555-:5555");
        Add("-device", "virtio-net-pci,netdev=net0");
        vm = Process.Start(info) ?? throw new InvalidOperationException("QEMU failed to start.");
    }
    public async Task StopAsync()
    {
        if (!IsRunning) return;
        vm!.Kill(entireProcessTree: true);
        await vm.WaitForExitAsync();
        vm.Dispose();
        vm = null;
    }
    public void Stop() => StopAsync().GetAwaiter().GetResult();
    public void Dispose() => Stop();
}
