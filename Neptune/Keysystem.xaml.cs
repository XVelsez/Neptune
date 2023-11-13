using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Neptune
{
    /// <summary>
    /// Interaction logic for Keysystem.xaml
    /// </summary>
    public partial class Keysystem : Window
    {
        public Keysystem()
        {
            InitializeComponent();
        }

        private void website_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://xneptune.com");
        }

        private void getkey_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://xneptune.com");
        }

        private void paste_Click(object sender, RoutedEventArgs e)
        {
            keybox.Text = Clipboard.GetText();
        }

        private void submit_Click(object sender, RoutedEventArgs e)
        {
            string myhwid = System.Security.Principal.WindowsIdentity.GetCurrent().User.Value;
            using (WebClient client = new WebClient())
            {
                string s = client.DownloadString("http://xneptune.com/api.php?regkey=" + keybox.Text + "&hwid=" + myhwid);
                //this.Close();
                //TO DO!
            }
        }
    }
}
