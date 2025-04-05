using CommunityToolkit.Mvvm.Input;
using System;
using CommunityToolkit.Mvvm.ComponentModel;
using Avalonia.Controls;
using Avalonia.Platform.Storage;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
namespace FloderNavTool.ViewModels
{
    public partial class MainWindowViewModel : ViewModelBase
    {
        private readonly Window _mainWindow;
        public MainWindowViewModel(Window mainWindow)
        {
            _mainWindow = mainWindow;
        }
        private int idCount = 1;
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
                   Id = idCount++,
                };
                NavItems.Add(NavItem);
            }
        }
        [RelayCommand]
        private async Task SearchFolderFile()
        {
            // 获取当前窗口的 StorageProvider
            var storageProvider = _mainWindow.StorageProvider;

            var folder = await storageProvider.OpenFolderPickerAsync(new FolderPickerOpenOptions
            {
                Title = "选择文件夹",
                SuggestedStartLocation = await storageProvider.TryGetFolderFromPathAsync(
                    Environment.GetFolderPath(Environment.SpecialFolder.Desktop))
            });
            if (folder.Count > 0 && folder[0] != null)
            {
                // 获取文件夹路径
                string folderPath = folder[0].Path.LocalPath;
                // 现在你可以使用 folderPath
                FolderPath = folderPath; // 假设你有一个绑定属性叫 FolderPath
            }
        }
    }
        
}
