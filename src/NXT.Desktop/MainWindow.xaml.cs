using System;
using System.IO;
using System.Windows;
using Microsoft.Win32;
using NXT.Core;

namespace NXT.Desktop;
public partial class MainWindow : Window
{
    readonly VmManager vm = new();
    public MainWindow(){InitializeComponent();}
    private void Start_Click(object sender,RoutedEventArgs e){try{vm.Start(Path.Combine(AppContext.BaseDirectory,"runtime"));MessageBox.Show("QEMU launched. Android image and QEMU binaries must be provided separately.");}catch(Exception ex){MessageBox.Show(ex.Message,"NXT-OS");}}
    private void Stop_Click(object sender,RoutedEventArgs e){vm.Stop();}
    private async void Install_Click(object sender,RoutedEventArgs e){var d=new OpenFileDialog{Filter="Android packages (*.apk)|*.apk"};if(d.ShowDialog()==true){try{await AdbManager.InstallAsync(Path.Combine(AppContext.BaseDirectory,"runtime","platform-tools","adb.exe"),d.FileName);MessageBox.Show("ADB install command completed.");}catch(Exception ex){MessageBox.Show(ex.Message);}}}
}