using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Security.Permissions;
using System.Windows;

namespace B_Stock
{
    class SQLFunc
    {
        SqlConnection server_cn = new SqlConnection(B_Stock.Properties.Settings.Default.strConn);

        private static SqlDataAdapter SDA;
        private static DataSet DS;
        //Units UnitData;
        string[] txtList;

        public SQLFunc()//Units tmpData)
        {
            //UnitData = tmpData;
        }

        public bool GetSNData(Units tmpData)//string tmpSN, string tmpAPN)
        {
            using (SqlConnection cn = new SqlConnection(B_Stock.Properties.Settings.Default.strConn))
            {
                cn.Open();

                using (SqlCommand QryCmd = new SqlCommand())
                {
                    QryCmd.CommandText = " SELECT A.NewQTA_SN, A.QTA_SN, A.RMACaseID, A.TKNo, A.CustPN, A.QTAModel, B.* " +
                                         " FROM datRMAEquipments A, PALRMA_MODEL_CONFIG_TE B " +
                                         " WHERE (A.QTA_SN = '" + tmpData.SN + "' OR A.NewQTA_SN = '" + tmpData.SN + "') " +
                                         " AND A.CustPN = '" + tmpData.APN + "' " +
                                         " AND A.DefectivePart = 'System' " +
                                         " AND A.CustPN = B.APN ";
                    SDA = new SqlDataAdapter(QryCmd.CommandText, cn);
                    DS = new DataSet();
                    SDA.Fill(DS, "UNITS");
                    if (DS.Tables["UNITS"].Rows.Count > 0)
                    {
                        tmpData.RMASN = DS.Tables["UNITS"].Rows[0]["NewQTA_SN"].ToString();
                        tmpData.SYSSN = DS.Tables["UNITS"].Rows[0]["QTA_SN"].ToString();
                        tmpData.TKNo = DS.Tables["UNITS"].Rows[0]["TKNo"].ToString();
                        tmpData.RMANo = DS.Tables["UNITS"].Rows[0]["RMACaseID"].ToString();
                        tmpData.Model = DS.Tables["UNITS"].Rows[0]["QTAModel"].ToString();
                        tmpData.CustPN = DS.Tables["UNITS"].Rows[0]["CustPN"].ToString();
                        tmpData.CONFIG = DS.Tables["UNITS"].Rows[0]["CONFIG"].ToString();
                        tmpData.UPC = DS.Tables["UNITS"].Rows[0]["UPC"].ToString();
                        tmpData.FamilyNo = DS.Tables["UNITS"].Rows[0]["FAMILY"].ToString();

                        cn.Close();
                        return true;
                    }
                    else
                    {
                        cn.Close();
                        return false;
                    }
                }
            }
            
        }
        public bool checkStatus(Units tmpData)
        {
            using (SqlConnection cn = new SqlConnection(B_Stock.Properties.Settings.Default.strConn))
            {
                cn.Open();

                using (SqlCommand QryCmd = new SqlCommand())
                {
                    QryCmd.CommandText = " SELECT TOP 1 * " +
                                         " FROM logEquipmentProcFlow " +
                                         " WHERE RMACaseID + TKNo = '" + tmpData.RMANo + tmpData.TKNo + "' ";// +
                                         //" AND Station = 'QC003' ";
                    SDA = new SqlDataAdapter(QryCmd.CommandText, cn);
                    DS = new DataSet();
                    SDA.Fill(DS, "UNITSTATUS");
					
                    if (DS.Tables["UNITSTATUS"].Rows.Count > 0)
                    {
						if(DS.Tables["UNITSTATUS"].Rows[0]["NextStation"].ToString() == "PACKING")
						{
							tmpData.flgStatus = true;
							cn.Close();
							return true;
						}
						else
						{
							tmpData.flgStatus = false;
							cn.Close();
							return false;
						}
                    }
                    else
                    {
						tmpData.flgStatus = false;
                        cn.Close();
                        return false;
                    }
                }
            }
            //return false;
        }
        public bool IsFileExists(Units tmpData)
        {
            string SN = tmpData.RMASN.Substring(0, 11) + "*.*";
            try
            {
                FileIOPermission f2;
                if (File.Exists(@"I:\" + tmpData.RMASN))
                {
                    //txtList = Directory.GetFiles(@"C:\Users\quyen\Desktop\test\", "*.*", SearchOption.AllDirectories);
                    //foreach (string file in txtList)
                    //{
                    File.Copy(@"I:\" + tmpData.RMASN, @"\\192.168.89.208\Log\" + tmpData.RMASN);
                    //File.Copy(@"I:\" + tmpData.RMASN, @"C:\Users\quyen\Desktop\dest\" + tmpData.RMASN);
                    //}
                }
                return true;
            }
            catch (DirectoryNotFoundException dirNotFound)
            {
                MessageBox.Show(dirNotFound.Message);
            }
            return false;
        }
    }
}
