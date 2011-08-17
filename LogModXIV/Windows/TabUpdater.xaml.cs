using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Data;

using AppModXIV;

namespace LogModXIV.Windows
{
    /// <summary>
    /// Interaction logic for TabUpdater.xaml
    /// </summary>
    public partial class TabUpdater : Window
    {

        XDocument xdoc = XDocument.Load("Resources/ChatCodes.xml");
        int SaveIndex = 0;

        public TabUpdater()
        {
            InitializeComponent();

            var items = from item in xdoc.Descendants("ChatCode")
                        select new ChatData { Desc = (string)item.Attribute("Desc"), Code = (string)item.Attribute("id") };

            foreach (var item in items)
            {
                guiChatModes.Items.Add(item);
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (MainWindow.Count != 0)
            {
                guiTabList.Items.Clear();
                if (!(MainWindow.TabNames.Count == 0))
                {
                    for (int i = 0; i <= MainWindow.TabNames.Count - 1; i++)
                    {
                        guiTabList.Items.Add(MainWindow.TabNames[i].ToString());
                    }
                }
                guiScanCode.Items.Clear();
                guiTabName.Text = "";
            }
        }

        private void guiAdd_Click(object sender, RoutedEventArgs e)
        {
            if (guiTabName.Text == "")
            {
                MessageBox.Show("Please enter a name for the tab.", "Warning!");
            }
            else
            {
                if (guiScanCode.Items.Count == 0)
                {
                    MessageBox.Show("Can't save tab with no chat modes selected.", "Warning!");
                }
                else
                {
                    AddTheTab();
                }
            }
        }

        private void AddTheTab()
        {
            string TempCodes = "";
            string[] SplitOfChatCodes;

            for (int i = 0; i <= guiScanCode.Items.Count - 1; i++)
            {
                if (i == guiScanCode.Items.Count - 1)
                {
                    TempCodes += guiScanCode.Items[i].ToString();
                }
                else
                {
                    TempCodes += guiScanCode.Items[i].ToString() + ",";
                }
            }

            SplitOfChatCodes = TempCodes.Split(',');

            MainWindow.myWindow.AddTabPageName(guiTabName.Text, SplitOfChatCodes);

            if (MainWindow.Count != 0)
            {
                guiTabList.Items.Clear();
                if (!(MainWindow.TabNames.Count == 0))
                {
                    for (int i = 0; i <= MainWindow.TabNames.Count - 1; i++)
                    {
                        guiTabList.Items.Add(Convert.ToString(MainWindow.TabNames[i]));
                    }
                }
                guiScanCode.Items.Clear();
                guiTabName.Text = "";
            }

            TempCodes = null;
            SplitOfChatCodes = null;
        }

        private void guiSave_Click(object sender, RoutedEventArgs e)
        {
            if (guiScanCode.Items.Count == 0)
            {
                MessageBox.Show("Can't save tab with no chat modes.", "Warning!");
                return;
            }

            MainWindow.ChatScan[SaveIndex].Items.Clear();
            string CSList = "";

            for (int i = 0; i <= guiScanCode.Items.Count - 1; i++)
            {
                CSList += guiScanCode.Items[i].ToString() + ",";
                MainWindow.ChatScan[SaveIndex].Items.Add(guiScanCode.Items[i]);
            }

            Constants.xTab[guiTabList.Items[SaveIndex].ToString()] = CSList.Substring(0, CSList.Length - 1);
            CSList = null;
            guiSave.IsEnabled = false;
        }

        private void guiTabList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                SaveIndex = guiTabList.SelectedIndex;

                guiScanCode.Items.Clear();
                for (int i = 0; i <= MainWindow.ChatScan[guiTabList.SelectedIndex].Items.Count - 1; i++)
                {
                    guiScanCode.Items.Add(MainWindow.ChatScan[guiTabList.SelectedIndex].Items[i]);
                }
                guiSave.IsEnabled = true;
            }
            catch (Exception ex)
            {
                //if (Constants.LogErrors == 1)
                //{
                //    ErrorLogging.LogError(ex.Message + ex.StackTrace + ex.InnerException);
                //}
            }
        }

        private void guiScanCode_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (guiScanCode.SelectedIndex != -1)
                {
                    guiScanCode.Items.RemoveAt(guiScanCode.SelectedIndex);
                }
            }
            catch (Exception ex)
            {
                //if (Constants.LogErrors == 1)
                //{
                //    ErrorLogging.LogError(ex.Message + ex.StackTrace + ex.InnerException);
                //}
            }
        }

        private void guiChatModes_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                ChatData Data = (ChatData)guiChatModes.SelectedItems[0];
                for (int i = 0; i <= guiScanCode.Items.Count; i++)
                {
                    if (!guiScanCode.Items.Contains(Data.Code))
                    {
                        guiScanCode.Items.Add(Data.Code);
                    }
                }
                Data = null;
            }
            catch (Exception ex)
            {
                //if (Constants.LogErrors == 1)
                //{
                //    ErrorLogging.LogError(ex.Message + ex.StackTrace + ex.InnerException);
                //}
            }
        }
    }

    [System.Reflection.ObfuscationAttribute(Feature = "renaming")]
    public class ChatData
    {
        public string Desc { get; set; }
        public string Code { get; set; }
    }
}
