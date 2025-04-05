using CommunityToolkit.Mvvm.Input;
using System;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;
using FloderNavTool.Views;
using Avalonia.Controls.ApplicationLifetimes;
using CommunityToolkit.Mvvm.Messaging;
using static FloderNavTool.ViewModels.SettingsWindowViewModel;

namespace FloderNavTool.ViewModels
{
    public partial class NavItemViewModel : ViewModelBase
    {
        public int Id { get; set; }
        public SettingsWindowViewModel viewModel { get; set; } = new SettingsWindowViewModel();
        public SettingsWindow settingsWindow { get; set; } = new SettingsWindow();
        public NavItemViewModel()
        {
            WeakReferenceMessenger.Default.Register<FolderSettingsChangedMessage>(this, (r, m) =>
            {
                if (m.Id == this.Id)
                {
                    this.FolderName = m.Name;
                    this.FolderText = m.Path;
                }
            });
        }
        [ObservableProperty]
        public string _folderName = "未命名文件夹";
        [ObservableProperty]
        public string _folderText = string.Empty;
        [RelayCommand]
        public async Task NavFolderFile() 
        {
            if (Directory.Exists(FolderText))
            {
                try
                {
                    // Windows
                    if (OperatingSystem.IsWindows())
                    {
                        await Task.Run(() => Process.Start("explorer.exe", FolderText));
                    }
                    // macOS
                    else if (OperatingSystem.IsMacOS())
                    {
                        await Task.Run(() => Process.Start("open", FolderText));
                    }
                    // Linux
                    else if (OperatingSystem.IsLinux())
                    {
                        await Task.Run(() => Process.Start("xdg-open", FolderText));
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"打开文件夹时出错: {ex.Message}");
                }
            }
            else
            {
                // 文件夹不存在时的处理
                Console.WriteLine($"文件夹不存在: {FolderText}");
            }
        }
        [RelayCommand]
        public async Task SetFolderFile()
        {
            // 创建 ViewModel，并设置初始数据
            this.viewModel = new SettingsWindowViewModel
            {
                FolderNameSetting = this.FolderName,// 传递当前页面的 FolderName
                FolderPathSetting = this.FolderText, // 传递当前页面的 FolderText
                Id = this.Id // 传递当前页面的 Id
            };
            // 创建窗口，并绑定 ViewModel
            this.settingsWindow = new SettingsWindow
            {
                DataContext = viewModel // 关键：设置 DataContext
            };
            var desktop = App.Current?.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime;
            await settingsWindow.ShowDialog(desktop.MainWindow);
        }
    }
}
