using CommunityToolkit.Mvvm.Input;
using System;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;


namespace FloderNavTool.ViewModels
{
    public partial class SettingsWindowViewModel : ViewModelBase
    {
        public record FolderSettingsChangedMessage(string Name, string Path,int Id);
        public int Id { get; set; }
        [ObservableProperty]
        public string _folderNameSetting = string.Empty;
        [ObservableProperty]
        public string _folderPathSetting = string.Empty;
        [RelayCommand]
        private void ModifyNav()
        {
            WeakReferenceMessenger.Default.Send(new FolderSettingsChangedMessage(
                FolderNameSetting,
                FolderPathSetting,
                Id
            ));
        }
    }
    
}
