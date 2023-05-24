using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;
using MySql.Data.MySqlClient;

namespace WpfApp1
{
    /// <summary>
    /// Логика взаимодействия для LoginView.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        MySqlConnection conn;
        public LoginWindow()
        {
            InitializeComponent();
            Connect connect = new Connect();
            connect.getConnection();
            
        }
        


        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }

        private void btnMinimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

 

        private void txtUser_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            String empID = txtID.Text;
            String pass = txtPass.Password;
            Connect.conn.Open();
            String query = "SELECT * FROM employees WHERE employee_id="+ empID +" AND password='"+ pass +"' ";
            MySqlCommand cmd = new MySqlCommand(query, Connect.conn);
            MySqlDataReader dr = cmd.ExecuteReader();

            if (dr.Read())
            {
                if (dr[6].ToString()=="cashier")
                {
                    MainWindow main = new MainWindow();
                    main.user.Text = dr[1].ToString();

                    byte[] imageData = (byte[])dr["user_img"];
                    BitmapImage bitmapImage = new BitmapImage();
                    bitmapImage.BeginInit();
                    bitmapImage.StreamSource = new MemoryStream(imageData);
                    bitmapImage.EndInit();
                    ImageSource imageSource = bitmapImage;
                    main.userImage.ImageSource = imageSource;

                    main.employee.Visibility = Visibility.Collapsed;

                    main.Show();
                    this.Close();
                }
                else if (dr[6].ToString() == "manager")
                {
                    MainWindow main = new MainWindow();
                    main.user.Text = dr[1].ToString();

                    byte[] imageData = (byte[])dr["user_img"];
                    BitmapImage bitmapImage = new BitmapImage();
                    bitmapImage.BeginInit();
                    bitmapImage.StreamSource = new MemoryStream(imageData);
                    bitmapImage.EndInit();
                    ImageSource imageSource = bitmapImage;
                    main.userImage.ImageSource = imageSource;

                    main.Show();
                    this.Close();
                }
            } 
            else
            {
                MessageBox.Show("Username or password incorrect");
            }
            Connect.conn.Close();

            
        }
    }
}
