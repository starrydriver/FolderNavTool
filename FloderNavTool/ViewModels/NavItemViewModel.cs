using CommunityToolkit.Mvvm.Input;
using System;
using CommunityToolkit.Mvvm.ComponentModel;

namespace FloderNavTool.ViewModels
{
    public partial class NavItemViewModel : ViewModelBase
    {

        [ObservableProperty]
        public string _folderText = string.Empty;// 绑定到Grid.Row
        //[ObservableProperty]
        //public int _columnIndex = 0; // 绑定到Grid.Column
    }
}
