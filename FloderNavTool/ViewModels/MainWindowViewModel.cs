using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Platform.Storage;
using Avalonia.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Newtonsoft.Json;
using static FloderNavTool.ViewModels.SettingsWindowViewModel;

namespace FloderNavTool.ViewModels
{
    public partial class MainWindowViewModel : ViewModelBase
    {
        private readonly Window _mainWindow;
        public static  StorageService _storageService;
        private AppState _appState;
        public MainWindowViewModel(Window mainWindow)
        {
            _mainWindow = mainWindow;
            _storageService = new StorageService();
            // 加载保存的状态
            _appState = _storageService.Load();
            idCount = _appState.IdCount;
            RowCount = _appState.RowCount;
            // 初始化NavItems
            NavItems = new ObservableCollection<NavItemViewModel>(
                _appState.NavItems?.Select(item => new NavItemViewModel
                {
                    Id = item.Id,
                    FolderText = item.FolderText,
                    FolderName = item.FolderName
                }) ?? new List<NavItemViewModel>());

            WeakReferenceMessenger.Default.Register<FolderDeleteMessageSetting>(this, (r, m) =>
            {
                if (m.IsDelected == true)
                {
                    //Dispatcher.UIThread.Post(() =>
                    //{
                    //    var itemToRemove = NavItems.FirstOrDefault(item => item.Id == m.Id);
                    //    if (itemToRemove != null)
                    //    {
                    //        itemToRemove.Dispose();
                    //        NavItems.Remove(itemToRemove);
                    //        SaveState();
                    //    }
                    //});
                    var itemToRemove = NavItems.FirstOrDefault(item => item.Id == m.Id);
                    if (itemToRemove != null)
                    {
                        itemToRemove.Dispose();
                        NavItems.Remove(itemToRemove);
                        SaveState();
                    }
                }
            });
        }

        private int idCount = 0;
        public int RowIndex { get; set; }
        public int ColumnIndex { get; set; }
        public ObservableCollection<NavItemViewModel> NavItems { get; set; }

        [ObservableProperty]
        private int _rowCount = 2;

        [ObservableProperty]
        private string _folderPath = string.Empty;

        [ObservableProperty]
        private string _myMark = "请选择地址";

        [RelayCommand]
        private void AddFolderFile()
        {
            if (idCount % 4 == 0)
            {
                RowCount++;
            }

            if (string.IsNullOrEmpty(FolderPath))
            {
                MyMark = "地址不能为空!!";
            }
            else
            {
                var navItem = new NavItemViewModel()
                {
                    FolderText = FolderPath,
                    Id = idCount++,
                };
                NavItems.Add(navItem);
                SaveState();
            }
        }
        [RelayCommand]
        private async Task SearchFolderFile()
        {
            var storageProvider = _mainWindow.StorageProvider;
            var folder = await storageProvider.OpenFolderPickerAsync(new FolderPickerOpenOptions
            {
                Title = "选择文件夹",
                SuggestedStartLocation = await storageProvider.TryGetFolderFromPathAsync(
                    Environment.GetFolderPath(Environment.SpecialFolder.Desktop))
            });

            if (folder.Count > 0 && folder[0] != null)
            {
                FolderPath = folder[0].Path.LocalPath;
            }
        }
        private void SaveState()
        {
            _appState = new AppState
            {
                IdCount = idCount,
                RowCount = RowCount,
                NavItems = NavItems
                .Select(item => new NavItemState
                {
                    Id = item.Id,
                    FolderName = item.FolderName,
                    FolderText = item.FolderText
                }).ToList()
            };
            _storageService.Save(_appState);
        }
    }
}