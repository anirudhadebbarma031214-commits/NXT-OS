# NXT-OS v0.2 — Backend virtualization starter

Windows WPF launcher with a QEMU WHPX process manager, an ADB input bridge and an Inno Setup installer.

## Build the launcher installer
Open GitHub **Actions → Build NXT-OS Windows → Run workflow**. The existing workflow publishes the WPF app and runs Inno Setup.

## Runtime layout
```text
runtime/
  qemu/qemu-system-x86_64.exe
  platform-tools/adb.exe
  android/kernel
  android/ramdisk.img
  android/system.img
  android/userdata.qcow2
```

The runtime binaries/images are **not** in the repository or the default installer. To bundle legally redistributable runtime files, place them in the layout above and compile Inno Setup with `/DBundleRuntime`. This may create a very large installer.

## Compatibility and limitations
- Enable Windows Hypervisor Platform and hardware virtualization in BIOS/UEFI.
- The direct-kernel QEMU boot arguments are a **template**. The Android kernel, initramfs, vendor/partition layout and device mapping must be adapted to a compatible Android x86_64 build.
- QEMU opens a separate SDL display; no embedded WPF rendering yet.
- ADB input works only after the guest enables TCP ADB and connects at `127.0.0.1:5555`.
- The input bridge is a library class, not yet wired to the WPF display events.
- Stop currently forces process termination; production needs QMP graceful shutdown.
- Google Play Store and GMS are not bundled; official distribution requires appropriate licensing/certification.
- Successful installer build does not prove Android boot.

## v0.2 changes
Added direct kernel/initrd launch, WHPX, raw system partition attachment, writable QCOW2 userdata and ADB tap/swipe/key APIs. Existing WPF Start/Stop methods remain compatible.
