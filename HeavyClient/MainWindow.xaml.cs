using Routing;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
namespace HeavyClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Service1Client service1 = new Service1Client();

        public MainWindow()
        {
            InitializeComponent();

            MenuItem logs = new MenuItem()
            {
                Header = "Logs",
                FontSize = 15
            };
            MenuItem quit = new MenuItem()
            {
                Header = "Quit",
                FontSize = 15,
            };
            quit.Click += On_Quit_Click;

            MenuItem export = new MenuItem()
            {
                Header = "Export",
                FontSize = 15
            };

            MenuItem mainItem = new MenuItem()
            {
                Header = "Menu",
                FontSize = 15,
            };

            mainItem.Items.Add(logs);
            mainItem.Items.Add(export);
            mainItem.Items.Add(quit);

            menu.Items.Add(mainItem);
            mainMenu.NavigationUIVisibility = NavigationUIVisibility.Automatic;
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
        }

        private void On_Quit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
