[Setup]
AppName=Studio Camera Viewer
AppVersion=1.0
DefaultDirName={autopf}\StudioCameraViewer
DefaultGroupName=Studio Camera Viewer
UninstallDisplayIcon={app}\StudioCameraViewer.exe
Compression=lzma2
SolidCompression=yes
OutputDir=.\InstallerOutput
OutputBaseFilename=StudioCameraViewer_Setup
SetupIconFile=app.ico

[Files]
; The asterisk (*) grabs EVERY file and DLL in your build folder automatically!
Source: "bin\Release\net10.0-windows\publish\win-x64\*"; DestDir: "{app}"; Flags: ignoreversion recursesubdirs

[Icons]
Name: "{group}\Studio Camera Viewer"; Filename: "{app}\StudioCameraViewer.exe"; IconFilename: "{app}\app.ico"
Name: "{autodesktop}\Studio Camera Viewer"; Filename: "{app}\StudioCameraViewer.exe"; IconFilename: "{app}\app.ico"

[Run]
; Auto-pin executable to Taskbar upon completion
Filename: "powershell.exe"; Parameters: "-ExecutionPolicy Bypass -NoProfile -Command ""$shell = New-Object -ComObject Shell.Application; $item = $shell.NameSpace('{app}').ParseName('StudioCameraViewer.exe'); if ($item) {{ $item.InvokeVerb('taskbarpin') }}"""; StatusMsg: "Pinning to taskbar..."; Flags: runhidden
Filename: "{app}\StudioCameraViewer.exe"; Description: "{cm:LaunchProgram,Studio Camera Viewer}"; Flags: nowait postinstall skipifsilent
