using MySql.Data.MySqlClient;
using OfficeOpenXml;
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
    public partial class TransactionReport : Form
    {
        public TransactionReport()
        {
            InitializeComponent();
        }

        private void LoadButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (Convert.ToDateTime(dateFrom.Text) <= Convert.ToDateTime(dateTo.Text))
                {
                    if (connection.State == ConnectionState.Closed) { connection.Open(); }

                    command = new MySqlCommand(String.Format("SELECT ivr.* FROM invoicerec AS ivr JOIN invoicetb AS ivt WHERE ivr.invoiceid = ivt.invoiceid AND ivr.invoicedate >= '{0}' AND ivr.invoicedate <= '{1}';", String.Format("{0:yyyy-MM-dd}", Convert.ToDateTime(dateFrom.Text)), String.Format("{0:yyyy-MM-dd}", Convert.ToDateTime(dateTo.Text))), connection);
                    reader = command.ExecuteReader();

                    listView1.Clear();
                    listView1.Columns.Add("Invoice #", 105, HorizontalAlignment.Left);
                    listView1.Columns.Add("Invoice Date", 120, HorizontalAlignment.Center);
                    listView1.Columns.Add("Sub Total", 125, HorizontalAlignment.Center);
                    listView1.Columns.Add("Discount", 105, HorizontalAlignment.Center);
                    listView1.Columns.Add("Net Total", 125, HorizontalAlignment.Center);
                    listView1.Columns.Add("Paid Amt", 75, HorizontalAlignment.Center);
                    listView1.Columns.Add("Return Amt", 75, HorizontalAlignment.Center);
                    listView1.Columns.Add("Status", 105, HorizontalAlignment.Center);
                    listView1.Font = new Font("Times New Roman", 12f, FontStyle.Bold);

                    while (reader.Read())
                    {
                        ListViewItem lvItem = new ListViewItem();
                        lvItem.SubItems[0].Text = (reader[0].ToString());
                        lvItem.SubItems[0].Font = new Font(new Font("Consolas", 10f), FontStyle.Regular);
                        lvItem.SubItems.Add(String.Format("{0:dd-MM-yyyy}", Convert.ToDateTime(reader[1])));
                        lvItem.SubItems[1].Font = new Font(new Font("Consolas", 10f), FontStyle.Regular);
                        lvItem.SubItems.Add(reader[2].ToString());
                        lvItem.SubItems[2].Font = new Font(new Font("Consolas", 10f), FontStyle.Regular);
                        lvItem.SubItems.Add(reader[3].ToString());
                        lvItem.SubItems[3].Font = new Font(new Font("Consolas", 10f), FontStyle.Regular);
                        lvItem.SubItems.Add(String.Format("{0}", reader[4]));
                        lvItem.SubItems[4].Font = new Font(new Font("Consolas", 10f), FontStyle.Regular);
                        lvItem.SubItems.Add(reader[5].ToString());
                        lvItem.SubItems[5].Font = new Font(new Font("Consolas", 10f), FontStyle.Regular);
                        lvItem.SubItems.Add(reader[6].ToString());
                        lvItem.SubItems[6].Font = new Font(new Font("Consolas", 10f), FontStyle.Regular);
                        lvItem.SubItems.Add(reader[7].ToString());
                        lvItem.SubItems[7].Font = new Font(new Font("Consolas", 10f), FontStyle.Regular);


                        listView1.Items.Add(lvItem);
                    }
                    reader.Dispose();
                }
                else { return; }



            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
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
                            ExcelWorksheet worksheet = package.Workbook.Worksheets.Add(String.Format("Transaction Report {0:yyMMdHHmm}", DateTime.Now));

                            //Header
                            worksheet.Cells["B1"].Value = "GST REPORT";
                            worksheet.Cells["B1:I1"].Style.Font.SetFromFont(new Font("Times New Roman", 16f, FontStyle.Bold));
                            worksheet.Cells["B2"].Value = String.Format("For the Period of {0} to {1}", dateFrom.Text, dateTo.Text);
                            worksheet.Cells["B2:I2"].Style.Font.SetFromFont(new Font("Times New Roman", 14f, FontStyle.Bold | FontStyle.Italic));
                            worksheet.Cells["B1:I1"].Merge = true;
                            worksheet.Cells["B2:I2"].Merge = true;
                            worksheet.Cells["B1:I2"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                            worksheet.Cells["B5"].Value = "Invoice #";
                            worksheet.Cells["C5"].Value = "Invoice Date";
                            worksheet.Cells["D5"].Value = "Sub Total";
                            worksheet.Cells["E5"].Value = "Discount";
                            worksheet.Cells["F5"].Value = "Net Total";
                            worksheet.Cells["G5"].Value = "Paid Amount";
                            worksheet.Cells["H5"].Value = "Return Amount";
                            worksheet.Cells["I5"].Value = "Status";
                            worksheet.Cells["B5:I5"].Style.Font.SetFromFont(new Font("Times New Roman", 14f, FontStyle.Bold));
                            worksheet.Cells["B5:I5"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                            

                            //worksheet.Cells["A1"].LoadFromCollection(myColl, true, OfficeOpenXml.Table.TableStyles.Medium1);
                            int rowNumber = 6;
                            for (int i = 0; i < listView1.Items.Count; i++)
                            {
                                worksheet.Cells[rowNumber, 2].Value = listView1.Items[i].SubItems[0].Text;
                                worksheet.Cells[rowNumber, 3].Value = listView1.Items[i].SubItems[1].Text;
                                worksheet.Cells[rowNumber, 4].Value = listView1.Items[i].SubItems[2].Text;
                                worksheet.Cells[rowNumber, 5].Value = listView1.Items[i].SubItems[3].Text;
                                worksheet.Cells[rowNumber, 6].Value = listView1.Items[i].SubItems[4].Text;
                                worksheet.Cells[rowNumber, 7].Value = listView1.Items[i].SubItems[5].Text;
                                worksheet.Cells[rowNumber, 8].Value = listView1.Items[i].SubItems[6].Text;
                                worksheet.Cells[rowNumber, 9].Value = listView1.Items[i].SubItems[7].Text;
                                using (var range = worksheet.Cells[rowNumber, 2, rowNumber, 9])
                                {
                                    range.Style.Font.SetFromFont(new Font("Times New Roman", 12f, FontStyle.Regular));
                                    range.Style.Font.Color.SetColor(Color.Black);
                                    range.Style.ShrinkToFit = false;
                                }
                                worksheet.Cells[rowNumber, 2].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                                worksheet.Cells[rowNumber, 3].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                                worksheet.Cells[rowNumber, 4].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;
                                worksheet.Cells[rowNumber, 5].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;
                                worksheet.Cells[rowNumber, 6].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;
                                worksheet.Cells[rowNumber, 7].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;
                                worksheet.Cells[rowNumber, 8].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;
                                worksheet.Cells[rowNumber, 9].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;

                                rowNumber++;
                            }

                            worksheet.Column(2).AutoFit();
                            worksheet.Column(3).AutoFit();
                            worksheet.Column(4).AutoFit();
                            worksheet.Column(5).AutoFit();
                            worksheet.Column(6).AutoFit();
                            worksheet.Column(7).AutoFit();
                            worksheet.Column(8).AutoFit();
                            worksheet.Column(9).AutoFit();

                            package.Save();
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
