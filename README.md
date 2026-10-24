# NXT-OS
Windows WPF-based QEMU Android emulator **starter**, version 0.1.0.

## Build
Go to Actions → Build NXT-OS Windows → Run workflow. Download the Windows installer artifact after successful completion.

## Known limitations
- This builds the **Windows launcher**, not a complete ready-to-play emulator.
- QEMU binaries, Android system image and ADB platform tools are **not bundled**.
- QEMU launches in a **separate SDL window**.
- WHPX must be enabled in Windows Features and CPU virtualization enabled in firmware.
- A compatible Android QCOW2 image with required VirtIO drivers is needed in `runtime/android/android.qcow2`.
- Google Play / GMS is not bundled, and official certification/licensing is required.
- APK install requires ADB configured at 127.0.0.1:5555 in the guest.
- Stop currently terminates QEMU abruptly, not a graceful guest shutdown.

This is an initial development milestone. Do not interpret successful packaging as verified Android boot or gaming performance.
