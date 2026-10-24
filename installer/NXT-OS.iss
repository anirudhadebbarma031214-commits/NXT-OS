#define AppName "NXT-OS"
#define AppVersion "0.2.0"
[Setup]
AppId={{A87C9F52-39A0-49B0-A273-2D5E60F4B982}
AppName={#AppName}
AppVersion={#AppVersion}
DefaultDirName={localappdata}\Programs\NXT-OS
DefaultGroupName=NXT-OS
OutputDir=..\dist
OutputBaseFilename=NXT-OS-Setup
Compression=lzma2
SolidCompression=yes
PrivilegesRequired=lowest
ArchitecturesAllowed=x64compatible
[Files]
Source: "..\publish\*"; DestDir: "{app}"; Flags: recursesubdirs createallsubdirs
#ifdef BundleRuntime
Source: "..\runtime\qemu\*"; DestDir: "{app}\runtime\qemu"; Flags: recursesubdirs createallsubdirs
Source: "..\runtime\platform-tools\*"; DestDir: "{app}\runtime\platform-tools"; Flags: recursesubdirs createallsubdirs
Source: "..\runtime\android\*"; DestDir: "{app}\runtime\android"; Flags: recursesubdirs createallsubdirs
#endif
[Icons]
Name: "{group}\NXT-OS"; Filename: "{app}\NXT.Desktop.exe"
[Run]
Filename: "{app}\NXT.Desktop.exe"; Description: "Launch NXT-OS"; Flags: nowait postinstall skipifsilent
