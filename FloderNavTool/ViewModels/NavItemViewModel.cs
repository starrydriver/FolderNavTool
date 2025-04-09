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
using System.Collections.Generic;
using System.Linq;

namespace FloderNavTool.ViewModels
{
    public partial class NavItemViewModel : ViewModelBase
    {
        public int Id { get; set; }
        public SettingsWindowViewModel viewModel { get; set; }
        public SettingsWindow settingsWindow { get; set; }
        private AppState _appState;
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

        public void Dispose()
        {
            if (this.settingsWindow!= null)
            {
                this.settingsWindow.Close();
                this.settingsWindow = null!;
                this.viewModel = null!;
            }
        }
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
            // 保存状态
            SaveState();
        }
        private void SaveState()
        {
            // 1. 加载当前存储状态
            _appState = MainWindowViewModel._storageService.Load();

            // 2. 找到匹配的项并更新
            if (_appState.NavItems != null)
            {
                var itemToUpdate = _appState.NavItems.FirstOrDefault(n => n.Id == this.Id);
                if (itemToUpdate != null)
                {
                    itemToUpdate.FolderName = this.FolderName;
                    itemToUpdate.FolderText = this.FolderText; // 如果需要也更新路径
                }
                else
                {
                    // 3. 如果没有找到匹配项，添加新项
                    _appState.NavItems.Add(new NavItemState
                    {
                        Id = this.Id,
                        FolderName = this.FolderName,
                        FolderText = this.FolderText
                    });
                }
            }
            else
            {
                // 4. 如果NavItems为null，初始化列表
                _appState.NavItems = new List<NavItemState>
        {
            new NavItemState
            {
                Id = this.Id,
                FolderName = this.FolderName,
                FolderText = this.FolderText
            }
        };
            }

            // 5. 保存更新后的状态
            MainWindowViewModel._storageService.Save(_appState);
        }
    }
}
