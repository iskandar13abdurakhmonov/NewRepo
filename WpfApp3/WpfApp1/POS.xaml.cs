using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
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

namespace WpfApp1
{
    /// <summary>
    /// Логика взаимодействия для POS.xaml
    /// </summary>
    public partial class POS : Page
    {
        MySqlConnection conn;
        string oID;
        public POS()
        {
            InitializeComponent();
            Connect connect = new Connect();
            connect.getConnection();
            loadData("ALL");

            Random random = new Random();   
            orderID.Content=random.Next(1,10000);
            oID = ""+orderID.Content;
        }

        



        public void loadData(string cat)
        {
            try
            {
                ScrollViewer scrollbar = new ScrollViewer();
                scrollbar.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
                scrollbar.HorizontalScrollBarVisibility = ScrollBarVisibility.Auto;

                WrapPanel panel = new WrapPanel();
                panel.Width = 800;

                Connect.conn.Open();

                String all_query;

                if (cat == "ALL")
                {
                    all_query = "SELECT * FROM products;";
                }
                else
                {
                    all_query = "SELECT * FROM products WHERE category_id = '"+ cat +"'";
                }


                MySqlCommand cmd = new MySqlCommand(all_query, Connect.conn);
                MySqlDataReader dr = cmd.ExecuteReader();

                int count = 0;
                while (dr.Read())
                {
                    StackPanel stackpanel = new StackPanel();
                    stackpanel.Orientation = Orientation.Horizontal;

                    Image image = new Image();
                    image.Width = 100;
                    image.Height = 100;
                    //image.Source = new BitmapImage(new Uri(dr[3].ToString(), UriKind.Relative));

                    byte[] imageData = (byte[])dr["product_image"];
                    BitmapImage bitmapImage = new BitmapImage();
                    bitmapImage.BeginInit();
                    bitmapImage.StreamSource = new MemoryStream(imageData);
                    bitmapImage.EndInit();
                    ImageSource imageSource = bitmapImage;
                    image.Source = imageSource;

                    Label pname = new Label();
                    pname.Content = dr[1].ToString();


                    Label price = new Label();
                    price.Content = dr[2].ToString();

                    stackpanel.Children.Add(image);
                    stackpanel.Children.Add(pname);
                    stackpanel.Children.Add(price);


                    Button btn = new Button();
                    btn.Width = 250;
                    btn.Height = 110;
                    btn.Name = dr[0].ToString();
                    btn.Content = stackpanel;
                    btn.Margin = new Thickness(2);
                    btn.Background = new SolidColorBrush(Color.FromRgb(225, 234, 244));

                    btn.Click += addProduct;

                    panel.Children.Add(btn);
                    count = count + 1;
                    items_count.Content = count;
                }

                scrollbar.Content = panel;
                lItem.Content = scrollbar;
            }
            catch (Exception ex)
            {
                MessageBox.Show("" + ex);
            }
            finally
            {
                Connect.conn.Close();
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            loadData("ALL");
        }

        private void beverages_btn_Click(object sender, RoutedEventArgs e)
        {
            loadData("A1");
        }

        private void breadnbakery_btn_Click(object sender, RoutedEventArgs e)
        {
            loadData("B1");
        }

        private void Canned_Goods_btn_Click(object sender, RoutedEventArgs e)
        {
            loadData("C1");
        }

        private void baby_item_btn_Click(object sender, RoutedEventArgs e)
        {
            loadData("D1");
        }
        private void bakery_btn_Click(object sender, RoutedEventArgs e)
        {
            loadData("E1");
        }
        private void cereal_btn_Click(object sender, RoutedEventArgs e)
        {
            loadData("F1");
        }
        private void condimentsNspices_btn_Click(object sender, RoutedEventArgs e)
        {
            loadData("G1");
        }
        private void deli_btn_Click(object sender, RoutedEventArgs e)
        {
            loadData("H1");
        }
        private void fishNshellfish_btn_Click(object sender, RoutedEventArgs e)
        {
            loadData("I1");
        }
        private void frozen_foods_btn_Click(object sender, RoutedEventArgs e)
        {
            loadData("J1");
        }
        private void fruits_btn_Click(object sender, RoutedEventArgs e)
        {
            loadData("K1");
        }
        private void health_care_btn_Click(object sender, RoutedEventArgs e)
        {
            loadData("L1");
        }
        private void household_supplies_btn_Click(object sender, RoutedEventArgs e)
        {
            loadData("M1");
        }
        private void meat_btn_Click(object sender, RoutedEventArgs e)
        {
            loadData("N1");
        }
        private void pastaNrice_btn_Click(object sender, RoutedEventArgs e)
        {
            loadData("O1");
        }
        private void personal_care_btn_Click(object sender, RoutedEventArgs e)
        {
            loadData("P1");
        }
        private void saucesNoil_btn_Click(object sender, RoutedEventArgs e)
        {
            loadData("Q1");
        }
        private void snacks_btn_Click(object sender, RoutedEventArgs e)
        {
            loadData("R1");
        }
        private void vegetables_btn_Click(object sender, RoutedEventArgs e)
        {
            loadData("S1");
        }


        public void addProduct(object sender, RoutedEventArgs e)
        {
            try
            {
                var btn = sender as Button;
                string product_id = btn.Name;

                Connect.conn.Open();
                String query = "SELECT * FROM products WHERE product_id = '"+ product_id +"' ;";
                MySqlCommand cmd = new MySqlCommand(query, Connect.conn);
                MySqlDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    var productData = new ProductsData
                    {
                        product_name = dr[1].ToString(),
                        product_price = dr[2].ToString(),
                    };

                    Connect.conn.Close();
                    Connect.conn.Open();

                    String itemQuery = "INSERT INTO orderItems VALUES('"+orderID.Content+"','"+ productData.product_name + "',"+ productData.product_price +")";                    
                    MySqlCommand itemCmd=new MySqlCommand(itemQuery, Connect.conn);
                    itemCmd.ExecuteNonQuery();

                    sItem.Items.Add(productData);

                    double subtot, tax, grandTotal;

                    subtot = Int32.Parse(subtotal.Text) + Int32.Parse(productData.product_price);
                    subtotal.Text = Convert.ToString(subtot);

                    tax = (subtot * (12/100));
                    taxItem.Text = Convert.ToString(Math.Round(tax, 2));

                    grandTotal = subtot + tax;
                    total.Text = Convert.ToString(grandTotal);
                }

            }
            catch (Exception ex)
            {
               // MessageBox.Show("" + ex);
            }
            finally
            {
                Connect.conn.Close();
            }
        }

        public void RemoveItem_Click(object sender, RoutedEventArgs e)
        {


            ProductsData ProductsData = sItem.SelectedItem as ProductsData;

            int subtot = Int32.Parse(subtotal.Text) - Int32.Parse(ProductsData.product_price);
            subtotal.Text = Convert.ToString(subtot);

            double tax = subtot * 100;
            taxItem.Text = Convert.ToString(Math.Round(tax, 2));

            double grandTotal = subtot - tax;
            total.Text = Convert.ToString(grandTotal);

            sItem.Items.RemoveAt(sItem.SelectedIndex);
        }

        private void receipt_btn_Click(object sender, RoutedEventArgs e)
        {

            receipt rct = new receipt(oID);
            rct.Show();

            PrintDialog pd = new PrintDialog();
            if (pd.ShowDialog() != true) return;
            pd.PrintVisual(rct, "Print Receipt Window");

           

           /* System.Windows.Controls.PrintDialog Printdlg = new System.Windows.Controls.PrintDialog();
            if ((bool)Printdlg.ShowDialog().GetValueOrDefault())
            {
                Size pageSize = new Size(Printdlg.PrintableAreaWidth, Printdlg.PrintableAreaHeight);
                // sizing of the element
                sItem.Measure(pageSize);
                sItem.Arrange(new Rect(5, 5, pageSize.Width, pageSize.Height));

                FlowDocument receipt = new FlowDocument();
                Paragraph p = new Paragraph();
                p.Inlines.Add(new Bold(new Run("Receipt\n")));
                p.Inlines.Add(new Run("Date: " + DateTime.Now.ToString() + "\n\n"));
                p.Inlines.Add(new Run("Item\t\tPrice\n"));

                Printdlg.PrintVisual(sItem, "Print Grid");
            }
           */


            //PrintDialog printDialog = new PrintDialog();
            //if (printDialog.ShowDialog() == true)
            //{
            //    FlowDocument receipt = new FlowDocument();
            //    Paragraph p = new Paragraph();
            //    p.Inlines.Add(new Bold(new Run("Receipt\n")));
            //    p.Inlines.Add(new Run("Date: " + DateTime.Now.ToString() + "\n\n"));
            //    p.Inlines.Add(new Run("Item\t\tPrice\n"));
            //    foreach (DataRowView row in sItem.Items)
            //    {
            //        string item = row["Item"].ToString();
            //        string price = row["Price"].ToString();
            //        p.Inlines.Add(new Run(item + "\t\t" + price + "\n"));
            //    }
            //    receipt.Blocks.Add(p);
            //    printDialog.PrintDocument(((IDocumentPaginatorSource)receipt).DocumentPaginator, "Receipt");
            //}


        }

        private void cash_Click(object sender, RoutedEventArgs e)
        {
           /* string MyConString = "server=localhost;" + "database=store;" + "port=3306;" + "user=root;" + "password=mdist";
            MySqlConnection connection = new MySqlConnection(MyConString);
            MySqlCommand command = connection.CreateCommand();
            command.CommandText = "INSERT INTO orders (product_name, product_price) VALUES (@value1, @value2)";

            command.Parameters.AddWithValue("@value1", "");
            command.Parameters.AddWithValue("@value2", "");
            connection.Open();

            foreach(DataRow row in sItem.Items)
            {
                command.Parameters["@value1"].Value = row["product_name"];
                command.Parameters["@value2"].Value = row["product_price"];
                command.ExecuteNonQuery();
            }

            connection.Close();
           */
        }
    }
}

