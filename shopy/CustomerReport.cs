using MySql.Data.MySqlClient;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static shopy.ConnectionString;


namespace shopy
{
    public partial class CustomerReport : Form
    {
        public CustomerReport()
        {
            InitializeComponent();
        }

        private void CustomerReport_Load(object sender, EventArgs e)
        {
            try
            {        
              
                 if (connection.State == ConnectionState.Closed) { connection.Open(); }
                 command = new MySqlCommand("SELECT DISTINCT(customername), customercontact, customeraddress FROM customertb ORDER BY customername", connection);
                 reader = command.ExecuteReader();

                listView1.Clear();
                listView1.Columns.Add("Sl No", 65, HorizontalAlignment.Left);
                listView1.Columns.Add("Customer Name", 220, HorizontalAlignment.Center);
                listView1.Columns.Add("Customer Contact", 175, HorizontalAlignment.Center);
                listView1.Columns.Add("Address", 265, HorizontalAlignment.Center);
                listView1.Font = new Font("Times New Roman", 12f, FontStyle.Bold);

                while (reader.Read())
                 {
                    ListViewItem lvItem = new ListViewItem();
                    lvItem.SubItems[0].Text = (listView1.Items.Count + 1).ToString();
                    lvItem.SubItems[0].Font = new Font(new Font("Consolas", 10f), FontStyle.Regular);
                    lvItem.SubItems.Add(reader[0].ToString());
                    lvItem.SubItems[1].Font = new Font(new Font("Consolas", 10f), FontStyle.Regular);
                    lvItem.SubItems.Add(reader[1].ToString());
                    lvItem.SubItems[2].Font = new Font(new Font("Consolas", 10f), FontStyle.Regular);
                    lvItem.SubItems.Add(reader[2].ToString());
                    lvItem.SubItems[3].Font = new Font(new Font("Consolas", 10f), FontStyle.Regular);
                    
                    listView1.Items.Add(lvItem);
                }
                 reader.Dispose();               


            }
            catch (Exception ex) { MessageBox.Show(ex.Message);  }
            finally { connection.Close(); }
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            try
            {
                using (SaveFileDialog sf = new SaveFileDialog())
                {
                    sf.Filter = "Spreadsheet|*.xlsx";
                    if (sf.ShowDialog() == DialogResult.OK)
                    {
                        var file = new FileInfo(sf.FileName);                       

                        using (ExcelPackage package = new ExcelPackage(file))
                        {
                            ExcelWorksheet worksheet = package.Workbook.Worksheets.Add(String.Format("Customer Report {0:yyMMdHHmm}", DateTime.Now));

                            worksheet.Cells["B5"].Value = "Sl No";
                            worksheet.Cells["C5"].Value = "Customer Name";
                            worksheet.Cells["D5"].Value = "Customer Contact";
                            worksheet.Cells["E5"].Value = "Customer Address";
                            worksheet.Cells["B5:E5"].Style.Font.Bold = true;
                            worksheet.Cells["B5:E5"].Style.Font.Size = 14;
                            worksheet.Cells["B5:E5"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                            worksheet.Cells["C5"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                            worksheet.Column(2).AutoFit();
                            worksheet.Column(3).AutoFit();
                            worksheet.Column(4).AutoFit();
                            worksheet.Column(5).AutoFit();

                            //worksheet.Cells["A1"].LoadFromCollection(myColl, true, OfficeOpenXml.Table.TableStyles.Medium1);
                            int rowNumber = 6;
                            for (int i = 0; i < listView1.Items.Count; i++)
                            {
                                worksheet.Cells[rowNumber, 2].Value = listView1.Items[i].SubItems[0].Text;                                
                                worksheet.Cells[rowNumber, 3].Value = listView1.Items[i].SubItems[1].Text;
                                worksheet.Cells[rowNumber, 4].Value = listView1.Items[i].SubItems[2].Text;
                                worksheet.Cells[rowNumber, 5].Value = listView1.Items[i].SubItems[3].Text;
                                using (var range = worksheet.Cells[rowNumber, 2, rowNumber, 4])
                                {
                                    range.Style.Font.Bold = false;
                                    worksheet.Cells["B5:E5"].Style.Font.Size = 12;
                                    range.Style.Font.Color.SetColor(Color.Black);
                                    range.Style.ShrinkToFit = false;
                                }
                                rowNumber++;
                            }
                            package.Save();
                        }
                        
                    }
                }

            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
            
        }
    }
}
