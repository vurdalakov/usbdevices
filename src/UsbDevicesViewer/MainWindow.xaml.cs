namespace UsbDevicesViewer
{
    using System;
    using System.Windows;
    using UsbDevicesViewer.Properties;
    using Vurdalakov;

    public partial class MainWindow : Window
    {
        private MainWindowViewModel mainWindowViewModel;

        public MainWindow()
        {
            this.InitializeComponent();

            this.Loaded += OnMainWindowLoaded;
            this.Closing += OnMainWindowClosing;
        }

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);

            this.SetWindowPlacement(Settings.Default.MainWindowPlacement);
        }
            
        private void OnMainWindowLoaded(Object sender, RoutedEventArgs e)
        {
            this.mainWindowViewModel = new MainWindowViewModel();

            this.DataContext = this.mainWindowViewModel;

            this.mainWindowViewModel.Refresh();
        }

        private void OnMainWindowClosing(Object sender, System.ComponentModel.CancelEventArgs e)
        {
            Settings.Default.MainWindowPlacement = this.GetWindowPlacement();
            Settings.Default.Save();

            this.mainWindowViewModel.Close();
        }
    }
}
