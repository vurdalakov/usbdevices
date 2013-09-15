namespace UsbDevicesViewer
{
    using System;
    using System.Windows;

    public partial class MainWindow : Window
    {
        private MainWindowViewModel mainWindowViewModel;

        public MainWindow()
        {
            this.InitializeComponent();

            this.Loaded += OnMainWindowLoaded;
        }

        private void OnMainWindowLoaded(object sender, RoutedEventArgs e)
        {
            this.mainWindowViewModel = new MainWindowViewModel();

            this.DataContext = this.mainWindowViewModel;

            this.mainWindowViewModel.Refresh();
        }
    }
}
