using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static shopy.ConnectionString;

namespace shopy
{
    public partial class MainForm : Form
    {

        StringBuilder itemlist = new StringBuilder();
        List<String> taxtypelist = new List<String>();

        String invoiceid = "0";
        public MainForm()
        {
            SetStyle(ControlStyles.ResizeRedraw, true);
            InitializeComponent();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            if (MenuVertical.Width == 57)
            {
                MenuVertical.Width = 225;
                MenuButton.Image = Properties.Resources.W32_02;
            }
            else
            {
                MenuVertical.Width = 57;
                MenuButton.Image = Properties.Resources.ic_dehaze_128;
            }
                
        }

        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();
        [DllImport("user32.DLL", EntryPoint = "SendMessage")]

        private extern static void SendMessage(System.IntPtr hWnd, int wMsg, int wParam, int lParam);


        private const int cGrip = 16;      // Grip size
        private const int cCaption = 32;   //Caption Bar Height


        private void headerArea_MouseMove(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }

        protected override void WndProc(ref Message msj)
        {
            if (msj.Msg == 0x84)
            {  // Trap WM_NCHITTEST
                Point pos = new Point(msj.LParam.ToInt32());
                pos = this.PointToClient(pos);
                if (pos.Y < cCaption)
                {
                    msj.Result = (IntPtr)2;  // HTCAPTION
                    return;
                }
                if (pos.X >= this.ClientSize.Width - cGrip && pos.Y >= this.ClientSize.Height - cGrip)
                {
                    msj.Result = (IntPtr)17; // HTBOTTOMRIGHT
                    return;
                }
            }
            
            const int CoordenadaWFP = 0x84; //ibicacion de la parte derecha inferior del form
            const int DesIzquierda = 16;
            const int DesDerecha = 17;
            if (msj.Msg == CoordenadaWFP)
            {
                int x = (int)(msj.LParam.ToInt64() & 0xFFFF);
                int y = (int)((msj.LParam.ToInt64() & 0xFFFF0000) >> 16);
                Point CoordenadaArea = PointToClient(new Point(x, y));
                Size TamañoAreaForm = ClientSize;
                if (CoordenadaArea.X >= TamañoAreaForm.Width - 16 && CoordenadaArea.Y >= TamañoAreaForm.Height - 16 && TamañoAreaForm.Height >= 16)
                {
                    msj.Result = (IntPtr)(IsMirrored ? DesIzquierda : DesDerecha);
                    return;
                }
            }
            base.WndProc(ref msj);
        }

        private void iconCerrar_Click(object sender, EventArgs e)
        {
            Application.Exit();
            if (MessageBox.Show("Are you sure to shut the application?", "Shutting Down?", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                //Quiting
                Application.Exit();
            }
            else
            {
                //Cancel
            }
        }

        private void iconMinimizar_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void iconmaximize_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
            this.iconmaximize.Visible = false;
            this.iconrestore.Visible = true;
        }

        private void iconrestore_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Normal;
            this.iconmaximize.Visible = true;
            this.iconrestore.Visible = false;
        }

        private void DashboardButton_MouseHover(object sender, EventArgs e)
        {
            try
            {
                
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void DashboardButton_Click(object sender, EventArgs e)
        {
            try
            {
                DashboardButton.BackColor = Color.FromArgb(70,10,61);
                SaleButton.BackColor = Color.FromArgb(34,0,51);
                ReportButton.BackColor = Color.FromArgb(34, 0, 51);
                SettingButton.BackColor = Color.FromArgb(34, 0, 51);

                DashboardButton.ForeColor = Color.DeepPink;
                SaleButton.ForeColor = Color.White;
                ReportButton.ForeColor = Color.White;
                SettingButton.ForeColor = Color.White;

                //SidePanel
                SidePanel.Height = DashboardButton.Height;
                SidePanel.Top = DashboardButton.Top;

                //Tab Control
                tabControl1.SelectTab(0);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }      


        private void SaleButton_Click(object sender, EventArgs e)
        {
            try
            {
                //Changing the Background Color
                DashboardButton.BackColor = Color.FromArgb(34,0,51);
                SaleButton.BackColor = Color.FromArgb(70, 10, 61);
                ReportButton.BackColor = Color.FromArgb(34, 0, 51);
                SettingButton.BackColor = Color.FromArgb(34, 0, 51);

                //Changing the Foreground Color
                DashboardButton.ForeColor = Color.White;
                SaleButton.ForeColor = Color.DeepPink;
                ReportButton.ForeColor = Color.White;
                SettingButton.ForeColor = Color.White;

                //SidePanel
                SidePanel.Height = SaleButton.Height;
                SidePanel.Top = SaleButton.Top;


                //Tab Control
                //Tab Control
                tabControl1.SelectTab(1);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void ReportButton_Click(object sender, EventArgs e)
        {
            try
            {
                DashboardButton.BackColor = Color.FromArgb(34, 0, 51);
                SaleButton.BackColor = Color.FromArgb(34,0,51);
                ReportButton.BackColor = Color.FromArgb(70, 10, 61);
                SettingButton.BackColor = Color.FromArgb(34, 0, 51);

                DashboardButton.ForeColor = Color.White;
                SaleButton.ForeColor = Color.White;
                ReportButton.ForeColor = Color.DeepPink;
                SettingButton.ForeColor = Color.White;

                //SidePanel
                SidePanel.Height = ReportButton.Height;
                SidePanel.Top = ReportButton.Top;

                //Tab Control
                //Tab Control
                tabControl1.SelectTab(2);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
             

        private void DashboardButton_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                DashboardButton.BackColor = Color.FromArgb(57,76,132);
                SaleButton.BackColor = Color.FromArgb(34, 0, 51);
                ReportButton.BackColor = Color.FromArgb(34, 0, 51);
                SettingButton.BackColor = Color.FromArgb(34, 0, 51);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void SaleButton_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                DashboardButton.BackColor = Color.FromArgb(34, 0, 51);
                SaleButton.BackColor = Color.FromArgb(57, 76, 132);
                ReportButton.BackColor = Color.FromArgb(34, 0, 51);
                SettingButton.BackColor = Color.FromArgb(34, 0, 51);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void ReportButton_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                DashboardButton.BackColor = Color.FromArgb(34, 0, 51);
                SaleButton.BackColor = Color.FromArgb(34, 0, 51);
                ReportButton.BackColor = Color.FromArgb(57, 76, 132);
                SettingButton.BackColor = Color.FromArgb(34, 0, 51);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void loadcustomerstatus()
        {
            try
            {
                if(connection.State == ConnectionState.Closed) { connection.Open(); }

                command = new MySqlCommand("SELECT COUNT(status) FROM invoicerec WHERE status = 'Partial Paid'", connection);
                long i = (long)command.ExecuteScalar();
                label37.Text = i.ToString("D08");
                command = new MySqlCommand("SELECT COUNT(status) FROM invoicerec WHERE status = 'Cancelled'", connection);
                i = (long)command.ExecuteScalar();
                label35.Text = i.ToString("D08");
                command = new MySqlCommand("SELECT COUNT(status) FROM invoicerec WHERE status = 'Pending'", connection);
                i = (long)command.ExecuteScalar();
                label33.Text = i.ToString("D08");
                command = new MySqlCommand("SELECT COUNT(status) FROM invoicerec WHERE status = 'Exchanged'", connection);
                i = (long)command.ExecuteScalar();
                label39.Text = i.ToString("D08");
                command = new MySqlCommand("SELECT COUNT(status) FROM invoicerec WHERE status = 'Paid'", connection);
                i = (long)command.ExecuteScalar();
                label41.Text = i.ToString("D08");
                command = new MySqlCommand("SELECT COUNT(DISTINCT(customername)) FROM customertb", connection);
                i = (long)command.ExecuteScalar();
                label31.Text = i.ToString("D08");
                command = new MySqlCommand("SELECT SUM(parttaxamount) FROM invoicetb", connection);
                decimal x = (decimal)command.ExecuteScalar();
                label28.Text = String.Format("₹ {0}", x);
                command = new MySqlCommand("SELECT SUM(nettotal) FROM invoicerec", connection);
                x = (decimal)command.ExecuteScalar() - x;
                label29.Text = String.Format("₹ {0}", x);
            }
            catch(Exception ex) { MessageBox.Show(ex.Message); }
            finally { connection.Close(); }
        }

        private void menuButton_Load(object sender, EventArgs e)
        {
            try
            {
                //Load Setting
                this.Size = Properties.Settings.Default.SizeSetting;
                this.Location = Properties.Settings.Default.LocationSetting;
                if(Properties.Settings.Default.isMenuClosed == false)
                {
                    MenuVertical.Width = 225;
                }
                else { MenuVertical.Width = 57; }
                if(this.Size == Screen.PrimaryScreen.Bounds.Size)
                {
                    iconmaximize.Hide();
                    iconrestore.Show();
                }

                //Set Shop Info Setting
                shopNameBox.Text = Properties.Settings.Default.shopName;
                shopAddressBox.Text = Properties.Settings.Default.shopAddress;
                if(Properties.Settings.Default.shopLogo != "")
                {
                    shopLogo.Image = GetImage(Properties.Settings.Default.shopLogo);
                }

                //Set Prokey
                proKeyBox.Text = Properties.Settings.Default.ProKey;

                //Set the Dashboard By Default
                DashboardButton.BackColor = Color.FromArgb(70, 10, 61);
                SaleButton.BackColor = Color.FromArgb(34, 0, 51);
                ReportButton.BackColor = Color.FromArgb(34, 0, 51);
                SettingButton.BackColor = Color.FromArgb(34, 0, 51);

                //Set the Button Color
                DashboardButton.ForeColor = Color.DeepPink;
                SaleButton.ForeColor = Color.White;
                ReportButton.ForeColor = Color.White;
                SettingButton.ForeColor = Color.White;

                LogoBox.Visible = true;

                tabControl1.SizeMode = TabSizeMode.Fixed;
                tabControl1.ItemSize = new Size(0, 1);

                
                rate.ResetText();
                tax.ResetText();

                //Call for Customer Payment Status
                loadcustomerstatus();

                if(connection.State == ConnectionState.Closed) { connection.Open();  }

                //reader.Dispose();
                command = new MySqlCommand("SELECT DISTINCT(partname) FROM particulartb ORDER BY partname ASC", connection);
                reader = command.ExecuteReader();

                while (reader.Read())
                {
                    part.Items.Add(reader[0].ToString());
                    partName.Items.Add(reader[0].ToString());
                }

                loadlistparticulars();
                connection.Close();

                if(Properties.Settings.Default.isPro == false || Properties.Settings.Default.ProKey == "0")
                {
                    AboutNag abt = new AboutNag();
                    abt.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        
        private void Setting_Click(object sender, EventArgs e)
        {
            try
            {
                DashboardButton.BackColor = Color.FromArgb(34, 0, 51);
                SaleButton.BackColor = Color.FromArgb(34, 0, 51);
                ReportButton.BackColor = Color.FromArgb(34, 0, 51);  
                SettingButton.BackColor = Color.FromArgb(70, 10, 61);
                
                DashboardButton.ForeColor = Color.White;
                SaleButton.ForeColor = Color.White;
                ReportButton.ForeColor = Color.White;
                SettingButton.ForeColor = Color.DeepPink;

                //SidePanel
                SidePanel.Height = SettingButton.Height;
                SidePanel.Top = SettingButton.Top;

                
                //Tab Control
                tabControl1.SelectTab(3);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Setting_MouseHover(object sender, EventArgs e)
        {
            
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Rectangle rc = new Rectangle(this.ClientSize.Width - cGrip, this.ClientSize.Height - cGrip, cGrip, cGrip);
            ControlPaint.DrawSizeGrip(e.Graphics, this.BackColor, rc);
            rc = new Rectangle(0, 0, this.ClientSize.Width, cCaption);
            e.Graphics.FillRectangle(Brushes.DarkBlue, rc);
        }

        
        private void SettingButton_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                DashboardButton.BackColor = Color.FromArgb(34, 0, 51);
                SaleButton.BackColor = Color.FromArgb(34, 0, 51);
                ReportButton.BackColor = Color.FromArgb(34, 0, 51);
                SettingButton.BackColor = Color.FromArgb(57, 76, 132);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void headerArea_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                if(this.WindowState == FormWindowState.Normal)
                {
                    this.WindowState = FormWindowState.Maximized;
                    iconmaximize.Visible = false;
                    iconrestore.Visible = true;

                }
                else if(this.WindowState == FormWindowState.Maximized)
                {
                    this.WindowState = FormWindowState.Normal;
                    iconmaximize.Visible = true;
                    iconrestore.Visible = false;
                }
                else
                {
                    this.WindowState = FormWindowState.Normal;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                Properties.Settings.Default.SizeSetting = this.Size;
                Properties.Settings.Default.LocationSetting = this.Location;
                if(MenuVertical.Width == 57) { Properties.Settings.Default.isMenuClosed = true; }
                else { Properties.Settings.Default.isMenuClosed = false; }
                Properties.Settings.Default.Save();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private string ToNameText(string name = "")
        {
            try
            {
                TextInfo myTI = new CultureInfo("en-US", false).TextInfo;
                return (myTI.ToTitleCase(name).ToString());
            }
            catch (Exception ex)
            { MessageBox.Show(ex.Message, "Error"); return name; }

        }

        private void custName_Click(object sender, EventArgs e)
        {
            try
            {
                custNameLabel.Location = new Point(64, 50);
                custNameLabel.Font = new Font("Microsoft Sans Serif", 10, FontStyle.Regular);
                custNameLabel.ForeColor = Color.Gray;
                custName.Focus();
                custNameLabel.Cursor = Cursors.Default;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void custName_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                if (custName.Text == String.Empty)
                {
                    custNameLabel.Location = new Point(65, custName.Location.Y);
                    custNameLabel.Font = new Font("Microsoft Sans Serif", 11, FontStyle.Italic);
                    custNameLabel.ForeColor = Color.Gray;
                    custNameLabel.BringToFront();
                    custNameLabel.Cursor = Cursors.IBeam;
                }
                else
                {
                    custNameLabel.Location = new Point(64, 50);
                    custNameLabel.Font = new Font("Microsoft Sans Serif", 10, FontStyle.Regular);
                    custNameLabel.ForeColor = Color.Gray;

                    custName.Text = ToNameText(custName.Text);
                }


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void custName_Enter(object sender, EventArgs e)
        {
            try
            {
                custNameLabel.Location = new Point(64, 50);
                custNameLabel.Font = new Font("Microsoft Sans Serif", 10, FontStyle.Regular);
                custNameLabel.ForeColor = Color.Gray;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

            }
        }

        private void custNameLabel_Click(object sender, EventArgs e)
        {
            try
            {
                custNameLabel.Location = new Point(64, 50);
                custNameLabel.Font = new Font("Microsoft Sans Serif", 10, FontStyle.Regular);
                custNameLabel.ForeColor = Color.Gray;
                custName.Focus();
                custNameLabel.Cursor = Cursors.Default;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void custContactLabel_Click(object sender, EventArgs e)
        {
            try
            {
                custContactLabel.Location = new Point(367, 52);
                custContactLabel.Font = new Font("Microsoft Sans Serif", 10, FontStyle.Regular);
                custContactLabel.ForeColor = Color.Gray;
                custContact.Focus();
                custContactLabel.Cursor = Cursors.Default;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void custContact_Enter(object sender, EventArgs e)
        {
            try
            {
                custContactLabel.Location = new Point(367, 52);
                custContactLabel.Font = new Font("Microsoft Sans Serif", 10, FontStyle.Regular);
                custContactLabel.ForeColor = Color.Gray;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void custContact_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                if (custContact.Text == String.Empty)
                {
                    custContactLabel.Location = new Point(359, custContact.Location.Y);
                    custContactLabel.Font = new Font("Microsoft Sans Serif", 11, FontStyle.Italic);
                    custContactLabel.ForeColor = Color.Gray;
                    custContactLabel.BringToFront();
                    custContactLabel.Cursor = Cursors.IBeam;
                }
                else
                {
                    custContactLabel.Location = new Point(367, 52);
                    custContactLabel.Font = new Font("Microsoft Sans Serif", 10, FontStyle.Regular);
                    custContactLabel.ForeColor = Color.Gray;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void custAddressLabel_Click(object sender, EventArgs e)
        {
            try
            {
                custAddressLabel.Location = new Point(64, 100);
                custAddressLabel.Font = new Font("Microsoft Sans Serif", 10, FontStyle.Regular);
                custAddressLabel.ForeColor = Color.Gray;
                custAddress.Focus();
                custAddressLabel.Cursor = Cursors.Default;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void custAddress_Enter(object sender, EventArgs e)
        {
            try
            {
                custAddressLabel.Location = new Point(64, 100);
                custAddressLabel.Font = new Font("Microsoft Sans Serif", 10, FontStyle.Regular);
                custAddressLabel.ForeColor = Color.Gray;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

            }
        }

        private void custAddress_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                if (custAddress.Text == String.Empty)
                {
                    custAddressLabel.Location = new Point(65, custAddress.Location.Y);
                    custAddressLabel.Font = new Font("Microsoft Sans Serif", 11, FontStyle.Italic);
                    custAddressLabel.ForeColor = Color.Gray;
                    custAddressLabel.BringToFront();
                    custAddressLabel.Cursor = Cursors.IBeam;
                }
                else
                {
                    custAddressLabel.Location = new Point(64, 100);
                    custAddressLabel.Font = new Font("Microsoft Sans Serif", 10, FontStyle.Regular);
                    custAddressLabel.ForeColor = Color.Gray;
                     custAddress.Text = ToNameText(custAddress.Text);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void custContact_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (custContact.TextLength >= 0 && (e.KeyChar == (char)Keys.Oemplus || e.KeyChar == (char)Keys.Oemcomma || e.KeyChar == (char)Keys.Space))
            {
                //tests 
            }
            else
            {
                if (!char.IsControl(e.KeyChar)
                    && !char.IsDigit(e.KeyChar)
                    && e.KeyChar != '+' && e.KeyChar != ',' && e.KeyChar != ' ')
                {
                    e.Handled = true;
                }
                // only allow one decimal point
                //if (e.KeyChar == '.' && (sender as TextBox).Text.IndexOf('.') > -1)
                //{
                //    e.Handled = true;
                //}
                //
                //if (e.KeyChar == ',' && (sender as TextBox).Text.IndexOf(',') > -1)
                //{
                //    e.Handled = true;
                //}
            }
        }

        private void AddButton_Click(object sender, EventArgs e)
        {
            try
            {
                if(partName.Text == "" || partRate.Text == "" || partQuantity.Text == "0" || partQuantity.Text == "")
                {
                    MessageBox.Show("Cannot add to list since some required field is not filled out!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                ListViewItem lvItem = new ListViewItem();
                lvItem.SubItems[0].Text = (listView1.Items.Count + 1).ToString();
                lvItem.SubItems[0].Font = new Font(new Font("Consolas", 10f), FontStyle.Regular);
                lvItem.SubItems.Add(partName.Text);
                lvItem.SubItems[1].Font = new Font(new Font("Consolas", 10f), FontStyle.Regular);
                lvItem.SubItems.Add(partQuantity.Text);
                lvItem.SubItems[2].Font = new Font(new Font("Consolas", 10f), FontStyle.Regular);
                lvItem.SubItems.Add(partRate.Text);
                lvItem.SubItems[3].Font = new Font(new Font("Consolas", 10f), FontStyle.Regular);
                lvItem.SubItems.Add(partTax.Text);
                lvItem.SubItems[4].Font = new Font(new Font("Consolas", 10f), FontStyle.Regular);
                lvItem.SubItems.Add(partTaxAmount.Text);
                lvItem.SubItems[5].Font = new Font(new Font("Consolas", 10f), FontStyle.Regular);
                lvItem.SubItems.Add(partTotal.Text);
                lvItem.SubItems[6].Font = new Font(new Font("Consolas", 10f), FontStyle.Regular);
                listView1.Items.Add(lvItem);

                itemlist.AppendLine(String.Format("INSERT INTO invoicetb(`invoiceid`, `partname`, `partquantity`, `partrate`, `parttax`, `parttaxtype` , `parttaxamount`, `partnetprice`) VALUES('%invoiceid%', '{0}', {1}, {2}, {3}, '{4}', {5}, {6});", partName.Text, partQuantity.Text, partRate.Text, partTax.Text, label6.Text.Remove(0,1), partTaxAmount.Text, partTotal.Text));
                taxtypelist.Add(label6.Text.Substring(1));

                decimal amount = 0;

                for(var item = 0; item <= listView1.Items.Count - 1; item++)
                {
                    amount += Convert.ToDecimal(listView1.Items[item].SubItems[6].Text);
                }

                subTotalBox.Text = amount.ToString("F02"); ;

                partName.Focus();

                partName.SelectedIndex = -1;
                partRate.ResetText();
                partQuantity.ResetText();
                partTax.ResetText();
                partTaxAmount.ResetText();
                partTotal.ResetText();                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message +ex.ToString());
            }
        }

        private void partQuantity_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                if ((partRate.Text != "" && partQuantity.Text != "") && partQuantity.SelectedIndex != -1)
                {
                    partTotal.Text = Convert.ToString((decimal)(Convert.ToDecimal(partRate.Text) * Convert.ToDecimal(partQuantity.Text)));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void discountBoxLabel_Click(object sender, EventArgs e)
        {
            try
            {
                discountBoxLabel.Location = new Point(115, 88);
                discountBoxLabel.Font = new Font("Microsoft Sans Serif", 11, FontStyle.Regular);
                discountBoxLabel.ForeColor = Color.Gray;
                discountBox.Focus();
                discountBoxLabel.Cursor = Cursors.Default;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void discountBox_Enter(object sender, EventArgs e)
        {
            try
            {
                discountBoxLabel.Location = new Point(115, 88);
                discountBoxLabel.Font = new Font("Microsoft Sans Serif", 11, FontStyle.Regular);
                discountBoxLabel.ForeColor = Color.Gray;                
                discountBox.Focus();
                discountBoxLabel.Cursor = Cursors.Default;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void discountBox_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                if (discountBox.Text != String.Empty)
                {
                    discountBoxLabel.Location = new Point(115, 88);
                    discountBoxLabel.Font = new Font("Microsoft Sans Serif", 11, FontStyle.Regular);
                    discountBoxLabel.ForeColor = Color.DarkGray;
                    discountBoxLabel.Cursor = Cursors.Default;

                    if(Convert.ToDecimal(subTotalBox.Text) >= Convert.ToDecimal(discountBox.Text))
                    {
                        totalBox.Text = string.Format("{0}", Convert.ToDecimal(subTotalBox.Text) - Convert.ToDecimal(discountBox.Text));
                    }
                    else
                    {
                        totalBox.Text = string.Format("-{0}", Convert.ToDecimal(discountBox.Text) - Convert.ToDecimal(subTotalBox.Text));
                    }
                    discountBox.Text = (Convert.ToDecimal(discountBox.Text)).ToString("F02");
                }
                else
                {
                    discountBoxLabel.Location = new Point(108, 108);
                    discountBoxLabel.Font = new Font("Microsoft Sans Serif", 11, FontStyle.Italic);
                    discountBoxLabel.ForeColor = Color.Gray;
                    discountBoxLabel.Cursor = Cursors.Default;
                    discountBoxLabel.Cursor = Cursors.IBeam;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void paidBoxLabel_Click(object sender, EventArgs e)
        {
            try
            {
                paidBoxLabel.Location = new Point(230, 184);
                paidBoxLabel.Font = new Font("Microsoft Sans Serif", 11, FontStyle.Regular);
                paidBoxLabel.ForeColor = Color.Gray;
                paidBoxLabel.Cursor = Cursors.Default;
                paidBox.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void paidBox_Enter(object sender, EventArgs e)
        {
            try
            {
                paidBoxLabel.Location = new Point(230, 184);
                paidBoxLabel.Font = new Font("Microsoft Sans Serif", 11, FontStyle.Regular);
                paidBoxLabel.ForeColor = Color.Gray;
                paidBoxLabel.Cursor = Cursors.Default;                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void paidBox_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                if (paidBox.Text != String.Empty)
                {
                    paidBoxLabel.Location = new Point(230, 184);
                    paidBoxLabel.Font = new Font("Microsoft Sans Serif", 11, FontStyle.Regular);
                    paidBoxLabel.ForeColor = Color.Gray;
                    paidBoxLabel.Cursor = Cursors.IBeam;

                    if(Convert.ToDecimal(paidBox.Text) >= Convert.ToDecimal(totalBox.Text))
                    {
                        returnBox.Text = Convert.ToString(Convert.ToDecimal(paidBox.Text) - Convert.ToDecimal(totalBox.Text));
                    }
                    else
                    {
                        returnBox.Text = string.Format("-{0}", Convert.ToDecimal(totalBox.Text) - Convert.ToDecimal(paidBox.Text));
                    }

                    paidBox.Text = (Convert.ToDecimal(paidBox.Text)).ToString("F02");
                }
                else
                {                    
                    paidBoxLabel.Location = new Point(225, 204);
                    paidBoxLabel.Font = new Font("Microsoft Sans Serif", 11, FontStyle.Italic);
                    paidBoxLabel.ForeColor = Color.Gray;
                    paidBoxLabel.Cursor = Cursors.Default;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void returnBoxLabel_Click(object sender, EventArgs e)
        {
            try
            {
                returnBoxLabel.Location = new Point(215, 232);
                returnBoxLabel.Font = new Font("Microsoft Sans Serif", 11, FontStyle.Regular);
                returnBoxLabel.ForeColor = Color.Gray;
                returnBoxLabel.Cursor = Cursors.Default;
                returnBox.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void returnBox_Enter(object sender, EventArgs e)
        {
            try
            {
                returnBoxLabel.Location = new Point(215, 232);
                returnBoxLabel.Font = new Font("Microsoft Sans Serif", 11, FontStyle.Regular);
                returnBoxLabel.ForeColor = Color.Gray;
                returnBoxLabel.Cursor = Cursors.Default;
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void returnBox_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                if (returnBox.Text != String.Empty)
                {
                    returnBoxLabel.Location = new Point(215, 232);
                    returnBoxLabel.Font = new Font("Microsoft Sans Serif", 11, FontStyle.Regular);
                    returnBoxLabel.ForeColor = Color.Gray;
                    returnBoxLabel.Cursor = Cursors.Default;                    
                }
                else
                {
                    returnBoxLabel.Location = new Point(210, 252);
                    returnBoxLabel.Font = new Font("Microsoft Sans Serif", 11, FontStyle.Italic);
                    returnBoxLabel.ForeColor = Color.Gray;
                    returnBoxLabel.Cursor = Cursors.IBeam;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void subTotalLabel_Click(object sender, EventArgs e)
        {
            try
            {
                subTotalLabel.Location = new Point(197, 41);
                subTotalLabel.Font = new Font("Microsoft Sans Serif", 11, FontStyle.Regular);
                subTotalLabel.ForeColor = Color.Gray;
                subTotalLabel.Cursor = Cursors.Default;
                subTotalBox.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void subTotalBox_Enter(object sender, EventArgs e)
        {
            try
            {
                subTotalLabel.Location = new Point(197, 41);
                subTotalLabel.Font = new Font("Microsoft Sans Serif", 11, FontStyle.Regular);
                subTotalLabel.ForeColor = Color.Gray;
                subTotalLabel.Cursor = Cursors.Default;
                subTotalBox.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void subTotalBox_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                if (subTotalBox.Text != String.Empty)
                {
                    subTotalLabel.Location = new Point(197, 41);
                    subTotalLabel.Font = new Font("Microsoft Sans Serif", 11, FontStyle.Regular);
                    subTotalLabel.ForeColor = Color.Gray;
                    subTotalLabel.Cursor = Cursors.Default;
                }
                else
                {
                    subTotalLabel.Location = new Point(191, 61);
                    subTotalLabel.Font = new Font("Microsoft Sans Serif", 11, FontStyle.Italic);
                    subTotalLabel.ForeColor = Color.Gray;
                    subTotalLabel.Cursor = Cursors.IBeam;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void totalBoxLabel_Click(object sender, EventArgs e)
        {
            try
            {
                totalBoxLabel.Location = new Point(226, 136);
                totalBoxLabel.Font = new Font("Microsoft Sans Serif", 11, FontStyle.Regular);
                totalBoxLabel.ForeColor = Color.Gray;
                totalBoxLabel.Cursor = Cursors.Default;
                totalBox.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void totalBox_Enter(object sender, EventArgs e)
        {
            try
            {
                totalBoxLabel.Location = new Point(226, 136);
                totalBoxLabel.Font = new Font("Microsoft Sans Serif", 11, FontStyle.Regular);
                totalBoxLabel.ForeColor = Color.Gray;
                totalBoxLabel.Cursor = Cursors.Default;
                totalBox.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void totalBox_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                if (totalBox.Text != String.Empty)
                {
                    totalBoxLabel.Location = new Point(226, 136);
                    totalBoxLabel.Font = new Font("Microsoft Sans Serif", 11, FontStyle.Regular);
                    totalBoxLabel.ForeColor = Color.Gray;
                    totalBoxLabel.Cursor = Cursors.Default;
                }
                else
                {
                    totalBoxLabel.Location = new Point(221, 156);
                    totalBoxLabel.Font = new Font("Microsoft Sans Serif", 11, FontStyle.Italic);
                    totalBoxLabel.ForeColor = Color.Gray;                    
                    totalBoxLabel.Cursor = Cursors.IBeam;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void paidBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (custContact.TextLength >= 0)
            {
                //tests 
            }
            else
            {
                if (!char.IsControl(e.KeyChar)
                    && !char.IsDigit(e.KeyChar)
                    )
                {
                    e.Handled = true;
                }                
            }

            // only allow one decimal point
            if (e.KeyChar == '.' && (sender as TextBox).Text.IndexOf('.') > 0)
            {
                e.Handled = true;
            }           
           
        }

        private void paidBox_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void subTotalBox_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if(subTotalBox.TextLength > 0)
                {
                    subTotalLabel.Location = new Point(197, 41);
                    subTotalLabel.Font = new Font("Microsoft Sans Serif", 11, FontStyle.Regular);
                    subTotalLabel.ForeColor = Color.Gray;
                    subTotalLabel.Cursor = Cursors.Default;

                    if(discountBox.TextLength == 0) { totalBox.Text = subTotalBox.Text; }
                    else { totalBox.Text = string.Format("{0}", Convert.ToDecimal(subTotalBox.Text) - Convert.ToDecimal(discountBox.Text)); }
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void totalBox_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (totalBox.TextLength > 0)
                {
                    totalBoxLabel.Location = new Point(226, 136);
                    totalBoxLabel.Font = new Font("Microsoft Sans Serif", 11, FontStyle.Regular);
                    totalBoxLabel.ForeColor = Color.Gray;
                    totalBoxLabel.Cursor = Cursors.Default;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void deleteItem_Click(object sender, EventArgs e)
        {
            try
            {
                if(listView1.SelectedItems.Count > 0)
                {    
                    listView1.Items[listView1.SelectedIndices[0]].Remove();
                    taxtypelist[listView1.SelectedIndices[0]].Remove(0);
                }

                decimal amount = 0;
                for (var item = 0; item <= listView1.Items.Count - 1; item++)
                {
                    amount += Convert.ToDecimal(listView1.Items[item].SubItems[6].Text);
                }

                itemlist.Clear();

                for (var item = 0; item <= listView1.Items.Count - 1; item++)
                {
                    string g = "";
                    reader.Dispose();
                    if(connection.State == ConnectionState.Closed) { connection.Open(); }
                    command = new MySqlCommand(String.Format("SELECT parttaxtype FROM particulartb WHERE partname = '{0}'", listView1.Items[item].SubItems[1].Text), connection);
                    reader = command.ExecuteReader();
                    while (reader.Read()) { g = reader[0].ToString(); }
                    reader.Dispose();
                    itemlist.AppendLine(String.Format("INSERT INTO invoicetb(`invoiceid`, `partname`, `partquantity`, `partrate`, `parttax`, `parttaxtype` , `parttaxamount`, `partnetprice`) VALUES('%invoiceid%', '{0}', {1}, {2}, {3}, '{4}', {5}, {6});", listView1.Items[item].SubItems[1].Text, listView1.Items[item].SubItems[2].Text, listView1.Items[item].SubItems[3].Text, listView1.Items[item].SubItems[4].Text, g, listView1.Items[item].SubItems[5].Text, listView1.Items[item].SubItems[6].Text));
                }
                reader.Dispose();

                subTotalBox.Text = String.Format("{0}", amount);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void copyItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (listView1.SelectedItems.Count > 0)
                {
                    Clipboard.SetText(String.Format("{0}\t{1}\t{2}\t{3}\t{4}\t{5}", listView1.Items[listView1.SelectedIndices[0]].SubItems[0].Text, listView1.Items[listView1.SelectedIndices[0]].SubItems[1].Text, listView1.Items[listView1.SelectedIndices[0]].SubItems[2].Text, listView1.Items[listView1.SelectedIndices[0]].SubItems[3].Text, listView1.Items[listView1.SelectedIndices[0]].SubItems[4].Text, listView1.Items[listView1.SelectedIndices[0]].SubItems[5].Text));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void clearButton_Click(object sender, EventArgs e)
        {
            try
            {
                

                //custName.Focus();
                //custName.ResetText();
                custContact.Focus();
                custContact.ResetText();
                custAddress.Focus();
                custAddress.ResetText();

                partName.SelectedIndex = -1;
                partRate.ResetText();
                partQuantity.ResetText();
                label6.Text = "Tax %";
                partTax.ResetText();
                partTaxAmount.ResetText();
                partTotal.ResetText();

                subTotalBox.Focus();
                subTotalBox.ResetText();
                discountBox.Focus();
                discountBox.ResetText();
                totalBox.Focus();
                totalBox.ResetText();
                paidBox.Focus();
                paidBox.ResetText();               
                returnBox.ResetText();
                returnBox.Focus();

                statusBox.SelectedIndex = -1;                
                todayDate.ResetText();

                for (var item = 0; item <= listView1.Items.Count - 1; item++)
                {
                    listView1.Items[item].SubItems.Clear();
                }

                printCheckBox.CheckState = CheckState.Unchecked;

                //clearButton.Focus();
                custName.ResetText();
                custName.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void partRate_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                if(partRate.Text == "") { return; }

                if (partRate.Text != "" && (partQuantity.Text != "" || partQuantity.SelectedIndex != -1))
                {
                    if(partQuantity.Text == "0") { partTotal.Text = "0.00"; return; }

                    partTotal.Text = ((decimal)(Convert.ToDecimal(partRate.Text) * Convert.ToDecimal(partQuantity.Text)) + (((decimal)(Convert.ToDecimal(partRate.Text) * Convert.ToDecimal(partQuantity.Text)) * Convert.ToDecimal(partQuantity.Text)) * (Convert.ToDecimal(partTax.Text) / 100))).ToString("F02");

                    partRate.Text = (Convert.ToDecimal(partRate.Text)).ToString("F02");
                    
                    decimal d = Convert.ToDecimal(Convert.ToDecimal(partQuantity.Text) * (Convert.ToDecimal(partRate.Text)) + Convert.ToDecimal(partTaxAmount.Text));
                    partTotal.Text = d.ToString("F02");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void partTax_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                partTaxAmount.Text = (Convert.ToDecimal(partRate.Text) * (Convert.ToDecimal(partTax.Text) / 100)).ToString("F02");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void partTax_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if(partRate.Text == "" || partTax.Text == "") { return; }
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void partTaxAmount_TextChanged(object sender, EventArgs e)
        {
            try
            {       
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void partTax_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                if(partRate.Text == "" || partTax.Text == "") { return; }
                partTaxAmount.Text = (Convert.ToDecimal(partRate.Text) * (Convert.ToDecimal(partTax.Text) / 100)).ToString("F02");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            try
            {
                if(listView1.Items.Count < 1)
                {
                    MessageBox.Show("Nothing to Save. You may add the particular in the purchase list first.", "Woops", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                if(paidBox.Text == "") { MessageBox.Show("Please enter the paid amount and click save again!", "Something's missing!", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
                if(returnBox.Text == "") { MessageBox.Show("Please enter the paid amount and click save again!", "Something's missing!", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
                if(discountBox.Text == "") { discountBox.Text = "0.00"; }
                if(totalBox.Text == "" || totalBox.Text == "0") { MessageBox.Show("Please select some item into the list!", "Something's missing!", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }

                //status paid or partial
                if (statusBox.SelectedIndex == -1 || statusBox.Text == "") { MessageBox.Show("Please select the payment status and click on Save again.", "Payment State Missing!", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
                
                if (custName.Text == "")
                {
                    if(MessageBox.Show("Customer name is not filled out, do you want to enter it first?", "Customer Name", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes) { return; }
                }
                if (custContact.Text == "")
                {
                    if (MessageBox.Show("Customer Contact Number is not filled out, do you want to enter it first?", "Customer Contact", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes) { return; }
                }
                
                //Save it and Print it if necessary
                if (connection.State == ConnectionState.Closed) { connection.Open(); }
                    using (MySqlTransaction trans = connection.BeginTransaction())
                    {
                        try
                        {
                            long i = 0;                          

                            if (i == 0)
                            {

                                string custname = "Guest";
                                string custcontact = "0000000000";
                                string custaddress = "Not Mentioned";

                                if (custName.Text != "")
                                { custname = custName.Text; }
                                if(custContact.Text != "")
                                { custcontact = custContact.Text; }
                                if(custAddress.Text != "") { custaddress = custAddress.Text; }
                                    
                                command = new MySqlCommand(String.Format("INSERT INTO customertb(`prefix`, `invoicedate`, `customername`, `customercontact`, `customeraddress`) VALUES('{0}', '{1}', '{2}', '{3}', '{4}');", Properties.Settings.Default.invoiceprefix, String.Format("{0:yyyy-MM-dd}", Convert.ToDateTime(todayDate.Text)), custname, custcontact, custaddress), connection, trans);
                                i += command.ExecuteNonQuery();                            

                                invoiceid = String.Format("{0}{1}", Properties.Settings.Default.invoiceprefix, command.LastInsertedId);                                

                                if (i == 0)
                                {
                                    MessageBox.Show("Nothing to be added!", "Not Done!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    trans.Rollback();
                                }
                                else if (i >= 1)
                                {
                                    itemlist.Replace("%invoiceid%", invoiceid);
                                    command = new MySqlCommand(itemlist.ToString(), connection, trans);
                                
                                    i += command.ExecuteNonQuery();

                                    command = new MySqlCommand(String.Format("INSERT INTO invoicerec(`invoiceid`, `invoicedate`, `subtotal`, `discount`, `nettotal`, `paidamount` , `returnamount`, `status`) VALUES('{0}', '{1}', {2}, {3}, {4}, {5}, {6}, '{7}');", invoiceid, string.Format("{0:yyyy-MM-dd}", Convert.ToDateTime(todayDate.Text)), subTotalBox.Text, discountBox.Text, totalBox.Text, paidBox.Text, returnBox.Text, statusBox.Text), connection, trans);
                                    i += command.ExecuteNonQuery();

                                    if(i >= 1)
                                    {
                                        MessageBox.Show("Records Added Successfully!", "Added!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                        trans.Commit();

                                    if (printCheckBox.Checked == true)
                                    {
                                        PrintReceipt();
                                    }

                                    clearButton.PerformClick();                                        
                                    }
                                    else { trans.Rollback(); }
                                    
                                    itemlist.Clear();                                    
                                }                          

                            }                            
                            
                        }
                        catch (MySqlException ex) { MessageBox.Show(ex.Message); trans.Rollback(); }
                    }
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally { connection.Close(); }
        }

        private void addPartButton_Click(object sender, EventArgs e)
        {
            try
            {
                tabControl1.SelectTab(5);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void addParticularButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (part.Text == "" || rate.Text == "" || rate.Text == "0" || tax.Text == "")
                {
                    MessageBox.Show("Cannot add to list since some required field is not filled out!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                ListViewItem lvItem = new ListViewItem();
                lvItem.SubItems[0].Text = (listView1.Items.Count + 1).ToString();
                lvItem.SubItems[0].Font = new Font(new Font("Consolas", 10f), FontStyle.Regular);
                lvItem.SubItems.Add(part.Text);
                lvItem.SubItems[1].Font = new Font(new Font("Consolas", 10f), FontStyle.Regular);
                lvItem.SubItems.Add(rate.Text);
                lvItem.SubItems[2].Font = new Font(new Font("Consolas", 10f), FontStyle.Regular);
                lvItem.SubItems.Add((tax.Value / 100).ToString("P02"));
                lvItem.SubItems[3].Font = new Font(new Font("Consolas", 10f), FontStyle.Regular);
                lvItem.SubItems.Add(taxtype.Text);
                lvItem.SubItems[4].Font = new Font(new Font("Consolas", 10f), FontStyle.Regular);
                lvItem.SubItems.Add(taxamount.Text);
                lvItem.SubItems[5].Font = new Font(new Font("Consolas", 10f), FontStyle.Regular);
                lvItem.SubItems.Add(totalamount.Text);
                lvItem.SubItems[6].Font = new Font(new Font("Consolas", 10f), FontStyle.Regular);
                listView2.Items.Add(lvItem);
                
                part.SelectedIndex = -1;
                rate.ResetText();
                tax.ResetText();
                taxtype.SelectedIndex = -1;
                taxamount.ResetText();
                totalamount.ResetText();

                part.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void part_Validating(object sender, CancelEventArgs e)
        {
            try { part.Text = ToNameText(part.Text); }
            catch(Exception ex) { MessageBox.Show(ex.Message); }
            
        }

        private void rate_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                decimal taxamt = 0;
                if(rate.Value != 0 && tax.Value != 0)
                {
                    taxamt = (decimal)rate.Value * (decimal)(tax.Value / 100);
                    taxamount.Text = taxamt.ToString("F02");
                    totalamount.Text = ((decimal)(Convert.ToDecimal(rate.Value) + taxamt)).ToString("F02");
                }
                else if(tax.Value == 0)
                {
                    taxamount.Text = "0.00";
                    totalamount.Text = ((decimal)rate.Value).ToString("F02");
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void listview2del_Click(object sender, EventArgs e)
        {
            try
            {
                if (listView2.SelectedItems.Count > 0)
                {
                    listView2.Items[listView2.SelectedIndices[0]].Remove();
                }                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void listview2cp_Click(object sender, EventArgs e)
        {
            try
            {
                if (listView2.SelectedItems.Count > 0)
                {
                    Clipboard.SetText(String.Format("{0}\t{1}\t{2}\t{3}\t{4}\t{5}", listView2.Items[listView2.SelectedIndices[0]].SubItems[0].Text, listView2.Items[listView2.SelectedIndices[0]].SubItems[1].Text, listView2.Items[listView2.SelectedIndices[0]].SubItems[2].Text, listView2.Items[listView2.SelectedIndices[0]].SubItems[3].Text, listView2.Items[listView2.SelectedIndices[0]].SubItems[4].Text, listView2.Items[listView2.SelectedIndices[0]].SubItems[5].Text));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void partSaveButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (connection.State == ConnectionState.Closed) { connection.Open(); }
                using (MySqlTransaction trans = connection.BeginTransaction())
                {
                    try
                    {
                        long i = 0;
                        if (part.Text != "" && totalamount.Text != "" && taxamount.Text != "")
                        {
                            command = new MySqlCommand(String.Format("UPDATE particulartb SET `partrate` = {1}, `parttax` = {2}, `parttaxtype` = '{3}', `taxamount` = {4}, `totalamount` = {5} WHERE `partname` = '{0}'", part.Text, rate.Value, (decimal)(tax.Value / 100), taxtype.Text, taxamount.Text, totalamount.Text), connection, trans);
                            i = command.ExecuteNonQuery();
                        }                        

                        if(i == 0)
                        {
                            for (var item = 0; item <= listView2.Items.Count - 1; item++)
                            {
                                command = new MySqlCommand(String.Format("INSERT INTO particulartb(`partname`, `partrate`, `parttax`, `parttaxtype`, `taxamount`, `totalamount`) VALUES('{0}', {1}, {2}, '{3}', {4}, {5});", Convert.ToString(listView2.Items[item].SubItems[1].Text), Convert.ToDecimal(listView2.Items[item].SubItems[2].Text), Convert.ToDecimal(listView2.Items[item].SubItems[3].Text.Remove(listView2.Items[item].SubItems[3].Text.Length - 1, 1)) / 100, Convert.ToString(listView2.Items[item].SubItems[4].Text), Convert.ToDecimal(listView2.Items[item].SubItems[5].Text), Convert.ToDecimal(listView2.Items[item].SubItems[6].Text)), connection, trans);
                                i += command.ExecuteNonQuery();
                            }

                            if(i == 0)
                            {
                                MessageBox.Show("Nothing to be added!", "Not Done!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                trans.Rollback();
                            }
                            else if(i >= 1)
                            {
                                MessageBox.Show("Records Added Successfully!", "Added!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                trans.Commit();

                                                                

                                part.SelectedIndex = -1;
                                rate.ResetText();
                                tax.ResetText();
                                taxtype.SelectedIndex = -1;
                                taxamount.ResetText();
                                totalamount.ResetText();

                                part.Focus();
                            }                            
                        }
                        else
                        {
                            MessageBox.Show("Record Updated Successfully!", "Updated!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            trans.Commit();

                        }
                        loadlistparticulars();
                    }
                    catch(MySqlException ex) { MessageBox.Show(ex.Message);  trans.Rollback(); }
                }              
                                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally { connection.Close(); }
        }

        private void listView3_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if(listView3.SelectedItems.Count > 0)
                {
                    part.Text = Convert.ToString(listView3.Items[listView3.SelectedIndices[0]].SubItems[1].Text);
                    rate.Value = Convert.ToDecimal(listView3.Items[listView3.SelectedIndices[0]].SubItems[2].Text);
                    tax.Value = Convert.ToDecimal(listView3.Items[listView3.SelectedIndices[0]].SubItems[3].Text.Remove(listView3.Items[listView3.SelectedIndices[0]].SubItems[3].Text.Length -1, 1));
                    taxtype.Text = Convert.ToString(listView3.Items[listView3.SelectedIndices[0]].SubItems[4].Text);
                    taxamount.Text = Convert.ToString(listView3.Items[listView3.SelectedIndices[0]].SubItems[5].Text);
                    totalamount.Text = Convert.ToString(listView3.Items[listView3.SelectedIndices[0]].SubItems[6].Text);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void loadlistparticulars()
        {
            try
            {
                //Clear the Item
                listView3.Clear();

                listView3.Columns.Add("ID", 40, HorizontalAlignment.Left);
                listView3.Columns.Add("Particular Name", 200, HorizontalAlignment.Left);
                listView3.Columns.Add("Price (₹)", 70, HorizontalAlignment.Center);
                listView3.Columns.Add("Tax %", 90, HorizontalAlignment.Center);
                listView3.Columns.Add("Tax Type", 90, HorizontalAlignment.Center);
                listView3.Columns.Add("Tax Amount", 120, HorizontalAlignment.Center);
                listView3.Columns.Add("Net Total", 120, HorizontalAlignment.Center);
                listView3.Font = new Font("Times New Roman", 10, FontStyle.Regular);

                if (connection.State == ConnectionState.Closed) { connection.Open(); }

                reader.Dispose();
                command = new MySqlCommand("SELECT * FROM particulartb", connection);
                reader = command.ExecuteReader();

                

                while (reader.Read())
                {
                    ListViewItem lvItem = new ListViewItem();
                    lvItem.SubItems[0].Text = reader[0].ToString();
                    lvItem.SubItems[0].Font = new Font(new Font("Consolas", 10f), FontStyle.Regular);
                    lvItem.SubItems.Add(reader[1].ToString());
                    lvItem.SubItems[1].Font = new Font(new Font("Consolas", 10f), FontStyle.Regular);
                    lvItem.SubItems.Add(reader[2].ToString());
                    lvItem.SubItems[2].Font = new Font(new Font("Consolas", 10f), FontStyle.Regular);
                    lvItem.SubItems.Add(Convert.ToDecimal(reader[3]).ToString("P02"));
                    lvItem.SubItems[3].Font = new Font(new Font("Consolas", 10f), FontStyle.Regular);
                    lvItem.SubItems.Add(reader[4].ToString());
                    lvItem.SubItems[4].Font = new Font(new Font("Consolas", 10f), FontStyle.Regular);
                    lvItem.SubItems.Add(reader[5].ToString());
                    lvItem.SubItems[5].Font = new Font(new Font("Consolas", 10f), FontStyle.Regular);
                    lvItem.SubItems.Add(reader[6].ToString());
                    lvItem.SubItems[6].Font = new Font(new Font("Consolas", 10f), FontStyle.Regular);
                    listView3.Items.Add(lvItem);
                }

                reader.Dispose();                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void partClearButton_Click(object sender, EventArgs e)
        {
            part.SelectedIndex = -1;
            rate.ResetText();
            tax.ResetText();
            taxtype.SelectedIndex = -1;
            taxamount.ResetText();
            totalamount.ResetText();
            part.Focus();
        }

        private void partName_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if(connection.State == ConnectionState.Closed) { connection.Open(); }
                if (partName.Text != "")
                {
                    reader.Dispose();
                    command = new MySqlCommand(string.Format("SELECT partrate, parttax, parttaxtype, taxamount, totalamount FROM particulartb WHERE partname = '{0}'", partName.Text), connection);
                    reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        partRate.Text = ((decimal)reader[0]).ToString("F02");
                        partTax.Text = ((decimal)reader[1] * 100).ToString("F02");
                        label6.Text = "%" + reader[2].ToString();
                        partTaxAmount.Text = ((decimal)reader[3]).ToString("F02");
                        partTotal.Text = ((decimal)reader[4]).ToString("F02");
                    }

                    reader.Dispose();

                    partQuantity.SelectedIndex = 1;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally { connection.Close(); }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if(comboBox1.SelectedIndex == 1)
                {
                    textBox1.Text = "HE SOFTWARE HI \"A NIH ANG ANG\" A PHO CHHUAH A NI A, WARRANTY ANG CHI RENG RENG A AWM LOVA. DAWRAH EMAW I LO HMANG A NIH PAWH IN RUK CHHAWN PHAL A NI LO BAWK A, LO COPY CHHUAK AN AWM PAWH IN CHUNG THLENG CHUAN A HUAM ZEL BAWK ANG. HE SOFTWARE AVANG A CHHIATNA ANG CHI HRIM HRIM AH A SIAMTU EMAW, PECHHUAKTU EMAW HIAN MAWH ENGMAH A PHUR LO ANG. \nHE SOFTWARE I HMAN CHIAH HIAN A CHUNG A MI KHI I PAWM A NI TIHNA A NI ANG.\nHE SOFTWARE SIAM HIAN KA THAWKRIM HLE A, ENGMAH HLAWH PAWH NEI LOVA KA SIAM LIAU LIAU A NI A, I LO HMANG THANGKAI A, THA I TIH VE CHUAN DONATION LAM TE PAWH MIN PE VE TUR AH KA NGAI E.DONATION I PEK HNU HIAN SOFTWARE FULL FEATURE I HMAN THEIH NAN THIL KA TIH SAK VE DAWN CHE A NI.";
                }
                else
                {
                    textBox1.Text = "THE SOFTWARE IS PROVIDED \"AS IS\", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.\nBY USING THIS SOFTWARE YOU AGREED THE ABOVE MENTIONED INFORMATION. \nI WORKED REALLY HARD FOR THIS SOFTWARE, WITHOUT BEING PAID.IF YOU FIND IT USEFUL AND FRUITFUL, PLEASE REMEMBER THAT I ACCEPT A DONATION.YOU CAN GET BETTER BENEFITS OF THIS SOFTWARE BY UNLOCKING FULL FEATURES AFTER YOU DONATE.";
                }
            }
            catch(Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void aboutButton_Click(object sender, EventArgs e)
        {
            try
            {
                tabControl1.SelectTab(4);
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void backButton_Click(object sender, EventArgs e)
        {
            tabControl1.SelectTab(3);
        }

        private void subTotalBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

       

        private void partRate_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if(partQuantity.Text == "" && partQuantity.SelectedIndex == -1 || partRate.Text == "") { return; }
                else
                {
                    //partRate.Text = (Convert.ToDecimal(partRate.Text)).ToString("F02");
                }
                
            }
            catch (Exception ex) { MessageBox.Show(ex.Message ); }
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
            
            if(printDialog.ShowDialog() == DialogResult.OK)
            {
                printDoc.Print();
            }

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

        private void PrintDoc_PrintPage(object sender, PrintPageEventArgs e)
        {
            Graphics g = e.Graphics;
            Font font = new Font("Times New Roman", 16f, FontStyle.Bold);
            float fontHeight = font.GetHeight();
            int startx = 30;
            int starty = 20;
            int offset = 5;

            if(Properties.Settings.Default.shopLogo == "")
            {
                //Shop Name
                g.DrawString(Properties.Settings.Default.shopName, font, new SolidBrush(Color.Black), startx, starty);
                font = new Font("Times New Roman", 11f, FontStyle.Regular);

                //Shop Address
                offset += font.Height + 5;
                g.DrawString(String.Format("{0}", Properties.Settings.Default.shopAddress), font, new SolidBrush(Color.Black), startx, starty + offset);


                //Receipt Date
                offset += font.Height + 5;
                g.DrawString(String.Format("Date:\t {0}", String.Format("{0:dd-MMM-yyyy}", Convert.ToDateTime(todayDate.Value))), font, new SolidBrush(Color.Black), startx, starty + offset);

                //Invoice ID
                g.DrawString(String.Format("Invoice #: {0}", invoiceid), font, new SolidBrush(Color.Black), startx + 320, starty + offset);

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
                g.DrawString(String.Format("Date:\t {0}", String.Format("{0:dd-MMM-yyyy}", Convert.ToDateTime(todayDate.Value))), font, new SolidBrush(Color.Black), startx + 80, starty + offset);

                //Invoice ID
                g.DrawString(String.Format("Invoice #: {0}", invoiceid), font, new SolidBrush(Color.Black), startx + 320, starty + offset);

            }


            offset += 5;
            //Customer Info
            font = new Font("Times New Roman", 10f, FontStyle.Regular);
            offset += font.Height + 5;
            g.DrawString(String.Format("Customer Name\t: {0}", custName.Text), font, new SolidBrush(Color.Black), startx, starty + offset);
            offset += font.Height + 2;
            g.DrawString(String.Format("Customer Contact\t: {0}", custContact.Text), font, new SolidBrush(Color.Black), startx, starty + offset);
            offset += font.Height + 2;
            g.DrawString(String.Format("Customer Address\t: {0}", custAddress.Text), font, new SolidBrush(Color.Black), startx, starty + offset);
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
                //Text Wrapping for Particular
                //Rectangle rect2 = new Rectangle(startx + 40, starty + offset, 275, starty + offset);
                //// Specify the text is wrapped.
                //TextFormatFlags flags = TextFormatFlags.WordBreak;
                //TextRenderer.DrawText(e.Graphics, listView1.Items[item].SubItems[1].Text, font, rect2, Color.Blue, flags);
                g.DrawString(String.Format("{0}", listView1.Items[item].SubItems[1].Text), font, new SolidBrush(Color.Black), startx + 40, starty + offset);
                g.DrawString(String.Format("{0}", listView1.Items[item].SubItems[2].Text), font, new SolidBrush(Color.Black), startx + 250, starty + offset);
                g.DrawString(String.Format("{0}%", listView1.Items[item].SubItems[4].Text), font, new SolidBrush(Color.Black), startx + 288, starty + offset);
                g.DrawString(String.Format("{0}", taxtypelist[item]), font, new SolidBrush(Color.Black), startx + 345, starty + offset);
                g.DrawString(String.Format("₹ {0}", listView1.Items[item].SubItems[6].Text), font, new SolidBrush(Color.Black), startx + 420, starty + offset);
                offset += font.Height + 5;
            }

            //Give some space
            //offset += 20;
            offset = 620;
            //Subtotal & Net Total etc here       
            font = new Font("Times New Roman", 10f, FontStyle.Regular);
            g.DrawString(String.Format("Sub-Total\t: ₹ {0}", subTotalBox.Text), font, new SolidBrush(Color.Black), startx, starty + offset);            
            g.DrawString(String.Format("Discount\t\t: ₹ {0}", discountBox.Text), font, new SolidBrush(Color.Black), startx + 250, starty + offset);
            offset += font.Height + 2;
            g.DrawString(String.Format("Net-Total\t\t: ₹ {0}", totalBox.Text), font, new SolidBrush(Color.Black), startx, starty + offset);
            offset += font.Height + 2;            
            g.DrawString(String.Format("Paid\t\t: ₹ {0}", paidBox.Text), font, new SolidBrush(Color.Black), startx, starty + offset);            
            g.DrawString(String.Format("Return\t\t: ₹ {0}", returnBox.Text), font, new SolidBrush(Color.Black), startx + 250, starty + offset);
            offset += font.Height + 2;
            g.DrawString(String.Format("Status\t\t: {0}", statusBox.Text), font, new SolidBrush(Color.Black), startx, starty + offset);
            offset += font.Height + 2;

            font = new Font("Times New Roman", 10f, FontStyle.Italic);

            string isNegative = "";
            string number = totalBox.Text;
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

        private void shopSaveButton_Click(object sender, EventArgs e)
        {
            try
            {
                if(shopLogo.Image != null)
                {
                    Properties.Settings.Default.shopName = shopNameBox.Text;
                    Properties.Settings.Default.shopAddress = shopAddressBox.Text;
                    var base64 = string.Empty;
                    using (MemoryStream ms = new MemoryStream())
                    {
                        shopLogo.Image.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                        base64 = Convert.ToBase64String(ms.ToArray());
                    }
                    Properties.Settings.Default.shopLogo = base64;
                }
                else
                {
                    Properties.Settings.Default.shopName = shopNameBox.Text;
                    Properties.Settings.Default.shopAddress = shopAddressBox.Text;
                }
                Properties.Settings.Default.Save();
                MessageBox.Show("Savd Successfully.", "Done", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void shopLogo_Click(object sender, EventArgs e)
        {
            try
            {
                using(OpenFileDialog op = new OpenFileDialog())
                {
                    op.Filter = "Image |*.jpg;*.png;*.jpeg;";
                    op.Title = "Logo for your shop";

                    if(op.ShowDialog() == DialogResult.OK)
                    {
                        shopLogo.Image = Image.FromFile(op.FileName);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void proKeyButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (proKeyBox.Text == "" || proKeyBox.Text == "0") { return; }

                //Checking and Verification
                //Phone Number to HEXA
                if(Convert.ToInt64(contactNoBox.Text).ToString("X") == proKeyBox.Text)
                {
                    Properties.Settings.Default.ProKey = proKeyBox.Text;
                    Properties.Settings.Default.Save();

                    MessageBox.Show(String.Format("Thank you for your donation. Now you will not see the nag screen."), "Done", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show(String.Format("The Product Key was not identified as a valid key."), "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }

                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void contactNoBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void criteriallist_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if(connection.State == ConnectionState.Closed) { connection.Open(); }


                if(criteriallist.SelectedIndex == 0)
                {
                    invoiceprefixbox.Visible = true;
                    invoiceprefixbox.Items.Clear();

                    command = new MySqlCommand("SELECT DISTINCT(invoiceid) FROM customertb", connection);
                    reader = command.ExecuteReader();

                    criterialvalue.Items.Clear();

                    while (reader.Read())
                    {
                        criterialvalue.Items.Add(reader[0].ToString());
                    }
                    reader.Dispose();

                    command = new MySqlCommand("SELECT DISTINCT(prefix) FROM customertb", connection);
                    reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        invoiceprefixbox.Items.Add(reader[0].ToString());
                    }
                    reader.Dispose();
                    if(invoiceprefixbox.Items.Count > 0) { invoiceprefixbox.SelectedIndex = 0; }                    
                }
                else if(criteriallist.SelectedIndex == 1)
                {
                    invoiceprefixbox.Visible = false;

                    command = new MySqlCommand("SELECT DISTINCT(invoicedate) FROM customertb", connection);
                    reader = command.ExecuteReader();

                    criterialvalue.Items.Clear();

                    while (reader.Read())
                    {
                        criterialvalue.Items.Add(String.Format("{0:dd-MM-yyyy}", Convert.ToDateTime(reader[0])));
                    }
                    reader.Dispose();
                }
                else if (criteriallist.SelectedIndex == 2)
                {
                    invoiceprefixbox.Visible = false;

                    command = new MySqlCommand("SELECT DISTINCT(customername) FROM customertb", connection);
                    reader = command.ExecuteReader();

                    criterialvalue.Items.Clear();

                    while (reader.Read())
                    {
                        criterialvalue.Items.Add(String.Format("{0}", reader[0].ToString()));
                    }
                    reader.Dispose();
                }
                else if (criteriallist.SelectedIndex == 3)
                {
                    invoiceprefixbox.Visible = false;

                    command = new MySqlCommand("SELECT DISTINCT(customercontact) FROM customertb", connection);
                    reader = command.ExecuteReader();

                    criterialvalue.Items.Clear();

                    while (reader.Read())
                    {
                        criterialvalue.Items.Add(String.Format("{0}", reader[0].ToString()));
                    }
                    reader.Dispose();
                }
                else if (criteriallist.SelectedIndex == 4)
                {
                    invoiceprefixbox.Visible = false;

                    command = new MySqlCommand("SELECT DISTINCT(status) FROM invoicerec", connection);
                    reader = command.ExecuteReader();

                    criterialvalue.Items.Clear();

                    while (reader.Read())
                    {
                        criterialvalue.Items.Add(String.Format("{0}", reader[0].ToString()));
                    }
                    reader.Dispose();
                }
                else { criterialvalue.Items.Clear(); invoiceprefixbox.Visible = false; }


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally { connection.Close(); }
        }

        private void listofcustomer(string invid = null, string invdate = null, string invcname = null, string invccon = null, string invcadd = null)
        {           
                ListViewItem lvItem = new ListViewItem();
                lvItem.SubItems[0].Text = invid;
                lvItem.SubItems[0].Font = new Font(new Font("Consolas", 10f), FontStyle.Regular);
                lvItem.SubItems.Add(invdate);
                lvItem.SubItems[1].Font = new Font(new Font("Consolas", 10f), FontStyle.Regular);
                lvItem.SubItems.Add(invcname);
                lvItem.SubItems[2].Font = new Font(new Font("Consolas", 10f), FontStyle.Regular);
                lvItem.SubItems.Add(invccon);
                lvItem.SubItems[3].Font = new Font(new Font("Consolas", 10f), FontStyle.Regular);
                lvItem.SubItems.Add(invcadd);
                lvItem.SubItems[4].Font = new Font(new Font("Consolas", 10f), FontStyle.Regular);                
                listView4.Items.Add(lvItem);            
        }

        private void searchButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (connection.State == ConnectionState.Closed) { connection.Open(); }

                if (criteriallist.SelectedIndex == 0)
                {
                    //Listview4 
                    listView4.Clear();
                    listView4.Columns.Add("Invoice ID", 90, HorizontalAlignment.Left);
                    listView4.Columns.Add("Invoice Date", 120, HorizontalAlignment.Center);
                    listView4.Columns.Add("Customer Name", 200, HorizontalAlignment.Center);
                    listView4.Columns.Add("Customer Contact", 150, HorizontalAlignment.Center);
                    listView4.Columns.Add("Customer Address", 280, HorizontalAlignment.Center);                    
                    listView4.Font = new Font("Times New Roman", 12, FontStyle.Bold);

                    //Search with Invoice ID
                    command = new MySqlCommand(String.Format("SELECT * FROM customertb WHERE invoiceid LIKE {0}", criterialvalue.Text), connection);
                    reader = command.ExecuteReader();                    

                    while (reader.Read())
                    {
                        listofcustomer(String.Format("{1}{0}", reader[0], reader[1]), String.Format("{0:dd-MM-yyyy}", Convert.ToDateTime(reader[2])), String.Format("{0}", reader[3]), String.Format("{0}", reader[4]), String.Format("{0}", reader[5]));
                    }
                    reader.Dispose();                    
                }
                else if (criteriallist.SelectedIndex == 1)
                {
                    //Listview4 
                    listView4.Clear();
                    listView4.Columns.Add("Invoice ID", 90, HorizontalAlignment.Left);
                    listView4.Columns.Add("Invoice Date", 120, HorizontalAlignment.Center);
                    listView4.Columns.Add("Customer Name", 200, HorizontalAlignment.Center);
                    listView4.Columns.Add("Customer Contact", 150, HorizontalAlignment.Center);
                    listView4.Columns.Add("Customer Address", 280, HorizontalAlignment.Center);
                    listView4.Font = new Font("Times New Roman", 12, FontStyle.Bold);

                    //Search with Invoice Date
                    command = new MySqlCommand(String.Format("SELECT * FROM customertb WHERE invoicedate LIKE '{0:yyyy-MM-dd}'", Convert.ToDateTime(criterialvalue.Text)), connection);
                    reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        listofcustomer(String.Format("{1}{0}", reader[0], reader[1]), String.Format("{0:dd-MM-yyyy}", Convert.ToDateTime(reader[2])), String.Format("{0}", reader[3]), String.Format("{0}", reader[4]), String.Format("{0}", reader[5]));
                    }
                    reader.Dispose();
                }
                else if (criteriallist.SelectedIndex == 2)
                {
                    //Listview4 
                    listView4.Clear();
                    listView4.Columns.Add("Invoice ID", 90, HorizontalAlignment.Left);
                    listView4.Columns.Add("Invoice Date", 120, HorizontalAlignment.Center);
                    listView4.Columns.Add("Customer Name", 200, HorizontalAlignment.Center);
                    listView4.Columns.Add("Customer Contact", 150, HorizontalAlignment.Center);
                    listView4.Columns.Add("Customer Address", 280, HorizontalAlignment.Center);
                    listView4.Font = new Font("Times New Roman", 12, FontStyle.Bold);

                    //Search with Invoice Date
                    command = new MySqlCommand(String.Format("SELECT * FROM customertb WHERE customername LIKE '{0}'", criterialvalue.Text), connection);
                    reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        listofcustomer(String.Format("{1}{0}", reader[0], reader[1]), String.Format("{0:dd-MM-yyyy}", Convert.ToDateTime(reader[2])), String.Format("{0}", reader[3]), String.Format("{0}", reader[4]), String.Format("{0}", reader[5]));
                    }
                    reader.Dispose();
                }
                else if (criteriallist.SelectedIndex == 3)
                {
                    //Listview4 
                    listView4.Clear();
                    listView4.Columns.Add("Invoice ID", 90, HorizontalAlignment.Left);
                    listView4.Columns.Add("Invoice Date", 120, HorizontalAlignment.Center);
                    listView4.Columns.Add("Customer Name", 200, HorizontalAlignment.Center);
                    listView4.Columns.Add("Customer Contact", 150, HorizontalAlignment.Center);
                    listView4.Columns.Add("Customer Address", 280, HorizontalAlignment.Center);
                    listView4.Font = new Font("Times New Roman", 12, FontStyle.Bold);

                    //Search with Invoice Date
                    command = new MySqlCommand(String.Format("SELECT * FROM customertb WHERE customercontact LIKE '{0}'", criterialvalue.Text), connection);
                    reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        listofcustomer(String.Format("{1}{0}", reader[0], reader[1]), String.Format("{0:dd-MM-yyyy}", Convert.ToDateTime(reader[2])), String.Format("{0}", reader[3]), String.Format("{0}", reader[4]), String.Format("{0}", reader[5]));
                    }
                    reader.Dispose();
                }
                else if (criteriallist.SelectedIndex == 4)
                {
                    //Listview4 
                    listView4.Clear();
                    listView4.Columns.Add("Invoice ID", 90, HorizontalAlignment.Left);
                    listView4.Columns.Add("Invoice Date", 120, HorizontalAlignment.Center);
                    listView4.Columns.Add("Customer Name", 200, HorizontalAlignment.Center);
                    listView4.Columns.Add("Customer Contact", 150, HorizontalAlignment.Center);
                    listView4.Columns.Add("Customer Address", 280, HorizontalAlignment.Center);
                    listView4.Font = new Font("Times New Roman", 12, FontStyle.Bold);

                    //Search with Invoice Date
                    command = new MySqlCommand(String.Format("SELECT ctb.invoiceid, ctb.prefix, ctb.invoicedate, ctb.customername, ctb.customercontact, ctb.customeraddress FROM customertb AS ctb JOIN invoicerec AS itb WHERE CONCAT(ctb.prefix, ctb.invoiceid) = itb.invoiceid AND itb.`status` = '{0}'", criterialvalue.Text), connection);
                    reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        listofcustomer(String.Format("{1}{0}", reader[0], reader[1]), String.Format("{0:dd-MM-yyyy}", Convert.ToDateTime(reader[2])), String.Format("{0}", reader[3]), String.Format("{0}", reader[4]), String.Format("{0}", reader[5]));
                    }
                    reader.Dispose();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally { connection.Close(); }
        }

        private void listView4_SelectedIndexChanged(object sender, EventArgs e)
        {
            //SELECT itb.invoiceid, ctb.customername, ctb.customercontact, ctb.customeraddress, itb.partname, itb.partquantity, itb.partrate, itb.parttax, itb.parttaxtype, itb.parttaxamount, itb.partnetprice FROM customertb AS ctb JOIN invoicetb AS itb WHERE CONCAT(ctb.prefix, ctb.invoiceid) = itb.invoiceid ;
            try
            {
                //Select the list and show a form for that particular invoice, allow to print as well
            }
            catch(Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void listView4_DoubleClick(object sender, EventArgs e)
        {
            //SELECT itb.invoiceid, ctb.customername, ctb.customercontact, ctb.customeraddress, itb.partname, itb.partquantity, itb.partrate, itb.parttax, itb.parttaxtype, itb.parttaxamount, itb.partnetprice FROM customertb AS ctb JOIN invoicetb AS itb WHERE CONCAT(ctb.prefix, ctb.invoiceid) = itb.invoiceid ;
            try
            {
                //Select the list and show a form for that particular invoice, allow to print as well
                if(listView4.SelectedItems.Count > 0)
                {
                    cashMemo cm = new cashMemo();

                    //Invoice Detail
                    cm.invoiceDateLabel.Text = listView4.Items[listView4.SelectedIndices[0]].SubItems[1].Text;
                    cm.invoiceLabel.Text = listView4.Items[listView4.SelectedIndices[0]].SubItems[0].Text;

                    //Customer Details
                    cm.invoiceCustNameLabel.Text = listView4.Items[listView4.SelectedIndices[0]].SubItems[2].Text;
                    cm.invoiceCustContactLabel.Text = listView4.Items[listView4.SelectedIndices[0]].SubItems[3].Text;
                    cm.invoiceCustAddressLabel.Text = listView4.Items[listView4.SelectedIndices[0]].SubItems[4].Text;

                    //Open Connection
                    if (connection.State == ConnectionState.Closed) { connection.Open(); }

                    //From invoicetb Table
                    command = new MySqlCommand(String.Format("SELECT * FROM invoicetb WHERE invoiceid = '{0}'", listView4.Items[listView4.SelectedIndices[0]].SubItems[0].Text), connection);
                    reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        ListViewItem lvItem = new ListViewItem();
                        lvItem.SubItems[0].Text = (cm.listView1.Items.Count + 1).ToString();
                        lvItem.SubItems[0].Font = new Font(new Font("Consolas", 10f), FontStyle.Regular);
                        lvItem.SubItems.Add(reader[1].ToString());
                        lvItem.SubItems[1].Font = new Font(new Font("Consolas", 10f), FontStyle.Regular);
                        lvItem.SubItems.Add(reader[2].ToString());
                        lvItem.SubItems[2].Font = new Font(new Font("Consolas", 10f), FontStyle.Regular);
                        lvItem.SubItems.Add(reader[3].ToString());
                        lvItem.SubItems[3].Font = new Font(new Font("Consolas", 10f), FontStyle.Regular);
                        lvItem.SubItems.Add(reader[4].ToString());
                        lvItem.SubItems[4].Font = new Font(new Font("Consolas", 10f), FontStyle.Regular);
                        lvItem.SubItems.Add(reader[5].ToString());
                        lvItem.SubItems[5].Font = new Font(new Font("Consolas", 10f), FontStyle.Regular);
                        lvItem.SubItems.Add(reader[6].ToString());
                        lvItem.SubItems[6].Font = new Font(new Font("Consolas", 10f), FontStyle.Regular);
                        lvItem.SubItems.Add(reader[7].ToString());
                        lvItem.SubItems[7].Font = new Font(new Font("Consolas", 10f), FontStyle.Regular);
                        cm.listView1.Items.Add(lvItem);
                    }
                    reader.Dispose();

                    //From invoicerec table
                    command = new MySqlCommand(String.Format("SELECT * FROM invoicerec WHERE invoiceid = '{0}'", listView4.Items[listView4.SelectedIndices[0]].SubItems[0].Text), connection);
                    reader = command.ExecuteReader();

                    string inword = "";
                    string isNegative = ""; 

                    while (reader.Read())
                    {
                        cm.invoiceGrossLabel.Text = String.Format("₹ {0}", reader[2].ToString());
                        cm.invoiceDiscountLabel.Text = String.Format("₹ {0}", reader[3].ToString());
                        cm.invoiceNetLabel.Text = String.Format("₹ {0}", reader[4].ToString());
                        cm.invoicePaidLabel.Text = String.Format("₹ {0}", reader[5].ToString());
                        cm.invoiceReturnLabel.Text = String.Format("₹ {0}", reader[6].ToString());
                        cm.invoiceStatusLabel.Text = String.Format("{0}", reader[7].ToString());

                        if(reader[7].ToString() == "Unpaid" || reader[7].ToString() == "Partial Paid" || reader[7].ToString() == "Cancelled" || reader[7].ToString() == "Exchanged")
                        {
                            cm.invoiceStatusLabel.ForeColor = Color.Red;
                        }
                        else if (reader[7].ToString() == "Paid")
                        {
                            cm.invoiceStatusLabel.ForeColor = Color.DarkGreen;
                        }
                        else if (reader[7].ToString() == "Check")
                        {
                            cm.invoiceStatusLabel.ForeColor = Color.DarkSlateBlue;
                        }
                        else
                        {
                            cm.invoiceStatusLabel.ForeColor = Color.DarkOrange;
                        }

                        string number = reader[4].ToString();
                        if (number.Contains("-"))
                        {
                            isNegative = "Minus ";
                            number = number.Substring(1, number.Length - 1);
                        }
                        if (number == "0")
                        {
                            inword = "Zero Only";
                        }
                        else
                        {

                           inword = String.Format("({0}{1})", isNegative, ConvertToWords(number));
                        }

                        cm.invoiceWordLabel.Text = inword;
                    }
                    reader.Dispose();

                    //Show Dialog
                    cm.StartPosition = FormStartPosition.CenterParent;
                    cm.ShowInTaskbar = false;
                    cm.ShowDialog();
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            //Call for Customer Payment Status
            loadcustomerstatus();


        }

        private void GetCustomerReport_Click(object sender, EventArgs e)
        {
            try
            {
                if (Properties.Settings.Default.isPro == false)
                {
                    MessageBox.Show("This feature is available only on Pro Version. If you want to use it, kindly consider to donate.", "Free Version Limitation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                CustomerReport cr = new CustomerReport();
                cr.ShowInTaskbar = false;
                cr.ShowDialog();
            }
            catch(Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void GSTReportButton_Click(object sender, EventArgs e)
        {
            try
            {
                if(Properties.Settings.Default.isPro == false)
                {
                    MessageBox.Show("This feature is available only on Pro Version. If you want to use it, kindly consider to donate.", "Free Version Limitation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                GSTReport cr = new GSTReport();
                cr.ShowInTaskbar = false;
                cr.ShowDialog();
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void TransReportButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (Properties.Settings.Default.isPro == false)
                {
                    MessageBox.Show("This feature is available only on Pro Version. If you want to use it, kindly consider to donate.", "Free Version Limitation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                TransactionReport cr = new TransactionReport();
                cr.ShowInTaskbar = false;
                cr.ShowDialog();
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void partTaxAmount_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                if (partQuantity.Text == "" || partQuantity.SelectedIndex == -1) { return; }
                decimal d = Convert.ToDecimal(Convert.ToDecimal(partQuantity.Text) * (Convert.ToDecimal(partRate.Text)) + Convert.ToDecimal(partTaxAmount.Text));
                partTotal.Text = d.ToString("F02");
            }
            catch(Exception ex) { MessageBox.Show(ex.Message); }
        }
    }
}