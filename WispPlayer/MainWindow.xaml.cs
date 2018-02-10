using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WispContract;

namespace WispPlayer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public WispPlugManager wispManager;
        public MenuItem anItem = new MenuItem();

        public MainWindow()
        {
            InitializeComponent();
            //Load Plugins
            WispControl.InitwispPlugManager(new WindowInteropHelper(this).Handle);
            wispManager = WispControl.wispPlugManager;

            this.DataContext = wispManager;

			int count = 0;
            foreach (UserControl temp in wispManager.GetUIList())
			{
				this.RootContainer.Children.Add(temp);
				Grid.SetRow(temp, count);
				count++;
            }
        }

        private void Menu_PlayMusic_Click(object sender, RoutedEventArgs e)
        {
			wispManager.OpenMusicFile(sender, e);
        }

		private void Window_MouseDown(object sender, MouseButtonEventArgs e)
		{
			this.DragMove();
		}
	}
}
