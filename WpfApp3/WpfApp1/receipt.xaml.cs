using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;
using System.Windows.Threading;
using MySql.Data.MySqlClient;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for receipt.xaml
    /// </summary>
    public partial class receipt : Window
    {
        public receipt(string oID)
        {
            InitializeComponent();
            startclock();

            

            try
            {
                ScrollViewer scrollbar = new ScrollViewer();
                scrollbar.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
                scrollbar.HorizontalScrollBarVisibility = ScrollBarVisibility.Auto;

                WrapPanel panel = new WrapPanel();
                panel.Width = 300;
                panel.Height = 300;

                order.Text = oID;

                Connect.conn.Open();
                String sel_query = "SELECT * FROM orderitems WHERE orderID = '" + oID + "'";
                
                MySqlCommand cmd = new MySqlCommand(sel_query, Connect.conn);
                
                MySqlDataReader dr = cmd.ExecuteReader();
                


                int count = 0;
                while (dr.Read())
                {
                    StackPanel stackPanel = new StackPanel();
                    stackPanel.Orientation = Orientation.Horizontal;

                    stackPanel.Width = 300;

                    Label OrderID = new Label();
                    OrderID.Content = dr[0].ToString();
                    OrderID.Margin = new Thickness(10, 0, 80, 0);

                    Label ProductName = new Label();
                    ProductName.Content = dr[1].ToString();
                    ProductName.Margin = new Thickness(0, 0, 20, 0);

                    Label ProductPrice = new Label();
                    ProductPrice.Content = dr[2].ToString();
                    ProductPrice.HorizontalAlignment = HorizontalAlignment.Right;


                    stackPanel.Children.Add(OrderID);
                    stackPanel.Children.Add(ProductName);
                    stackPanel.Children.Add(ProductPrice);

                    panel.Children.Add(stackPanel);
                    count = count + 1;
                }

                scrollbar.Content = panel;
                rItem.Content = panel;

                
                

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                Connect.conn.Close();
            }


            Connect.conn.Open();
            String subtot_query = "SELECT SUM(productPrice) FROM orderItems WHERE orderID = '" + oID + "'";
            MySqlCommand st_cmd = new MySqlCommand(subtot_query, Connect.conn);
            MySqlDataReader st_dr = st_cmd.ExecuteReader();


            if(st_dr.Read())
            {
                subtotal.Text = st_dr[0].ToString();
            }
            

            Connect.conn.Close();
        }

        private void startclock()
        {
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += tickevent;
            timer.Start();
        }

        private void tickevent(object sender, EventArgs e)
        {
            receiptDate.Text = DateTime.Now.ToString();
        }
    }
}
