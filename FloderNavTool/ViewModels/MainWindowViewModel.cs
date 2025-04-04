using CommunityToolkit.Mvvm.Input;
using System;
using CommunityToolkit.Mvvm.ComponentModel;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Platform.Storage;
using System.Collections.ObjectModel;
namespace FloderNavTool.ViewModels
{
    public partial class MainWindowViewModel : ViewModelBase
    {
        private readonly Window _mainWindow;
        public MainWindowViewModel(Window mainWindow)
        {
            _mainWindow = mainWindow;
        }
        private int _currentIndex = 0;
        public int RowIndex { get; set; }
        public int ColumnIndex { get; set; }
        public ObservableCollection<NavItemViewModel> NavItems { get; set; } = new();

        [ObservableProperty]
        private string _folderPath = string.Empty; // 自动生成 FolderPath 属性
        [ObservableProperty]
        private string _myMark = "请选择地址"; // 自动生成 IsFolderSelected 属性

        [RelayCommand]
        private void AddFolderFile()
        {
            if (string.IsNullOrEmpty(FolderPath))
            {
                MyMark = "地址不能为空!!";
            }
            else
            {
                var NavItem = new NavItemViewModel()
                {
                   FolderText = FolderPath,
                };
                NavItems.Add(NavItem);
               
            }
        }
        [RelayCommand]
        private async void SearchFolderFile()
        {
            var dialog = new OpenFolderDialog
            {
                Title = "选择文件夹",
                Directory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop)
            };
            var result = await dialog.ShowAsync(_mainWindow);


            if (!string.IsNullOrEmpty(result))
            {
                FolderPath = result;
            }
        }
    }
        
}
