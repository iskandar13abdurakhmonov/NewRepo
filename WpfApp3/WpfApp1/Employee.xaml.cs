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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Google.Protobuf.WellKnownTypes;
using Microsoft.Win32;
using MySql.Data.MySqlClient;

namespace WpfApp1
{
    /// <summary>
    /// Логика взаимодействия для Employee.xaml
    /// </summary>
    public partial class Employee : Page
    {
        MySqlConnection conn;
        public Employee()
        {
            InitializeComponent();
            Connect connect = new Connect();
            connect.getConnection();
        }
        private void select_image(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image files (*.jpg; *.png;*.jpeg)|*.png;*.jpeg|All files (*.*)|*.*";
            openFileDialog.FilterIndex = 1;
            if (openFileDialog.ShowDialog() == true)
            {
                selected_image.Source = new BitmapImage(new Uri(openFileDialog.FileName));
                //BitmapImage bitmapImage = new BitmapImage(new Uri(openFileDialog.FileName));
                //JpegBitmapEncoder encoder = new JpegBitmapEncoder();
                //encoder.Frames.Add(BitmapFrame.Create(bitmapImage));
                //MemoryStream memoryStream = new MemoryStream();
                //encoder.Save(memoryStream);
                //byte[] imageBt = memoryStream.ToArray();

                byte[] image = null;
                FileStream stream = new FileStream(openFileDialog.FileName, FileMode.Open, FileAccess.Read);
                BinaryReader reader = new BinaryReader(stream);
                image = reader.ReadBytes((int)stream.Length);
            }
        }
        private void emp_add_Click(object sender, RoutedEventArgs e)
        {
            Connect.conn.Open();
            String add_query = "INSERT INTO employees(employee_id,employee_name,dob,phone,email,address,role,password,user_img) VALUES("+emp_id.Text+",'"+emp_name.Text+"','"+emp_dob.Text+"',"+int.Parse(emp_phone.Text)+",'"+emp_email.Text+"','"+emp_address.Text+"','"+emp_role.Text+"',"+int.Parse(emp_password.Text)+",'"+selected_image+"')";
            MySqlCommand cmd = new MySqlCommand(add_query, Connect.conn);
            cmd.ExecuteNonQuery();
            MessageBox.Show("Record Saved");
        }

        private void emp_update_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Connect.conn.Open();
                String u_query = "UPDATE employees SET employee_id = "+emp_id.Text+", employee_name = '"+emp_name.Text+"', dob = '"+emp_dob.Text+"', phone = "+int.Parse(emp_phone.Text)+", email = '"+emp_email.Text+"', address = '"+emp_address.Text+"', role = '"+emp_role.Text+"', password = "+int.Parse(emp_password.Text)+" WHERE employee_id = "+emp_id.Text+"";
                MySqlCommand u_cmd = new MySqlCommand(u_query, Connect.conn);
                u_cmd.ExecuteNonQuery();
                MessageBox.Show("Record Updated!");
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("" + ex);
            }
            finally
            {
                Connect.conn.Close();
            }
        }

        private void emp_search_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Connect.conn.Open();
                String query = "SELECT * FROM employees WHERE employee_id="+ emp_id.Text;
                MySqlCommand q_cmd = new MySqlCommand(query, Connect.conn);
                MySqlDataReader dr = q_cmd.ExecuteReader();

                if(dr.Read())
                {
                    emp_id.Text = dr[0].ToString();
                    emp_name.Text = dr[1].ToString();
                    emp_dob.Text = dr[2].ToString();
                    emp_phone.Text = dr[3].ToString();
                    emp_email.Text = dr[4].ToString();
                    emp_address.Text = dr[5].ToString();
                    emp_role.Text = dr[6].ToString();
                    emp_password.Text = dr[7].ToString();

                    byte[] imageData = (byte[])dr["user_img"];
                    BitmapImage bitmapImage = new BitmapImage();
                    bitmapImage.BeginInit();
                    bitmapImage.StreamSource = new MemoryStream(imageData);
                    bitmapImage.EndInit();
                    ImageSource imageSource = bitmapImage;
                    selected_image.Source = imageSource;
                }
                else
                {
                    MessageBox.Show("EmployeeID not found!");
                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("" + ex);
            }
            finally
            {
                Connect.conn.Close();
            }
        }

        private void emp_delete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Connect.conn.Open();
                String d_query = "DELETE FROM employees WHERE employee_id = " + emp_id.Text;
                MySqlCommand d_cmd = new MySqlCommand(d_query, Connect.conn);
                d_cmd.ExecuteNonQuery();
                MessageBox.Show("Record Deleted");
                clear();
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("" + ex);
            }
            finally
            {
                Connect.conn.Close();
            }
        }

        public void clear()
        {
            
        }

        private void emp_clear_Click(object sender, RoutedEventArgs e)
        {
            emp_id.Clear();
            emp_name.Clear();
            emp_phone.Clear();
            emp_email.Clear();
            emp_address.Clear();
            emp_role.Clear();
            emp_password.Clear();
        }
    }
}
