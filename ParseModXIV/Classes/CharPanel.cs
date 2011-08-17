using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using ParseModXIV;

namespace ParseModXIV.Classes
{
    class CharPanel
    {
        public void Panel(string cNum, string type, int rowNum, string charName, Grid parentGrid)
        {
            //Create Parent Grid
            string[] nameSplit = charName.Split(' ');

            Grid cGrid = new Grid();
            cGrid.Name = "gui" + type + "Character_" + cNum;

            RowDefinition cRow = new RowDefinition();
            cRow.Height = GridLength.Auto;
            ColumnDefinition cCol = new ColumnDefinition();
            cCol.Width = GridLength.Auto;
            cGrid.RowDefinitions.Add(cRow);
            cGrid.ColumnDefinitions.Add(cCol);

            Grid.SetRow(cGrid, rowNum);
            Grid.SetColumn(cGrid, 0);

            //Great GroupBox To Go In Parent Grid
            GroupBox cGroupBox = new GroupBox();
            cGroupBox.Name = "gui" + type + "CharName_" + cNum;

            cGroupBox.Header = charName;
            cGroupBox.Margin = new Thickness(5);
            cGroupBox.FontWeight = FontWeights.Bold;
            cGroupBox.FontSize = 14;
            cGroupBox.BorderBrush = new SolidColorBrush(Color.FromArgb(255, 114, 114, 114));

            Grid.SetRow(cGroupBox, 0);
            Grid.SetColumn(cGroupBox, 0);

            //Add Grid To GroupBox
            Grid dGrid = new Grid();

            RowDefinition dRow1 = new RowDefinition();
            dRow1.Height = new GridLength(23, GridUnitType.Pixel);
            RowDefinition dRow2 = new RowDefinition();
            dRow2.Height = new GridLength(22, GridUnitType.Pixel);
            RowDefinition dRow3 = new RowDefinition();
            dRow3.Height = GridLength.Auto;

            dGrid.RowDefinitions.Add(dRow1);
            dGrid.RowDefinitions.Add(dRow2);
            dGrid.RowDefinitions.Add(dRow3);

            ColumnDefinition dCol1 = new ColumnDefinition();
            dCol1.Width = new GridLength(45, GridUnitType.Pixel);
            ColumnDefinition dCol2 = new ColumnDefinition();
            dCol2.Width = GridLength.Auto;
            ColumnDefinition dCol3 = new ColumnDefinition();
            dCol3.Width = GridLength.Auto;
            ColumnDefinition dCol4 = new ColumnDefinition();
            dCol4.Width = GridLength.Auto;
            ColumnDefinition dCol5 = new ColumnDefinition();
            dCol5.Width = GridLength.Auto;
            ColumnDefinition dCol6 = new ColumnDefinition();
            dCol6.Width = GridLength.Auto;
            ColumnDefinition dCol7 = new ColumnDefinition();
            dCol7.Width = GridLength.Auto;
            ColumnDefinition dCol8 = new ColumnDefinition();
            dCol8.Width = GridLength.Auto;
            ColumnDefinition dCol9 = new ColumnDefinition();
            dCol9.Width = GridLength.Auto;
            ColumnDefinition dCol10 = new ColumnDefinition();
            dCol10.Width = GridLength.Auto;
            ColumnDefinition dCol11 = new ColumnDefinition();
            dCol11.Width = GridLength.Auto;
            ColumnDefinition dCol12 = new ColumnDefinition();
            dCol12.Width = GridLength.Auto;
            ColumnDefinition dCol13 = new ColumnDefinition();
            dCol13.Width = GridLength.Auto;

            dGrid.ColumnDefinitions.Add(dCol1);
            dGrid.ColumnDefinitions.Add(dCol2);
            dGrid.ColumnDefinitions.Add(dCol3);
            dGrid.ColumnDefinitions.Add(dCol4);
            dGrid.ColumnDefinitions.Add(dCol5);
            dGrid.ColumnDefinitions.Add(dCol6);
            dGrid.ColumnDefinitions.Add(dCol7);
            dGrid.ColumnDefinitions.Add(dCol8);
            dGrid.ColumnDefinitions.Add(dCol9);
            dGrid.ColumnDefinitions.Add(dCol10);
            dGrid.ColumnDefinitions.Add(dCol11);
            dGrid.ColumnDefinitions.Add(dCol12);
            dGrid.ColumnDefinitions.Add(dCol13);

            Image dImage = new Image();
            dImage.Name = "gui" + type + "CharImage_" + cNum;
            dImage.Height = dImage.Width = 40;

            Grid.SetRowSpan(dImage, 2);
            Grid.SetColumn(dImage, 0);

            #region " LABELS TITLES "

            Label tLabel1 = new Label();
            tLabel1.Content = "Total";
            tLabel1.FontWeight = FontWeights.Bold;
            tLabel1.FontSize = 10;
            tLabel1.HorizontalAlignment = HorizontalAlignment.Center;

            Label tLabel2 = new Label();
            tLabel2.Content = "Dam %";
            tLabel2.FontWeight = FontWeights.Bold;
            tLabel2.FontSize = 10;
            tLabel2.HorizontalAlignment = HorizontalAlignment.Center;

            Label tLabel3 = new Label();
            tLabel3.Content = "Hit/Miss";
            tLabel3.FontWeight = FontWeights.Bold;
            tLabel3.FontSize = 10;
            tLabel3.HorizontalAlignment = HorizontalAlignment.Center;

            Label tLabel4 = new Label();
            tLabel4.Content = "Acc %";
            tLabel4.FontWeight = FontWeights.Bold;
            tLabel4.FontSize = 10;
            tLabel4.HorizontalAlignment = HorizontalAlignment.Center;

            Label tLabel5 = new Label();
            tLabel5.Content = "Low/Hi";
            tLabel5.FontWeight = FontWeights.Bold;
            tLabel5.FontSize = 10;
            tLabel5.HorizontalAlignment = HorizontalAlignment.Center;

            Label tLabel6 = new Label();
            tLabel6.Content = "Avg";
            tLabel6.FontWeight = FontWeights.Bold;
            tLabel6.FontSize = 10;
            tLabel6.HorizontalAlignment = HorizontalAlignment.Center;

            Label tLabel7 = new Label();
            tLabel7.Content = "C. Total";
            tLabel7.FontWeight = FontWeights.Bold;
            tLabel7.FontSize = 10;
            tLabel7.HorizontalAlignment = HorizontalAlignment.Center;

            Label tLabel8 = new Label();
            tLabel8.Content = "Crits";
            tLabel8.FontWeight = FontWeights.Bold;
            tLabel8.FontSize = 10;
            tLabel8.HorizontalAlignment = HorizontalAlignment.Center;

            Label tLabel9 = new Label();
            tLabel9.Content = "C. Low/Hi";
            tLabel9.FontWeight = FontWeights.Bold;
            tLabel9.FontSize = 10;
            tLabel9.HorizontalAlignment = HorizontalAlignment.Center;

            Label tLabel10 = new Label();
            tLabel10.Content = "C. Avg";
            tLabel10.FontWeight = FontWeights.Bold;
            tLabel10.FontSize = 10;
            tLabel10.HorizontalAlignment = HorizontalAlignment.Center;

            Label tLabel11 = new Label();
            tLabel11.Content = "C. %";
            tLabel11.FontWeight = FontWeights.Bold;
            tLabel11.FontSize = 10;
            tLabel11.HorizontalAlignment = HorizontalAlignment.Center;

            Label tLabel12 = new Label();
            tLabel12.Content = "DPS";
            tLabel12.FontWeight = FontWeights.Bold;
            tLabel12.FontSize = 10;
            tLabel12.HorizontalAlignment = HorizontalAlignment.Center;

            Grid.SetRow(tLabel1, 0);
            Grid.SetRow(tLabel2, 0);
            Grid.SetRow(tLabel3, 0);
            Grid.SetRow(tLabel4, 0);
            Grid.SetRow(tLabel5, 0);
            Grid.SetRow(tLabel6, 0);
            Grid.SetRow(tLabel7, 0);
            Grid.SetRow(tLabel8, 0);
            Grid.SetRow(tLabel9, 0);
            Grid.SetRow(tLabel10, 0);
            Grid.SetRow(tLabel11, 0);
            Grid.SetRow(tLabel12, 0);

            Grid.SetColumn(tLabel1, 1);
            Grid.SetColumn(tLabel2, 2);
            Grid.SetColumn(tLabel3, 3);
            Grid.SetColumn(tLabel4, 4);
            Grid.SetColumn(tLabel5, 5);
            Grid.SetColumn(tLabel6, 6);
            Grid.SetColumn(tLabel7, 7);
            Grid.SetColumn(tLabel8, 8);
            Grid.SetColumn(tLabel9, 9);
            Grid.SetColumn(tLabel10, 10);
            Grid.SetColumn(tLabel11, 11);
            Grid.SetColumn(tLabel12, 12);

            #endregion

            #region " LABELS DATA "

            Label dLabel1 = new Label();
            dLabel1.Content = "0";
            dLabel1.Name = "gui" + type + "Total_" + cNum;
            dLabel1.FontWeight = FontWeights.Normal;
            dLabel1.FontSize = 10;
            dLabel1.HorizontalAlignment = HorizontalAlignment.Center;

            Label dLabel2 = new Label();
            dLabel2.Content = "0.00 %";
            dLabel2.Name = "gui" + type + "DamP_" + cNum;
            dLabel2.FontWeight = FontWeights.Normal;
            dLabel2.FontSize = 10;
            dLabel2.HorizontalAlignment = HorizontalAlignment.Center;

            Label dLabel3 = new Label();
            dLabel3.Content = "0 / 0";
            dLabel3.Name = "gui" + type + "HM_" + cNum;
            dLabel3.FontWeight = FontWeights.Normal;
            dLabel3.FontSize = 10;
            dLabel3.HorizontalAlignment = HorizontalAlignment.Center;

            Label dLabel4 = new Label();
            dLabel4.Content = "0.00 %";
            dLabel4.Name = "gui" + type + "AccP_" + cNum;
            dLabel4.FontWeight = FontWeights.Normal;
            dLabel4.FontSize = 10;
            dLabel4.HorizontalAlignment = HorizontalAlignment.Center;

            Label dLabel5 = new Label();
            dLabel5.Content = "0 / 0";
            dLabel5.Name = "gui" + type + "LH_" + cNum;
            dLabel5.FontWeight = FontWeights.Normal;
            dLabel5.FontSize = 10;
            dLabel5.HorizontalAlignment = HorizontalAlignment.Center;

            Label dLabel6 = new Label();
            dLabel6.Content = "0";
            dLabel6.Name = "gui" + type + "Avg_" + cNum;
            dLabel6.FontWeight = FontWeights.Normal;
            dLabel6.FontSize = 10;
            dLabel6.HorizontalAlignment = HorizontalAlignment.Center;

            Label dLabel7 = new Label();
            dLabel7.Content = "0";
            dLabel7.Name = "gui" + type + "CTotal_" + cNum;
            dLabel7.FontWeight = FontWeights.Normal;
            dLabel7.FontSize = 10;
            dLabel7.HorizontalAlignment = HorizontalAlignment.Center;

            Label dLabel8 = new Label();
            dLabel8.Content = "0";
            dLabel8.Name = "gui" + type + "Crits_" + cNum;
            dLabel8.FontWeight = FontWeights.Normal;
            dLabel8.FontSize = 10;
            dLabel8.HorizontalAlignment = HorizontalAlignment.Center;

            Label dLabel9 = new Label();
            dLabel9.Content = "0 / 0";
            dLabel9.Name = "gui" + type + "CLH_" + cNum;
            dLabel9.FontWeight = FontWeights.Normal;
            dLabel9.FontSize = 10;
            dLabel9.HorizontalAlignment = HorizontalAlignment.Center;

            Label dLabel10 = new Label();
            dLabel10.Content = "0";
            dLabel10.Name = "gui" + type + "CAvg_" + cNum;
            dLabel10.FontWeight = FontWeights.Normal;
            dLabel10.FontSize = 10;
            dLabel10.HorizontalAlignment = HorizontalAlignment.Center;

            Label dLabel11 = new Label();
            dLabel11.Content = "0.00 %";
            dLabel11.Name = "gui" + type + "CP_" + cNum;
            dLabel11.FontWeight = FontWeights.Normal;
            dLabel11.FontSize = 10;
            dLabel11.HorizontalAlignment = HorizontalAlignment.Center;

            Label dLabel12 = new Label();
            dLabel12.Content = "0";
            dLabel12.Name = "gui" + type + "DPS_" + cNum;
            dLabel12.FontWeight = FontWeights.Normal;
            dLabel12.FontSize = 10;
            dLabel12.HorizontalAlignment = HorizontalAlignment.Center;

            Grid.SetRow(dLabel1, 1);
            Grid.SetRow(dLabel2, 1);
            Grid.SetRow(dLabel3, 1);
            Grid.SetRow(dLabel4, 1);
            Grid.SetRow(dLabel5, 1);
            Grid.SetRow(dLabel6, 1);
            Grid.SetRow(dLabel7, 1);
            Grid.SetRow(dLabel8, 1);
            Grid.SetRow(dLabel9, 1);
            Grid.SetRow(dLabel10, 1);
            Grid.SetRow(dLabel11, 1);
            Grid.SetRow(dLabel12, 1);

            Grid.SetColumn(dLabel1, 1);
            Grid.SetColumn(dLabel2, 2);
            Grid.SetColumn(dLabel3, 3);
            Grid.SetColumn(dLabel4, 4);
            Grid.SetColumn(dLabel5, 5);
            Grid.SetColumn(dLabel6, 6);
            Grid.SetColumn(dLabel7, 7);
            Grid.SetColumn(dLabel8, 8);
            Grid.SetColumn(dLabel9, 9);
            Grid.SetColumn(dLabel10, 10);
            Grid.SetColumn(dLabel11, 11);
            Grid.SetColumn(dLabel12, 12);


            dGrid.Children.Add(dImage);
            dGrid.Children.Add(tLabel1);
            dGrid.Children.Add(tLabel2);
            dGrid.Children.Add(tLabel3);
            dGrid.Children.Add(tLabel4);
            dGrid.Children.Add(tLabel5);
            dGrid.Children.Add(tLabel6);
            dGrid.Children.Add(tLabel7);
            dGrid.Children.Add(tLabel8);
            dGrid.Children.Add(tLabel9);
            dGrid.Children.Add(tLabel10);
            dGrid.Children.Add(tLabel11);
            dGrid.Children.Add(tLabel12);
            dGrid.Children.Add(dLabel1);
            dGrid.Children.Add(dLabel2);
            dGrid.Children.Add(dLabel3);
            dGrid.Children.Add(dLabel4);
            dGrid.Children.Add(dLabel5);
            dGrid.Children.Add(dLabel6);
            dGrid.Children.Add(dLabel7);
            dGrid.Children.Add(dLabel8);
            dGrid.Children.Add(dLabel9);
            dGrid.Children.Add(dLabel10);
            dGrid.Children.Add(dLabel11);
            dGrid.Children.Add(dLabel12);

            cGroupBox.Content = dGrid;
            cGrid.Children.Add(cGroupBox);

            parentGrid.Children.Add(cGrid);
            MainWindow.myWindow.GetAvatars(nameSplit[0], nameSplit[1], MainWindow.myWindow.Server, dImage, cGroupBox);

            #endregion
        }
    }
}
