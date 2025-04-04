using Avalonia.Controls;
using FloderNavTool.ViewModels;

namespace FloderNavTool.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainWindowViewModel(this);
        }
    }
}