using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using DocumentFormat.OpenXml;
using ClosedXML;
using ClosedXML.Excel;

namespace B_Stock
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        SqlConnection server_cn = new SqlConnection(B_Stock.Properties.Settings.Default.strConn);

        Units UnitData;
        SQLFunc func;

        public MainWindow()
        {
            InitializeComponent();

            func = new SQLFunc();
            UnitData = new Units();

            tbAPN.Focus();
        }

        
        private void CreateExcel(DataTable dt,string WorksheetName)
        {
            XLWorkbook wb = new XLWorkbook();
            wb.Worksheets.Add(dt, WorksheetName);
        }
        private void PrintLabel()
        {
            string Region = (UnitData.APN.Remove(UnitData.APN.Length - 2)).Remove(0,5);

            switch(Region)
            {
                case "LL":

                    break;
                case "B":

                    break;
                case "Y":

                    break;
                case "D":

                    break;
                case "T":

                    break;
            }
        }
        private void tbAPN_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                tbSN.Focus();
                UnitData.APN = tbAPN.Text;
            }
            else
                return;
        }
        private void tbSN_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                UnitData.SN = tbSN.Text;
                func.GetSNData(UnitData);

                tbTKNo.Text = UnitData.TKNo.ToString();
                tbRMA_No.Text = UnitData.RMANo;
                tbRMA_SN.Text = UnitData.RMASN;
                tbSYSSN.Text = UnitData.SYSSN;
                tbCONFIG.Text = UnitData.CONFIG;
                tbFamilyNo.Text = UnitData.FamilyNo;
                tbUPC.Text = UnitData.UPC;
                tbModel.Text = UnitData.Model;
                tbCustPN.Text = UnitData.CustPN;
                //PrintLabel();
				
				//if(func.checkStatus(UnitData))
				//{
                    if(func.IsFileExists(UnitData))
                        MessageBox.Show("Ode");
                    else
                        MessageBox.Show("Eo Ode");
    //            }
				//else
				//	MessageBox.Show("Eo Ode");
            }
            else
                return;
        }
        private void tbAPN_LostFocus(object sender, RoutedEventArgs e)
        {

        }
        private void tbSN_LostFocus(object sender, RoutedEventArgs e)
        {
            //if (tbSN.Text.Length == 12)
            //{
            //    UnitData.SN = tbSN.Text;
            //    SQLFunc func = new SQLFunc();
            //    func.GetSNData(UnitData);
            //    tbTKNo.Text = UnitData.TKNo.ToString();
            //}
            //else
            //    return;

        }
        private void btExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void btClear_Click(object sender, RoutedEventArgs e)
        {
            foreach (TextBox tb in FindVisualChildren<TextBox>(this))
            {
                tb.Clear();
            }
            UnitData = new Units();

            tbAPN.Focus();
        }
        public static IEnumerable<T> FindVisualChildren<T>(DependencyObject depObj) where T : DependencyObject
        {
            if (depObj != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(depObj, i);
                    if (child != null && child is T)
                    {
                        yield return (T)child;
                    }

                    foreach (T childOfChild in FindVisualChildren<T>(child))
                    {
                        yield return childOfChild;
                    }
                }
            }
        }


    }


    public class Units
    {
        public string SN        { get; set; }
        public string APN       { get; set; }
        public string UPC       { get; set; }
        public string TKNo      { get; set; }
        public string RMANo     { get; set; }
        public string RMASN     { get; set; }
        public string SYSSN     { get; set; }
        public string Model     { get; set; }
        public string CONFIG    { get; set; }
        public string BT_Mac    { get; set; }
        public string AirMac    { get; set; }
        public string CustPN    { get; set; }
        public string MLB_SN    { get; set; }
        public string Region    { get; set; }
        public string EtherMac  { get; set; }
        public string FamilyNo  { get; set; }
        public string SizeType  { get; set; }
        public string SIPROM_MAC { get; set; }
        public string Flag      { get; set; }
        public bool   flgStatus { get; set; }
        public bool   flgDW     { get; set; }

    }
}
