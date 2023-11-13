using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Diagnostics;

using System.Windows.Forms;
using System.IO;
using ICSharpCode.AvalonEdit;
using static System.Windows.Forms.LinkLabel;
using System.Xml;
using System.Xml.Linq;
using ICSharpCode.AvalonEdit.Highlighting;
using ICSharpCode.AvalonEdit.Highlighting.Xshd;

using System.Reflection;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Tab;
using System.Linq.Expressions;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;
using System.Net;
using static System.Net.Mime.MediaTypeNames;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using Microsoft.Win32;
using System.Data.SqlTypes;

namespace Neptune
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary> 

    public partial class MainWindow : System.Windows.Window
    {

        // MAIN DATA SET HERE!!!

        int pagesget = 10; // Request Pages
        string apilink = "https://scriptblox.com/api/script/fetch?page=";
        string apirawlink = "https://rawscripts.net/raw/";
        int mycounts = 0;
        string sfolder = System.Windows.Forms.Application.StartupPath + "/Scripts";
        string wellcome = "-- Welcome to Neptune" + System.Environment.NewLine + System.Environment.NewLine + "-- <3"; // Welcome message!

        private void SaveSettings() {
            string scriptlocation = sfolder;
            string autoexecute = autoexecutecheck.IsChecked.ToString();
            string autoattach = autoattachcheck.IsChecked.ToString();
            string topmost = topmostcheck.IsChecked.ToString();
            string tosave = scriptlocation + ";" + autoexecute + ";" + autoattach + ";" + topmost;
            if (File.Exists("config.cfig")) {
                File.WriteAllText(@"config.cfig", tosave);
            } else {
                FileStream fs = File.Create("config.cfig");
                fs.Dispose();
                File.WriteAllText(@"config.cfig", tosave);
            }
        }

        private void LoadSettings() {
            string content = File.ReadAllText(@"config.cfig");
            string[] cfigs = content.Split(';');
            sfolder = cfigs[0];
            if (cfigs[1] == "True") {
                autoexecutecheck.IsChecked = true;
            } else {
                autoexecutecheck.IsChecked = false;
            }
            if (cfigs[2] == "True")
            {
                autoattachcheck.IsChecked = true;
            }
            else
            {
                autoattachcheck.IsChecked = false;
            }
            if (cfigs[3] == "True")
            {
                topmostcheck.IsChecked = true;
                this.Topmost = true;
            }
            else
            {
                topmostcheck.IsChecked = false;
                this.Topmost = false;
            }
        }

        private void LoadEditorWithSyntax() {
            IHighlightingDefinition customHighlighting;
            using (var reader = new XmlTextReader("lua.xml")) {
                customHighlighting = HighlightingLoader.Load(reader, HighlightingManager.Instance);
            }
            MainTextEditor.SyntaxHighlighting = customHighlighting;
            MainTextEditor1.SyntaxHighlighting = customHighlighting;
        }

        private void UpdateScriptFolder() {
            try
            {
                for (int i = FoldersList.Items.Count - 1; i > 0; i--)
                {
                    FoldersList.Items.RemoveAt(i);
                }
            }
            catch {

            }

            if (!Directory.Exists(sfolder + "/Auto Execute"))
            {
                Directory.CreateDirectory(sfolder + "/Auto Execute");
            }

            DirectoryInfo dinfo = new DirectoryInfo(sfolder);
            DirectoryInfo aedinfo = new DirectoryInfo(sfolder + "/Auto Execute");
            FileInfo[] Files = dinfo.GetFiles("*.txt");
            FileInfo[] aedFiles = aedinfo.GetFiles("*.txt");
            TreeViewItem ListHeader = new TreeViewItem()
            {
                Header = "Auto Execute",
                Padding = BaseExample.Padding,
                FontFamily = BaseExample.FontFamily,
                FontSize = 17,
                Foreground = BaseExample.Foreground,
                Style = BaseExample.Style,
                Tag = "Folder"
            };
            FoldersList.Items.Add(ListHeader);
            foreach (FileInfo file in Files)
            {
                TreeViewItem SubItemer = new TreeViewItem()
                {
                    Header = file.Name,
                    HorizontalAlignment = BaseSubExample.HorizontalAlignment,
                    FontSize = BaseSubExample.FontSize,
                    Foreground = BaseSubExample.Foreground,
                    Style = BaseSubExample.Style,
                    Tag = sfolder + "/" + file.Name
                };
                FoldersList.Items.Add(SubItemer);
            }
            foreach (FileInfo file in aedFiles)
            {
                TreeViewItem SubItemer = new TreeViewItem()
                {
                    Header = file.Name,
                    HorizontalAlignment = BaseSubExample.HorizontalAlignment,
                    FontSize = BaseSubExample.FontSize,
                    Foreground = BaseSubExample.Foreground,
                    Style = BaseSubExample.Style,
                    Tag = sfolder + "/Auto Execute/" + file.Name
                };
                ListHeader.Items.Add(SubItemer);
            }
            SaveSettings();
        }

        private void LoadScriptFolder() {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            DialogResult resulter = dialog.ShowDialog();
            if (resulter == System.Windows.Forms.DialogResult.OK)
            {
                string selectedpath = dialog.SelectedPath;
                sfolder = dialog.SelectedPath;
                if (!Directory.Exists(sfolder + "/Auto Execute"))
                {
                    Directory.CreateDirectory(sfolder + "/Auto Execute");
                }
                DirectoryInfo dinfo = new DirectoryInfo(selectedpath);
                DirectoryInfo aedinfo = new DirectoryInfo(sfolder + "/Auto Execute");
                FileInfo[] Files = dinfo.GetFiles("*.txt");
                FileInfo[] aedFiles = aedinfo.GetFiles("*.txt");
                TreeViewItem ListHeader = new TreeViewItem()
                {
                    Header = "Auto Execute",
                    Padding = BaseExample.Padding,
                    FontFamily = BaseExample.FontFamily,
                    FontSize = 17,
                    Foreground = BaseExample.Foreground,
                    Style = BaseExample.Style,
                    Tag = "Folder"
                };
                FoldersList.Items.Add(ListHeader);
                foreach (FileInfo file in Files)
                {
                    TreeViewItem SubItemer = new TreeViewItem()
                    {
                        Header = file.Name,
                        HorizontalAlignment = BaseSubExample.HorizontalAlignment,
                        FontSize = BaseSubExample.FontSize,
                        Foreground = BaseSubExample.Foreground,
                        Style = BaseSubExample.Style,
                        Tag = selectedpath + "/" + file.Name
                    };
                    FoldersList.Items.Add(SubItemer);
                }
                foreach (FileInfo file in aedFiles)
                {
                    TreeViewItem SubItemer = new TreeViewItem()
                    {
                        Header = file.Name,
                        HorizontalAlignment = BaseSubExample.HorizontalAlignment,
                        FontSize = BaseSubExample.FontSize,
                        Foreground = BaseSubExample.Foreground,
                        Style = BaseSubExample.Style,
                        Tag = selectedpath + "/Auto Execute/" + file.Name
                    };
                    ListHeader.Items.Add(SubItemer);
                }
            }
        }

        private void LoadEveryAutoExecute()
        {
            if (!Directory.Exists(sfolder + "/Auto Execute"))
            {
                Directory.CreateDirectory(sfolder + "/Auto Execute");
            }
            DirectoryInfo aedinfo = new DirectoryInfo(sfolder + "/Auto Execute");
            FileInfo[] aedFiles = aedinfo.GetFiles("*.txt");
            foreach (FileInfo file in aedFiles)
            {
                System.Windows.Controls.TabItem NewTab = new System.Windows.Controls.TabItem()
                {
                    Header = file.Name,
                    FontFamily = FirstItem1.FontFamily,
                    Foreground = FirstItem1.Foreground,
                    Style = FirstItem1.Style,
                    VerticalContentAlignment = FirstItem1.VerticalContentAlignment,
                    VerticalAlignment = FirstItem1.VerticalAlignment
                };
                TextEditor NewEditor = new TextEditor()
                {
                    SyntaxHighlighting = MainTextEditor1.SyntaxHighlighting,
                    Background = MainTextEditor1.Background,
                    FontFamily = MainTextEditor1.FontFamily,
                    Foreground = MainTextEditor1.Foreground,
                    FontSize = MainTextEditor1.FontSize,
                    FontWeight = MainTextEditor1.FontWeight,
                    ShowLineNumbers = MainTextEditor1.ShowLineNumbers,
                    HorizontalScrollBarVisibility = MainTextEditor1.HorizontalScrollBarVisibility,
                    VerticalScrollBarVisibility = MainTextEditor1.VerticalScrollBarVisibility,
                    Text = File.ReadAllText(sfolder + "/Auto Execute/" + file.Name)
                };
                tabControl1.Items.Add(NewTab);
                NewTab.Content = NewEditor;
            }
        }

        private void LoadBloxScripts()
        {
            int counter = 0;
            string splitering = "";
            using (WebClient client = new WebClient())
            {
                try
                {
                    for (int i = 0; i < pagesget; i++)
                    {
                        string s = client.DownloadString(apilink + i.ToString());
                        splitering += s;
                    }
                    string[] splitersone = splitering.Split('"');
                    foreach (var word in splitersone)
                    {
                        counter++;
                        if (word == "title")
                        {
                            TextBlock boxone = new TextBlock()
                            {
                                Width = LikeCaption.Width,
                                Height = LikeCaption.Height,
                                Margin = LikeCaption.Margin,
                                HorizontalAlignment = LikeCaption.HorizontalAlignment,
                                VerticalAlignment = LikeCaption.VerticalAlignment,
                                FontFamily = LikeCaption.FontFamily,
                                FontSize = LikeCaption.FontSize,
                                Text = "Likes:" + splitersone[counter + 76],
                                TextWrapping = LikeCaption.TextWrapping
                            };
                            Border boxtwo = new Border()
                            {
                                Width = BloxBorder.Width,
                                Height = BloxBorder.Height,
                                Margin = BloxBorder.Margin,
                                HorizontalAlignment = BloxBorder.HorizontalAlignment,
                                VerticalAlignment = BloxBorder.VerticalAlignment,
                                Background = BloxBorder.Background,
                                BorderThickness = BloxBorder.BorderThickness,
                                CornerRadius = BloxBorder.CornerRadius
                            };
                            TextBlock boxthree = new TextBlock()
                            {
                                Width = ScripterTitle.Width,
                                Height = ScripterTitle.Height,
                                Margin = ScripterTitle.Margin,
                                HorizontalAlignment = ScripterTitle.HorizontalAlignment,
                                VerticalAlignment = ScripterTitle.VerticalAlignment,
                                FontFamily = ScripterTitle.FontFamily,
                                FontSize = ScripterTitle.FontSize,
                                Foreground = ScripterTitle.Foreground,
                                Text = splitersone[counter + 1],
                                TextWrapping = ScripterTitle.TextWrapping
                            };
                            TextBlock boxfour = new TextBlock()
                            {
                                Width = ScripterGame.Width,
                                Height = ScripterGame.Height,
                                Margin = ScripterGame.Margin,
                                HorizontalAlignment = ScripterGame.HorizontalAlignment,
                                VerticalAlignment = ScripterGame.VerticalAlignment,
                                FontFamily = ScripterGame.FontFamily,
                                FontSize = ScripterGame.FontSize,
                                Foreground = ScripterGame.Foreground,
                                Text = splitersone[counter + 9],
                                TextWrapping = ScripterGame.TextWrapping
                            };
                            System.Windows.Controls.Button boxfive = new System.Windows.Controls.Button()
                            {
                                Tag = splitersone[counter + 53] + "|" + splitersone[counter + 1],
                                Width = ScripterBtn.Width,
                                Height = ScripterBtn.Height,
                                Margin = ScripterBtn.Margin,
                                HorizontalAlignment = ScripterBtn.HorizontalAlignment,
                                VerticalAlignment = ScripterBtn.VerticalAlignment,
                                FontFamily = ScripterBtn.FontFamily,
                                FontSize = ScripterBtn.FontSize,
                                Foreground = ScripterBtn.Foreground,
                                Style = ScripterBtn.Style,
                                Content = ScripterBtn.Content,
                            };
                            boxfive.Click += new RoutedEventHandler(Button_Click_1);
                            TextBlock boxsix = new TextBlock() {
                                Width = ScripterDateCaption.Width,
                                Height = ScripterDateCaption.Height,
                                Margin = ScripterDateCaption.Margin,
                                HorizontalAlignment = ScripterDateCaption.HorizontalAlignment,
                                VerticalAlignment = ScripterDateCaption.VerticalAlignment,
                                FontFamily = ScripterDateCaption.FontFamily,
                                FontSize = ScripterDateCaption.FontSize,
                                Foreground = ScripterDateCaption.Foreground,
                                Text = splitersone[counter + 39],
                                TextWrapping = ScripterDateCaption.TextWrapping
                            };
                            Border boxseven = new Border()
                            {
                                Width = ScripterData.Width,
                                Height = ScripterData.Height,
                                Margin = ScripterData.Margin,
                                HorizontalAlignment = ScripterData.HorizontalAlignment,
                                VerticalAlignment = ScripterData.VerticalAlignment,
                                Background = ScripterData.Background,
                                BorderThickness = ScripterData.BorderThickness,
                                CornerRadius = ScripterData.CornerRadius
                            };
                            TextBlock boxeight = new TextBlock() {
                                Margin = Scriptexecutionercaption.Margin,
                                FontFamily = Scriptexecutionercaption.FontFamily,
                                FontSize = Scriptexecutionercaption.FontSize,
                                Text = Scriptexecutionercaption.Text
                            };
                            Ellipse boxnine = new Ellipse()
                            {
                                Width = ExecutionerBtnBack.Width,
                                Height = ExecutionerBtnBack.Height,
                                Margin = ExecutionerBtnBack.Margin,
                                HorizontalAlignment = ExecutionerBtnBack.HorizontalAlignment,
                                VerticalAlignment = ExecutionerBtnBack.VerticalAlignment,
                                Fill = ExecutionerBtnBack.Fill
                            };
                            System.Windows.Shapes.Path boxten = new System.Windows.Shapes.Path() {
                                Width = ExecutionerBtnPlay.Width,
                                Height = ExecutionerBtnPlay.Height,
                                Margin = ExecutionerBtnPlay.Margin,
                                HorizontalAlignment = ExecutionerBtnPlay.HorizontalAlignment,
                                VerticalAlignment = ExecutionerBtnPlay.VerticalAlignment,
                                Data = ExecutionerBtnPlay.Data,
                                Fill = ExecutionerBtnPlay.Fill,
                                Stretch = ExecutionerBtnPlay.Stretch
                            };
                            Grid boxeleven = new Grid() {
                                Width = ScripterExecutionerGrid.Width
                            };
                            System.Windows.Controls.Button boxtwelve = new System.Windows.Controls.Button()
                            {
                                Tag = splitersone[counter + 53] + "|" + splitersone[counter + 1],
                                Width = ScripterExecuter.Width,
                                Height = ScripterExecuter.Height,
                                Margin = ScripterExecuter.Margin,
                                HorizontalAlignment = ScripterExecuter.HorizontalAlignment,
                                VerticalAlignment = ScripterExecuter.VerticalAlignment,
                                FontFamily = ScripterExecuter.FontFamily,
                                FontWeight = ScripterExecuter.FontWeight,
                                Foreground = ScripterExecuter.Foreground,
                                Style = ScripterExecuter.Style,
                                Content = ScripterExecuter.Content,
                            };
                            Grid boxthirteen = new Grid() {};
                            Border boxfourteen = new Border() {
                                Width = ScriptBorderBox.Width,
                                Height = ScriptBorderBox.Height,
                                Margin = ScriptBorderBox.Margin,
                                HorizontalAlignment = ScriptBorderBox.HorizontalAlignment,
                                VerticalAlignment = ScriptBorderBox.VerticalAlignment,
                                Background = ScriptBorderBox.Background,
                                BorderThickness = ScriptBorderBox.BorderThickness,
                                CornerRadius = ScriptBorderBox.CornerRadius
                            };
                            Border boxfifteen = new Border(){};
                            boxeleven.Children.Add(boxeight);
                            boxeleven.Children.Add(boxnine);
                            boxeleven.Children.Add(boxten);
                            boxseven.Child = boxsix;
                            boxtwelve.Content = boxeleven;
                            boxthirteen.Children.Add(boxone);
                            boxthirteen.Children.Add(boxtwo);
                            boxthirteen.Children.Add(boxthree);
                            boxthirteen.Children.Add(boxfour);
                            boxthirteen.Children.Add(boxseven);
                            boxthirteen.Children.Add(boxfive);
                            boxthirteen.Children.Add(boxtwelve);
                            boxfourteen.Child = boxthirteen;
                            boxfifteen.Child = boxfourteen;
                            scriptbloxscripts.Children.Add(boxfifteen);

                            //System.Windows.MessageBox.Show("Title:" + splitersone[counter + 1]);
                            //System.Windows.MessageBox.Show("Name:" + splitersone[counter + 9]);
                            //System.Windows.MessageBox.Show("CreatedAt:" + splitersone[counter + 39]);
                            //System.Windows.MessageBox.Show("Slug:" + splitersone[counter + 53]);
                            //System.Windows.MessageBox.Show("LikeCount:" + splitersone[counter + 76]);
                        }
                    }
                }
                catch 
                {
                    System.Windows.MessageBox.Show("We can't get BloxList! Sorry ,-,");
                }
            }
        }

        public MainWindow()
        {
            InitializeComponent();
            LoadSettings();
            MainTextEditor.Text = wellcome;
            if (File.Exists("lua.xml")) {
                LoadEditorWithSyntax();
            }
            if (Directory.Exists(sfolder)) {
                UpdateScriptFolder();
            } else {
                System.Windows.Forms.MessageBox.Show("You don't have a scripts folder, please, select one!", "Neptune!", MessageBoxButtons.OK ,MessageBoxIcon.Exclamation);
                LoadScriptFolder();
            }
            LoadEveryAutoExecute();
            LoadBloxScripts();
        }

        private void websitebutton_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://xneptune.com");
        }

        private void discordbutton_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://discord.gg/9v375pYK");
        }

        private void button_Click(object sender, RoutedEventArgs e){
            System.Windows.Controls.TabItem NewTab = new System.Windows.Controls.TabItem()
            {
                Header = "New Tab " + mycounts,
                FontFamily = FirstItem1.FontFamily,
                Foreground = FirstItem1.Foreground,
                Style = FirstItem1.Style,
                VerticalContentAlignment = FirstItem1.VerticalContentAlignment,
                VerticalAlignment = FirstItem1.VerticalAlignment
            };
            TextEditor NewEditor = new TextEditor()
            {
                SyntaxHighlighting = MainTextEditor1.SyntaxHighlighting,
                Background = MainTextEditor1.Background,
                FontFamily = MainTextEditor1.FontFamily,
                Foreground = MainTextEditor1.Foreground,
                FontSize = MainTextEditor1.FontSize,
                FontWeight = MainTextEditor1.FontWeight,
                ShowLineNumbers = MainTextEditor1.ShowLineNumbers,
                HorizontalScrollBarVisibility = MainTextEditor1.HorizontalScrollBarVisibility,
                VerticalScrollBarVisibility = MainTextEditor1.VerticalScrollBarVisibility
            };
            tabControl1.Items.Add(NewTab);
            NewTab.Content = NewEditor;
            tabControl1.SelectedItem = NewTab;
            mycounts = mycounts + 1;
        }

        private void CloseBtn_Click(object sender, RoutedEventArgs e)
        {
            if (tabControl1.Items.Count > 1)
            {
                System.Windows.Controls.TabItem MyTabToClose = tabControl1.SelectedItem as System.Windows.Controls.TabItem;
                tabControl1.Items.Remove(MyTabToClose);
            }
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            autoexec.Visibility = Visibility.Visible;
            autoattach.Visibility = Visibility.Visible;
            autolaunch.Visibility = Visibility.Visible;
            monaco.Visibility = Visibility.Visible;
            minimap.Visibility = Visibility.Visible;
            avalonedit.Visibility = Visibility.Visible;
        }

        private void OptExploit_Click(object sender, RoutedEventArgs e)
        {
            autoexec.Visibility = Visibility.Visible;
            autoattach.Visibility = Visibility.Visible;
            autolaunch.Visibility = Visibility.Visible;
            monaco.Visibility = Visibility.Collapsed;
            minimap.Visibility = Visibility.Collapsed;
            avalonedit.Visibility = Visibility.Collapsed;
        }

        private void OptAppearance_Click(object sender, RoutedEventArgs e)
        {
            autoexec.Visibility = Visibility.Collapsed;
            autoattach.Visibility = Visibility.Collapsed;
            autolaunch.Visibility = Visibility.Collapsed;
            monaco.Visibility = Visibility.Visible;
            minimap.Visibility = Visibility.Visible;
            avalonedit.Visibility = Visibility.Visible;
        }

        private void OptAll_Click(object sender, RoutedEventArgs e)
        {
            autoexec.Visibility = Visibility.Visible;
            autoattach.Visibility = Visibility.Visible;
            autolaunch.Visibility = Visibility.Visible;
            monaco.Visibility = Visibility.Visible;
            minimap.Visibility = Visibility.Visible;
            avalonedit.Visibility = Visibility.Visible;
        }

        private void OptProductivity_Click(object sender, RoutedEventArgs e)
        {
            autoexec.Visibility = Visibility.Collapsed;
            autoattach.Visibility = Visibility.Collapsed;
            autolaunch.Visibility = Visibility.Collapsed;
            monaco.Visibility = Visibility.Collapsed;
            minimap.Visibility = Visibility.Collapsed;
            avalonedit.Visibility = Visibility.Collapsed;
        }

        private void radioButton4_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            SolidColorBrush brush = new SolidColorBrush(Colors.Red);
            Closepather.Fill = brush;
        }

        private void radioButton4_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            SolidColorBrush brush = new SolidColorBrush(Colors.White);
            Closepather.Fill = brush;
        }

        private void radioButton4_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void folder_Click(object sender, RoutedEventArgs e) {
            UpdateScriptFolder();
        }

        private void clear_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Controls.TabItem MyTabToClear = tabControl1.SelectedItem as System.Windows.Controls.TabItem;
            System.Windows.Controls.TabItem NewTab = new System.Windows.Controls.TabItem()
            {
                Header = MyTabToClear.Header,
                FontFamily = FirstItem1.FontFamily,
                Foreground = FirstItem1.Foreground,
                Style = FirstItem1.Style,
                VerticalContentAlignment = FirstItem1.VerticalContentAlignment,
                VerticalAlignment = FirstItem1.VerticalAlignment
            };
            TextEditor NewEditor = new TextEditor()
            {
                SyntaxHighlighting = MainTextEditor1.SyntaxHighlighting,
                Background = MainTextEditor1.Background,
                FontFamily = MainTextEditor1.FontFamily,
                Foreground = MainTextEditor1.Foreground,
                FontSize = MainTextEditor1.FontSize,
                FontWeight = MainTextEditor1.FontWeight,
                ShowLineNumbers = MainTextEditor1.ShowLineNumbers,
                HorizontalScrollBarVisibility = MainTextEditor1.HorizontalScrollBarVisibility,
                VerticalScrollBarVisibility = MainTextEditor1.VerticalScrollBarVisibility
            };
            tabControl1.Items.Remove(MyTabToClear);
            tabControl1.Items.Add(NewTab);
            NewTab.Content = NewEditor;
            tabControl1.SelectedItem = NewTab;
        }

        private void FoldersList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            TreeViewItem Selct = FoldersList.SelectedItem as TreeViewItem;
            if (Selct != null)
            {
                string cabecalho = Selct.Header.ToString();
                string tager = Selct.Tag.ToString();
                if (tager != "Folder") {
                    if (tager == "wellcome") {
                        System.Windows.Controls.TabItem NewTab = new System.Windows.Controls.TabItem()
                        {
                            Header = cabecalho,
                            FontFamily = FirstItem1.FontFamily,
                            Foreground = FirstItem1.Foreground,
                            Style = FirstItem1.Style,
                            VerticalContentAlignment = FirstItem1.VerticalContentAlignment,
                            VerticalAlignment = FirstItem1.VerticalAlignment
                        };
                        TextEditor NewEditor = new TextEditor()
                        {
                            SyntaxHighlighting = MainTextEditor1.SyntaxHighlighting,
                            Background = MainTextEditor1.Background,
                            FontFamily = MainTextEditor1.FontFamily,
                            Foreground = MainTextEditor1.Foreground,
                            FontSize = MainTextEditor1.FontSize,
                            FontWeight = MainTextEditor1.FontWeight,
                            ShowLineNumbers = MainTextEditor1.ShowLineNumbers,
                            Text = wellcome,
                            HorizontalScrollBarVisibility = MainTextEditor1.HorizontalScrollBarVisibility,
                            VerticalScrollBarVisibility = MainTextEditor1.VerticalScrollBarVisibility
                        };
                        tabControl1.Items.Add(NewTab);
                        NewTab.Content = NewEditor;
                        tabControl1.SelectedItem = NewTab;
                    } else {
                        string contenter = File.ReadAllText(tager);
                        System.Windows.Controls.TabItem NewTab = new System.Windows.Controls.TabItem()
                        {
                            Header = cabecalho,
                            FontFamily = FirstItem1.FontFamily,
                            Foreground = FirstItem1.Foreground,
                            Style = FirstItem1.Style,
                            VerticalContentAlignment = FirstItem1.VerticalContentAlignment,
                            VerticalAlignment = FirstItem1.VerticalAlignment
                        };
                        TextEditor NewEditor = new TextEditor()
                        {
                            SyntaxHighlighting = MainTextEditor1.SyntaxHighlighting,
                            Background = MainTextEditor1.Background,
                            FontFamily = MainTextEditor1.FontFamily,
                            Foreground = MainTextEditor1.Foreground,
                            FontSize = MainTextEditor1.FontSize,
                            FontWeight = MainTextEditor1.FontWeight,
                            ShowLineNumbers = MainTextEditor1.ShowLineNumbers,
                            Text = contenter,
                            HorizontalScrollBarVisibility = MainTextEditor1.HorizontalScrollBarVisibility,
                            VerticalScrollBarVisibility = MainTextEditor1.VerticalScrollBarVisibility
                        };
                        tabControl1.Items.Add(NewTab);
                        NewTab.Content = NewEditor;
                        tabControl1.SelectedItem = NewTab;
                    }
                }
            }
        }

        private void open_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.OpenFileDialog ofd = new System.Windows.Forms.OpenFileDialog();
            ofd.Title = "Neptune";
            ofd.Filter = "Text files (*.txt)|*.txt";
            ofd.InitialDirectory = @"C:\";
            if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK) {
                string mf = ofd.FileName;
                string content = File.ReadAllText(mf);
                string selectedfilename = System.IO.Path.GetFileName(mf);
                System.Windows.Controls.TabItem NewTab = new System.Windows.Controls.TabItem()
                {
                    Header = selectedfilename,
                    FontFamily = FirstItem1.FontFamily,
                    Foreground = FirstItem1.Foreground,
                    Style = FirstItem1.Style,
                    VerticalContentAlignment = FirstItem1.VerticalContentAlignment,
                    VerticalAlignment = FirstItem1.VerticalAlignment
                };
                TextEditor NewEditor = new TextEditor()
                {
                    SyntaxHighlighting = MainTextEditor1.SyntaxHighlighting,
                    Background = MainTextEditor1.Background,
                    FontFamily = MainTextEditor1.FontFamily,
                    Foreground = MainTextEditor1.Foreground,
                    FontSize = MainTextEditor1.FontSize,
                    FontWeight = MainTextEditor1.FontWeight,
                    ShowLineNumbers = MainTextEditor1.ShowLineNumbers,
                    HorizontalScrollBarVisibility = MainTextEditor1.HorizontalScrollBarVisibility,
                    VerticalScrollBarVisibility = MainTextEditor1.VerticalScrollBarVisibility,
                    Text = content
                };
                tabControl1.Items.Add(NewTab);
                NewTab.Content = NewEditor;
                tabControl1.SelectedItem = NewTab;
                mycounts = mycounts + 1;
            }
        }

        private void CheckTopMost_Click(object sender, RoutedEventArgs e)
        {
            if (topmostcheck.IsChecked == true) {
                this.Topmost = true;
            } else {
                this.Topmost = false;
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            SaveSettings();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                System.Windows.Controls.Button botaoClicado = (System.Windows.Controls.Button)sender;
                string[] words = botaoClicado.Tag.ToString().Split('|');
                string mytitle = words[1];
                string myval = words[0];
                using (WebClient client = new WebClient())
                {
                    string s = client.DownloadString(apirawlink + myval);
                    System.Windows.Controls.TabItem NewTab = new System.Windows.Controls.TabItem()
                    {
                        Header = mytitle,
                        FontFamily = FirstItem1.FontFamily,
                        Foreground = FirstItem1.Foreground,
                        Style = FirstItem1.Style,
                        VerticalContentAlignment = FirstItem1.VerticalContentAlignment,
                        VerticalAlignment = FirstItem1.VerticalAlignment
                    };
                    TextEditor NewEditor = new TextEditor()
                    {
                        SyntaxHighlighting = MainTextEditor1.SyntaxHighlighting,
                        Background = MainTextEditor1.Background,
                        FontFamily = MainTextEditor1.FontFamily,
                        Foreground = MainTextEditor1.Foreground,
                        FontSize = MainTextEditor1.FontSize,
                        FontWeight = MainTextEditor1.FontWeight,
                        ShowLineNumbers = MainTextEditor1.ShowLineNumbers,
                        HorizontalScrollBarVisibility = MainTextEditor1.HorizontalScrollBarVisibility,
                        VerticalScrollBarVisibility = MainTextEditor1.VerticalScrollBarVisibility,
                        Text = s
                    };
                    tabControl1.Items.Add(NewTab);
                    NewTab.Content = NewEditor;
                    tabControl1.SelectedItem = NewTab;
                    System.Windows.MessageBox.Show("New tab has been created!");
                }
            }
            catch
            {
                System.Windows.MessageBox.Show("Something go wrong!");
            }
        }

        private void filter_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                int counter = 0;
                string splitering = "";
                using (WebClient client = new WebClient())
                {
                    try
                    {
                        for (int i = 0; i < pagesget; i++)
                        {
                            string s = client.DownloadString(apilink + i.ToString() + "&q=" + filter.Text);
                            splitering += s;
                        }
                        string[] splitersone = splitering.Split('"');

                        for (int i = scriptbloxscripts.Children.Count - 1; i > 0; i--)
                        {
                            scriptbloxscripts.Children.RemoveAt(i);
                        }

                        foreach (var word in splitersone)
                        {
                            counter++;
                            if (word == "title")
                            {
                                TextBlock boxone = new TextBlock()
                                {
                                    Width = LikeCaption.Width,
                                    Height = LikeCaption.Height,
                                    Margin = LikeCaption.Margin,
                                    HorizontalAlignment = LikeCaption.HorizontalAlignment,
                                    VerticalAlignment = LikeCaption.VerticalAlignment,
                                    FontFamily = LikeCaption.FontFamily,
                                    FontSize = LikeCaption.FontSize,
                                    Text = "Likes:" + splitersone[counter + 76],
                                    TextWrapping = LikeCaption.TextWrapping
                                };
                                Border boxtwo = new Border()
                                {
                                    Width = BloxBorder.Width,
                                    Height = BloxBorder.Height,
                                    Margin = BloxBorder.Margin,
                                    HorizontalAlignment = BloxBorder.HorizontalAlignment,
                                    VerticalAlignment = BloxBorder.VerticalAlignment,
                                    Background = BloxBorder.Background,
                                    BorderThickness = BloxBorder.BorderThickness,
                                    CornerRadius = BloxBorder.CornerRadius
                                };
                                TextBlock boxthree = new TextBlock()
                                {
                                    Width = ScripterTitle.Width,
                                    Height = ScripterTitle.Height,
                                    Margin = ScripterTitle.Margin,
                                    HorizontalAlignment = ScripterTitle.HorizontalAlignment,
                                    VerticalAlignment = ScripterTitle.VerticalAlignment,
                                    FontFamily = ScripterTitle.FontFamily,
                                    FontSize = ScripterTitle.FontSize,
                                    Foreground = ScripterTitle.Foreground,
                                    Text = splitersone[counter + 1],
                                    TextWrapping = ScripterTitle.TextWrapping
                                };
                                TextBlock boxfour = new TextBlock()
                                {
                                    Width = ScripterGame.Width,
                                    Height = ScripterGame.Height,
                                    Margin = ScripterGame.Margin,
                                    HorizontalAlignment = ScripterGame.HorizontalAlignment,
                                    VerticalAlignment = ScripterGame.VerticalAlignment,
                                    FontFamily = ScripterGame.FontFamily,
                                    FontSize = ScripterGame.FontSize,
                                    Foreground = ScripterGame.Foreground,
                                    Text = splitersone[counter + 9],
                                    TextWrapping = ScripterGame.TextWrapping
                                };
                                System.Windows.Controls.Button boxfive = new System.Windows.Controls.Button()
                                {
                                    Tag = splitersone[counter + 53]+"|"+ splitersone[counter + 1],
                                    Width = ScripterBtn.Width,
                                    Height = ScripterBtn.Height,
                                    Margin = ScripterBtn.Margin,
                                    HorizontalAlignment = ScripterBtn.HorizontalAlignment,
                                    VerticalAlignment = ScripterBtn.VerticalAlignment,
                                    FontFamily = ScripterBtn.FontFamily,
                                    FontSize = ScripterBtn.FontSize,
                                    Foreground = ScripterBtn.Foreground,
                                    Style = ScripterBtn.Style,
                                    Content = ScripterBtn.Content,
                                };
                                boxfive.Click += new RoutedEventHandler(Button_Click_1);
                                TextBlock boxsix = new TextBlock()
                                {
                                    Width = ScripterDateCaption.Width,
                                    Height = ScripterDateCaption.Height,
                                    Margin = ScripterDateCaption.Margin,
                                    HorizontalAlignment = ScripterDateCaption.HorizontalAlignment,
                                    VerticalAlignment = ScripterDateCaption.VerticalAlignment,
                                    FontFamily = ScripterDateCaption.FontFamily,
                                    FontSize = ScripterDateCaption.FontSize,
                                    Foreground = ScripterDateCaption.Foreground,
                                    Text = splitersone[counter + 39],
                                    TextWrapping = ScripterDateCaption.TextWrapping
                                };
                                Border boxseven = new Border()
                                {
                                    Width = ScripterData.Width,
                                    Height = ScripterData.Height,
                                    Margin = ScripterData.Margin,
                                    HorizontalAlignment = ScripterData.HorizontalAlignment,
                                    VerticalAlignment = ScripterData.VerticalAlignment,
                                    Background = ScripterData.Background,
                                    BorderThickness = ScripterData.BorderThickness,
                                    CornerRadius = ScripterData.CornerRadius
                                };
                                TextBlock boxeight = new TextBlock()
                                {
                                    Margin = Scriptexecutionercaption.Margin,
                                    FontFamily = Scriptexecutionercaption.FontFamily,
                                    FontSize = Scriptexecutionercaption.FontSize,
                                    Text = Scriptexecutionercaption.Text
                                };
                                Ellipse boxnine = new Ellipse()
                                {
                                    Width = ExecutionerBtnBack.Width,
                                    Height = ExecutionerBtnBack.Height,
                                    Margin = ExecutionerBtnBack.Margin,
                                    HorizontalAlignment = ExecutionerBtnBack.HorizontalAlignment,
                                    VerticalAlignment = ExecutionerBtnBack.VerticalAlignment,
                                    Fill = ExecutionerBtnBack.Fill
                                };
                                System.Windows.Shapes.Path boxten = new System.Windows.Shapes.Path()
                                {
                                    Width = ExecutionerBtnPlay.Width,
                                    Height = ExecutionerBtnPlay.Height,
                                    Margin = ExecutionerBtnPlay.Margin,
                                    HorizontalAlignment = ExecutionerBtnPlay.HorizontalAlignment,
                                    VerticalAlignment = ExecutionerBtnPlay.VerticalAlignment,
                                    Data = ExecutionerBtnPlay.Data,
                                    Fill = ExecutionerBtnPlay.Fill,
                                    Stretch = ExecutionerBtnPlay.Stretch
                                };
                                Grid boxeleven = new Grid()
                                {
                                    Width = ScripterExecutionerGrid.Width
                                };
                                System.Windows.Controls.Button boxtwelve = new System.Windows.Controls.Button()
                                {
                                    Tag = splitersone[counter + 53] + "|" + splitersone[counter + 1],
                                    Width = ScripterExecuter.Width,
                                    Height = ScripterExecuter.Height,
                                    Margin = ScripterExecuter.Margin,
                                    HorizontalAlignment = ScripterExecuter.HorizontalAlignment,
                                    VerticalAlignment = ScripterExecuter.VerticalAlignment,
                                    FontFamily = ScripterExecuter.FontFamily,
                                    FontWeight = ScripterExecuter.FontWeight,
                                    Foreground = ScripterExecuter.Foreground,
                                    Style = ScripterExecuter.Style,
                                    Content = ScripterExecuter.Content,
                                };
                                Grid boxthirteen = new Grid() { };
                                Border boxfourteen = new Border()
                                {
                                    Width = ScriptBorderBox.Width,
                                    Height = ScriptBorderBox.Height,
                                    Margin = ScriptBorderBox.Margin,
                                    HorizontalAlignment = ScriptBorderBox.HorizontalAlignment,
                                    VerticalAlignment = ScriptBorderBox.VerticalAlignment,
                                    Background = ScriptBorderBox.Background,
                                    BorderThickness = ScriptBorderBox.BorderThickness,
                                    CornerRadius = ScriptBorderBox.CornerRadius
                                };
                                Border boxfifteen = new Border() { };
                                boxeleven.Children.Add(boxeight);
                                boxeleven.Children.Add(boxnine);
                                boxeleven.Children.Add(boxten);
                                boxseven.Child = boxsix;
                                boxtwelve.Content = boxeleven;
                                boxthirteen.Children.Add(boxone);
                                boxthirteen.Children.Add(boxtwo);
                                boxthirteen.Children.Add(boxthree);
                                boxthirteen.Children.Add(boxfour);
                                boxthirteen.Children.Add(boxseven);
                                boxthirteen.Children.Add(boxfive);
                                boxthirteen.Children.Add(boxtwelve);
                                boxfourteen.Child = boxthirteen;
                                boxfifteen.Child = boxfourteen;
                                scriptbloxscripts.Children.Add(boxfifteen);
                            }
                        }
                    }
                    catch
                    {
                        System.Windows.MessageBox.Show("We can't get BloxList! Sorry ,-,");
                    }
                }
            }
        }

        private void save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int selectedIndex = tabControl1.SelectedIndex;
                TextEditor avalonEditer = ((tabControl1.Items[selectedIndex] as System.Windows.Controls.TabItem).Content as TextEditor);
                string text = avalonEditer.Text;

                System.Windows.Forms.SaveFileDialog saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
                saveFileDialog1.Title = "Save file";
                saveFileDialog1.FileName = "myfile.txt";
                saveFileDialog1.Filter = "Text file (*.txt)|*.txt";
                if (saveFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    using (StreamWriter sw = new StreamWriter(saveFileDialog1.FileName))
                    {
                        sw.Write(text);
                        sw.Close();
                    }
                }
            }
            catch
            {
                System.Windows.Forms.SaveFileDialog saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
                saveFileDialog1.Title = "Save file";
                saveFileDialog1.FileName = "myfile.txt";
                saveFileDialog1.Filter = "Text file (*.txt)|*.txt";
                if (saveFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    using (StreamWriter sw = new StreamWriter(saveFileDialog1.FileName))
                    {
                        sw.Write(wellcome);
                        sw.Close();
                    }
                }
            }
        }
    }
}

//System.Windows.Controls.TabItem MyTb = (System.Windows.Controls.TabItem)this.Parent;
/*

<Button
                            x:Name="CloserBtn"
                            Width="14"
                            Height="13"
                            Margin="0,0,5,0"
                            BorderBrush="{x:Null}"
                            Foreground="White"

                            Style="{DynamicResource button-background-null}" Click="CloserBtn_Click">
                            <Button.Background>
                                <SolidColorBrush Opacity="0.001" Color="#FFDDDDDD" />
                            </Button.Background>
                            <Path
                                Width="6"
                                Height="6"
                                Data="M5.62334,1.41023L5.0358,0.807665 3.20445,2.59336 1.44184,0.785692 0.831388,1.38093 2.59399,3.1886 0.762612,4.97432 1.35015,5.57689 3.18153,3.79116 4.94416,5.59886 5.55462,5.00362 3.79199,3.19592 5.62334,1.41023z"
                                Fill="White"
                                Stretch="Fill" />
                        </Button>


                        FOCO PRINCIPAL DO PROJETO ATUALMENTE: FAZER LISTA DE SCRIPTS! / SCRIPT LOADER THAT I NEED HELP ON


 */