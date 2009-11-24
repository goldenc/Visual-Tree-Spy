using System.Windows;
using System.Windows.Controls;

namespace VisualTreeSpy
{
    public partial class MainPage
    {
        public MainPage()
        {
            InitializeComponent();
            Loaded += MainPage_Loaded;
        }

        void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            VisualTreeSpy.Start();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            SilverlightSpy spy = new SilverlightSpy();
            spy.Show();
        }
    }
}