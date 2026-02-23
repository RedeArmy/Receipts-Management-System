using System.Windows;
using ReceiptsManagementSystem.Presentation.ViewModels;

namespace ReceiptsManagementSystem.Presentation;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow(MainViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }
}
