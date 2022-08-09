using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace shopy
{
    public partial class cashMemo : Form
    {
        public cashMemo()
        {
            InitializeComponent();
        }

        private void PrintReceipt()
        {
            PrinterSettings ps = new PrinterSettings();
            PrintDialog printDialog = new PrintDialog();
            PrintDocument printDoc = new PrintDocument();
            printDoc.PrinterSettings = ps;

            IEnumerable<PaperSize> paperSizes = ps.PaperSizes.Cast<PaperSize>();
            PaperSize sizeA5 = paperSizes.First<PaperSize>(size => size.Kind == PaperKind.A5);
            printDoc.DefaultPageSettings.PaperSize = sizeA5;

            printDialog.Document = printDoc;

            printDoc.PrintPage += new PrintPageEventHandler(PrintDoc_PrintPage);

            if (printDialog.ShowDialog() == DialogResult.OK)
            {
                printDoc.Print();
            }
        }

        private void PrintDoc_PrintPage(object sender, PrintPageEventArgs e)
        {
            Graphics g = e.Graphics;
            Font font = new Font("Times New Roman", 16f, FontStyle.Bold);
            float fontHeight = font.GetHeight();
            int startx = 30;
            int starty = 20;
            int offset = 5;

            if (Properties.Settings.Default.shopLogo == "")
            {
                //Shop Name
                g.DrawString(Properties.Settings.Default.shopName, font, new SolidBrush(Color.Black), startx, starty);
                font = new Font("Times New Roman", 11f, FontStyle.Regular);

                //Shop Address
                offset += font.Height + 5;
                g.DrawString(String.Format("{0}", Properties.Settings.Default.shopAddress), font, new SolidBrush(Color.Black), startx, starty + offset);


                //Receipt Date
                offset += font.Height + 5;
                g.DrawString(String.Format("Date:\t {0}", String.Format("{0:dd-MMM-yyyy}", Convert.ToDateTime(invoiceDateLabel.Text))), font, new SolidBrush(Color.Black), startx, starty + offset);

                //Invoice ID
                g.DrawString(String.Format("Invoice #: {0}", invoiceLabel.Text), font, new SolidBrush(Color.Black), startx + 320, starty + offset);

            }
            else
            {
                //Shop Logo
                g.DrawImage(GetImage(Properties.Settings.Default.shopLogo), new Rectangle(new Point(30, 20), new Size(64, 64)));

                //Shop Name
                g.DrawString(Properties.Settings.Default.shopName, font, new SolidBrush(Color.Black), startx + 80, starty);
                font = new Font("Times New Roman", 11f, FontStyle.Regular);

                //Shop Address
                offset += font.Height + 5;
                g.DrawString(String.Format("{0}", Properties.Settings.Default.shopAddress), font, new SolidBrush(Color.Black), startx + 80, starty + offset);


                //Receipt Date
                offset += font.Height + 5;
                g.DrawString(String.Format("Date:\t {0}", String.Format("{0:dd-MMM-yyyy}", Convert.ToDateTime(invoiceDateLabel.Text))), font, new SolidBrush(Color.Black), startx + 80, starty + offset);

                //Invoice ID
                g.DrawString(String.Format("Invoice #: {0}", invoiceLabel.Text), font, new SolidBrush(Color.Black), startx + 320, starty + offset);

            }


            offset += 5;
            //Customer Info
            font = new Font("Times New Roman", 10f, FontStyle.Regular);
            offset += font.Height + 5;
            g.DrawString(String.Format("Customer Name\t: {0}", invoiceCustNameLabel.Text), font, new SolidBrush(Color.Black), startx, starty + offset);
            offset += font.Height + 2;
            g.DrawString(String.Format("Customer Contact\t: {0}", invoiceCustContactLabel.Text), font, new SolidBrush(Color.Black), startx, starty + offset);
            offset += font.Height + 2;
            g.DrawString(String.Format("Customer Address\t: {0}", invoiceCustAddressLabel.Text), font, new SolidBrush(Color.Black), startx, starty + offset);
            offset += font.Height + 5;

            offset += 10;
            //Column Item
            font = new Font("Times New Roman", 10f, FontStyle.Bold);
            g.DrawString("Sl#", font, new SolidBrush(Color.Black), startx + 7, starty + offset);
            g.DrawString("Particulars", font, new SolidBrush(Color.Black), startx + 40, starty + offset);
            g.DrawString("Qty.", font, new SolidBrush(Color.Black), startx + 250, starty + offset);
            g.DrawString("Tax", font, new SolidBrush(Color.Black), startx + 288, starty + offset);
            g.DrawString("Tax Type", font, new SolidBrush(Color.Black), startx + 345, starty + offset);
            g.DrawString("Net Price", font, new SolidBrush(Color.Black), startx + 420, starty + offset);
            offset += font.Height + 5;

            //Draw Table
            g.DrawLine(new Pen(new SolidBrush(Color.Black), 1.5f), new Point(35, starty + offset - 3 - font.Height - 5), new Point(545, starty + offset - 3 - font.Height - 5));
            g.DrawLine(new Pen(new SolidBrush(Color.Black), 1.5f), new Point(35, starty + offset - 3), new Point(545, starty + offset - 3));
            //Vertical Line
            g.DrawLine(new Pen(new SolidBrush(Color.Black), 1.5f), new Point(35, 635), new Point(35, starty + offset - 3 - font.Height - 5));
            g.DrawLine(new Pen(new SolidBrush(Color.Black), 1.5f), new Point(545, 635), new Point(545, starty + offset - 3 - font.Height - 5));
            g.DrawLine(new Pen(new SolidBrush(Color.Black), 1.0f), new Point(60, 635), new Point(60, starty + offset - 3 - font.Height - 5)); //After Sl#
            g.DrawLine(new Pen(new SolidBrush(Color.Black), 1.0f), new Point(278, 635), new Point(278, starty + offset - 3 - font.Height - 5)); // After Particular
            g.DrawLine(new Pen(new SolidBrush(Color.Black), 1.0f), new Point(315, 635), new Point(315, starty + offset - 3 - font.Height - 5)); //After Quantity
            g.DrawLine(new Pen(new SolidBrush(Color.Black), 1.0f), new Point(373, 635), new Point(373, starty + offset - 3 - font.Height - 5)); //After Tax
            g.DrawLine(new Pen(new SolidBrush(Color.Black), 1.0f), new Point(448, 635), new Point(448, starty + offset - 3 - font.Height - 5)); //After Tax Type
            //Bottom Line
            g.DrawLine(new Pen(new SolidBrush(Color.Black), 1.5f), new Point(35, 635), new Point(545, 635));


            //Particular Item
            font = new Font("Times New Roman", 10f, FontStyle.Regular);
            for (int item = 0; item < listView1.Items.Count; item++)
            {
                g.DrawString(listView1.Items[item].SubItems[0].Text, font, new SolidBrush(Color.Black), startx + 10, starty + offset);                
                g.DrawString(String.Format("{0}", listView1.Items[item].SubItems[1].Text), font, new SolidBrush(Color.Black), startx + 40, starty + offset);
                g.DrawString(String.Format("{0}", listView1.Items[item].SubItems[2].Text), font, new SolidBrush(Color.Black), startx + 250, starty + offset);
                g.DrawString(String.Format("{0}%", listView1.Items[item].SubItems[4].Text), font, new SolidBrush(Color.Black), startx + 288, starty + offset);
                g.DrawString(String.Format("{0}", listView1.Items[item].SubItems[5].Text), font, new SolidBrush(Color.Black), startx + 345, starty + offset);
                g.DrawString(String.Format("₹ {0}", listView1.Items[item].SubItems[6].Text), font, new SolidBrush(Color.Black), startx + 420, starty + offset);
                offset += font.Height + 5;
            }

            //Give some space
            //offset += 20;
            offset = 620;
            //Subtotal & Net Total etc here       
            font = new Font("Times New Roman", 10f, FontStyle.Regular);
            g.DrawString(String.Format("Sub-Total\t: {0}", invoiceGrossLabel.Text), font, new SolidBrush(Color.Black), startx, starty + offset);
            g.DrawString(String.Format("Discount\t\t: {0}", invoiceDiscountLabel.Text), font, new SolidBrush(Color.Black), startx + 250, starty + offset);
            offset += font.Height + 2;
            g.DrawString(String.Format("Net-Total\t\t: {0}", invoiceNetLabel.Text), font, new SolidBrush(Color.Black), startx, starty + offset);
            offset += font.Height + 2;
            g.DrawString(String.Format("Paid\t\t: {0}", invoicePaidLabel.Text), font, new SolidBrush(Color.Black), startx, starty + offset);
            g.DrawString(String.Format("Return\t\t: {0}", invoiceReturnLabel.Text), font, new SolidBrush(Color.Black), startx + 250, starty + offset);
            offset += font.Height + 2;
            g.DrawString(String.Format("Status\t\t: {0}", invoiceStatusLabel.Text), font, new SolidBrush(Color.Black), startx, starty + offset);
            offset += font.Height + 2;

            font = new Font("Times New Roman", 10f, FontStyle.Italic);

            string isNegative = "";
            string number = invoiceNetLabel.Text.Remove(0, 2);
            if (number.Contains("-"))
            {
                isNegative = "Minus ";
                number = number.Substring(1, number.Length - 1);
            }
            if (number == "0")
            {
                g.DrawString("Zero Only", font, new SolidBrush(Color.Black), startx, starty + offset);
            }
            else
            {

                g.DrawString(String.Format("({0}{1})", isNegative, ConvertToWords(number)), font, new SolidBrush(Color.Black), startx, starty + offset);
            }

            offset += font.Height + 15;

            g.DrawString("*This is computer generated invoice.", font, new SolidBrush(Color.Black), startx, starty + offset);
            font = new Font("Times New Roman", 10f, FontStyle.Italic | FontStyle.Bold);
            offset += font.Height + 15;
            g.DrawString(String.Format("For {0},", Properties.Settings.Default.shopName), font, new SolidBrush(Color.Black), startx, starty + offset);
        }

        public Image GetImage(string value)
        {
            byte[] bytes = Convert.FromBase64String(value);
            Image image;
            using (MemoryStream ms = new MemoryStream(bytes))
            {
                image = Image.FromStream(ms);
            }
            return image;
        }

        private void PrintButton_Click(object sender, EventArgs e)
        {
            try { PrintReceipt(); }
            catch(Exception ex) { MessageBox.Show(ex.Message); }
        }

        private static String ConvertDecimals(String number)
        {
            String cd = "", digit = "", engOne = "";
            for (int i = 0; i < number.Length; i++)
            {
                digit = number[i].ToString();
                if (digit.Equals("0"))
                {
                    engOne = "Zero";
                }
                else
                {
                    engOne = ones(digit);
                }
                cd += " " + engOne;
            }
            return cd;
        }


        private static String ConvertToWords(String numb)
        {
            String val = "", wholeNo = numb, points = "", andStr = "", pointStr = "";
            String endStr = "Only";
            try
            {
                int decimalPlace = numb.IndexOf(".");
                if (decimalPlace > 0)
                {
                    wholeNo = numb.Substring(0, decimalPlace);
                    points = numb.Substring(decimalPlace + 1);
                    if (Convert.ToInt32(points) > 0)
                    {
                        andStr = "and";// just to separate whole numbers from points/paise  
                        endStr = "Paisa " + endStr;//paisa 
                        pointStr = ConvertDecimals(points);
                    }
                }
                val = String.Format("{0} {1}{2} {3}", ConvertWholeNumber(wholeNo).Trim(), andStr, pointStr, endStr);
            }
            catch { }
            return val;
        }

        private static String ConvertWholeNumber(String Number)
        {
            string word = "";
            try
            {
                bool beginsZero = false;//tests for 0XX  
                bool isDone = false;//test if already translated  
                double dblAmt = (Convert.ToDouble(Number));
                //if ((dblAmt > 0) && number.StartsWith("0"))  
                if (dblAmt > 0)
                {//test for zero or digit zero in a nuemric  
                    beginsZero = Number.StartsWith("0");

                    int numDigits = Number.Length;
                    int pos = 0;//store digit grouping  
                    String place = "";//digit grouping name:hundres,thousand,etc...  
                    switch (numDigits)
                    {
                        case 1://ones' range  

                            word = ones(Number);
                            isDone = true;
                            break;
                        case 2://tens' range  
                            word = tens(Number);
                            isDone = true;
                            break;
                        case 3://hundreds' range  
                            pos = (numDigits % 3) + 1;
                            place = " Hundred ";
                            break;
                        case 4://thousands' range  
                        case 5:
                        case 6:
                            pos = (numDigits % 4) + 1;
                            place = " Thousand ";
                            break;
                        case 7://millions' range  
                        case 8:
                        case 9:
                            pos = (numDigits % 7) + 1;
                            place = " Million ";
                            break;
                        case 10://Billions's range  
                        case 11:
                        case 12:

                            pos = (numDigits % 10) + 1;
                            place = " Billion ";
                            break;
                        //add extra case options for anything above Billion...  
                        default:
                            isDone = true;
                            break;
                    }
                    if (!isDone)
                    {//if transalation is not done, continue...(Recursion comes in now!!)  
                        if (Number.Substring(0, pos) != "0" && Number.Substring(pos) != "0")
                        {
                            try
                            {
                                word = ConvertWholeNumber(Number.Substring(0, pos)) + place + ConvertWholeNumber(Number.Substring(pos));
                            }
                            catch { }
                        }
                        else
                        {
                            word = ConvertWholeNumber(Number.Substring(0, pos)) + ConvertWholeNumber(Number.Substring(pos));
                        }

                        //check for trailing zeros  
                        //if (beginsZero) word = " and " + word.Trim();  
                    }
                    //ignore digit grouping names  
                    if (word.Trim().Equals(place.Trim())) word = "";
                }
            }
            catch { }
            return word.Trim();
        }


        private static String tens(String Number)
        {
            int _Number = Convert.ToInt32(Number);
            String name = null;
            switch (_Number)
            {
                case 10:
                    name = "Ten";
                    break;
                case 11:
                    name = "Eleven";
                    break;
                case 12:
                    name = "Twelve";
                    break;
                case 13:
                    name = "Thirteen";
                    break;
                case 14:
                    name = "Fourteen";
                    break;
                case 15:
                    name = "Fifteen";
                    break;
                case 16:
                    name = "Sixteen";
                    break;
                case 17:
                    name = "Seventeen";
                    break;
                case 18:
                    name = "Eighteen";
                    break;
                case 19:
                    name = "Nineteen";
                    break;
                case 20:
                    name = "Twenty";
                    break;
                case 30:
                    name = "Thirty";
                    break;
                case 40:
                    name = "Fourty";
                    break;
                case 50:
                    name = "Fifty";
                    break;
                case 60:
                    name = "Sixty";
                    break;
                case 70:
                    name = "Seventy";
                    break;
                case 80:
                    name = "Eighty";
                    break;
                case 90:
                    name = "Ninety";
                    break;
                default:
                    if (_Number > 0)
                    {
                        name = tens(Number.Substring(0, 1) + "0") + " " + ones(Number.Substring(1));
                    }
                    break;
            }
            return name;
        }

        private static String ones(String Number)
        {
            int _Number = Convert.ToInt32(Number);
            String name = "";
            switch (_Number)
            {

                case 1:
                    name = "One";
                    break;
                case 2:
                    name = "Two";
                    break;
                case 3:
                    name = "Three";
                    break;
                case 4:
                    name = "Four";
                    break;
                case 5:
                    name = "Five";
                    break;
                case 6:
                    name = "Six";
                    break;
                case 7:
                    name = "Seven";
                    break;
                case 8:
                    name = "Eight";
                    break;
                case 9:
                    name = "Nine";
                    break;
            }
            return name;
        }
    }
}
