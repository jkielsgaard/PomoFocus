using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Notifications;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace PomoFocus
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            ApplicationView.PreferredLaunchViewSize = new Size(600, 800);
            ApplicationView.PreferredLaunchWindowingMode = ApplicationViewWindowingMode.PreferredLaunchViewSize;

            // if you want not to have any window smaller than this size...
            ApplicationView.GetForCurrentView().SetPreferredMinSize(new Size(600, 800));

            this.InitializeComponent();

            this.NavigationCacheMode = NavigationCacheMode.Required;
            timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 1);
            timer.Tick += time;

            started = false;
            Menu = false;
        }

        private DispatcherTimer timer;
        private int basetime;
        private int startime;

        private bool started;
        private bool shortBreak;
        private bool longBreak;
        private bool Menu;



        private void time(object sender, object e)
        {
            startime = startime - 500;

            TimeSpan st = TimeSpan.FromSeconds(startime);
            string TicTok = string.Format("{0:D2}:{1:D2}", st.Minutes, st.Seconds);



            tbl_time.Text = TicTok;
            if (startime == 0)
            {
                timer.Stop();
                btn_ss.IsEnabled = true;


                string lastTimeString = tbl_tasktime.Text;

                TimeSpan timeSpantOnTask = TimeSpan.FromSeconds(basetime) + TimeSpan.Parse(lastTimeString);
                string bt_TicTok = string.Format("{0:D2}:{1:D2}:{2:D2}", timeSpantOnTask.Hours, timeSpantOnTask.Minutes, timeSpantOnTask.Seconds);
                tbl_tasktime.Text = bt_TicTok;
            }
        }

        private void btn_ss_Click(object sender, RoutedEventArgs e)
        {
            if (!started)
            {
                started = true;
                btn_ss.Content      = "💤";
                btn_mode.Content    = "✔";

                basetime = startime = 1500;

                TimeSpan st = TimeSpan.FromSeconds(startime);
                string TicTok = string.Format("{0:D2}:{1:D2}", st.Minutes, st.Seconds);

                tbl_time.Text = TicTok;

                timer.Start();             
            }
            else if (started)
            {
                timer.Stop();

                btn_ss.Content      = "⏳";
                btn_mode.Content    = "♻";
            }
        }


        private void btn_mode_Click(object sender, RoutedEventArgs e)
        {
            if (!started)
            {
                if (true)
                {

                }
                tbl_mode.Text = "Break ☕";
            }
        }

        private void btn_clear_Click(object sender, RoutedEventArgs e)
        {
            ShowToastNotification("foo", "bar");
        }

        private void ShowToastNotification(string title, string stringContent)
        {
            ToastNotifier ToastNotifier = ToastNotificationManager.CreateToastNotifier();
            Windows.Data.Xml.Dom.XmlDocument toastXml = ToastNotificationManager.GetTemplateContent(ToastTemplateType.ToastText02);
            Windows.Data.Xml.Dom.XmlNodeList toastNodeList = toastXml.GetElementsByTagName("text");
            toastNodeList.Item(0).AppendChild(toastXml.CreateTextNode(title));
            toastNodeList.Item(1).AppendChild(toastXml.CreateTextNode(stringContent));
            Windows.Data.Xml.Dom.IXmlNode toastNode = toastXml.SelectSingleNode("/toast");
            Windows.Data.Xml.Dom.XmlElement audio = toastXml.CreateElement("audio");
            audio.SetAttribute("src", "ms-winsoundevent:Notification.SMS");

            ToastNotification toast = new ToastNotification(toastXml);
            toast.ExpirationTime = DateTime.Now.AddSeconds(4);
            ToastNotifier.Show(toast);
        }

        private void btn_menu_Click(object sender, RoutedEventArgs e)
        {
            if (!Menu)
            {
                btn_menu.Content = "🏡";
                Menu = true;
                MainWindow.Height = 800;
                MainWindow.Width = 1400;
                ApplicationView.GetForCurrentView().TryResizeView(new Size { Width = 1400, Height = 800 });
            }
            else if (Menu)
            {
                btn_menu.Content = "🏠";
                Menu = false;
                MainWindow.Height = 800;
                MainWindow.Width = 600;
                ApplicationView.GetForCurrentView().TryResizeView(new Size { Width = 600, Height = 800 });
            }
        }
    }
}
