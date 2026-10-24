using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace NXT.Core;
public sealed class AdbInputBridge
{
    private readonly string adb;
    public AdbInputBridge(string adbPath) => adb = adbPath;

    private async Task ExecuteAsync(params string[] args)
    {
        var info = new ProcessStartInfo(adb)
        {
            UseShellExecute = false,
            CreateNoWindow = true,
            RedirectStandardError = true
        };
        info.ArgumentList.Add("-s");
        info.ArgumentList.Add("127.0.0.1:5555");
        foreach (var a in args) info.ArgumentList.Add(a);
        using var process = Process.Start(info) ?? throw new InvalidOperationException("ADB failed");
        var error = await process.StandardError.ReadToEndAsync();
        await process.WaitForExitAsync();
        if (process.ExitCode != 0) throw new InvalidOperationException(error);
    }
    public Task TapAsync(int x, int y) =>
        ExecuteAsync("shell", "input", "tap", x.ToString(), y.ToString());
    public Task SwipeAsync(int x1, int y1, int x2, int y2, int durationMs = 300) =>
        ExecuteAsync("shell", "input", "swipe", x1.ToString(), y1.ToString(), x2.ToString(), y2.ToString(), durationMs.ToString());
    public Task KeyAsync(int androidKeyCode) =>
        ExecuteAsync("shell", "input", "keyevent", androidKeyCode.ToString());
    public Task BackAsync() => KeyAsync(4);
    public Task HomeAsync() => KeyAsync(3);
}
