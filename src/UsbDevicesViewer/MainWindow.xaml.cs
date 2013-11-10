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
            this.Closing += OnMainWindowClosing;
        }

        private void OnMainWindowLoaded(Object sender, RoutedEventArgs e)
        {
            this.mainWindowViewModel = new MainWindowViewModel();

            this.DataContext = this.mainWindowViewModel;

            this.mainWindowViewModel.Refresh();
        }

        private void OnMainWindowClosing(Object sender, System.ComponentModel.CancelEventArgs e)
        {
            this.mainWindowViewModel.Close();
        }
    }
}
