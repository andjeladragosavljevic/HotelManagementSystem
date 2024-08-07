using MySql.Data.MySqlClient;
using ProjekatB.Model;
using ProjekatB.Resources;
using System;
using System.Collections.Generic;
using System.Configuration;
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

namespace ProjekatB
{

    public partial class UpdateProfile : Window
    {
        private readonly string connectionString = ConfigurationManager.ConnectionStrings["hci"].ConnectionString;
        public UpdateProfile()
        {

            InitializeComponent();

        }

        private void btn_signeUp_Click(object sender, RoutedEventArgs e)
        {
            string ime = firstnameTxtBox.Text;
            string prezime = lastnameTxtBox.Text;
            string email = emailTxtBox.Text;
            string JMB = jmbTxtBox.Text;
            string username = txtUsername.Text;
            string password = txtPassword.Password.ToString();
            string repeatPassword = txtRepeatPassword.Password.ToString();

            if (password == repeatPassword)
            {
                Person person = new Person()
                {
                    FirstName = firstnameTxtBox.Text,
                    LastName = lastnameTxtBox.Text,
                    Email = emailTxtBox.Text,
                    Jmb = jmbTxtBox.Text,
                    Username = txtUsername.Text,
                    Password = txtPassword.Password.ToString()
                };

                MySqlConnection connection = new MySqlConnection(connectionString);
                connection.Open();
                try
                {
                    MySqlCommand command = new MySqlCommand("UPDATE OSOBA SET Ime=@Ime, Prezime=@Prezime, Email=@Email, Username=@Username, Password=@Password WHERE JMB=@JMB", connection);
                    command.Parameters.AddWithValue("@Ime", person.FirstName);
                    command.Parameters.AddWithValue("@Prezime", person.LastName);
                    command.Parameters.AddWithValue("@Email", person.Email);
                    command.Parameters.AddWithValue("@Username", person.Username);
                    command.Parameters.AddWithValue("@Password", person.Password);
                    command.Parameters.AddWithValue("@JMB", person.Jmb);

                    command.ExecuteNonQuery();
                    MessageBox.Show(LocalizedStrings.Instance["UpdatedProfile"]);
                    
                    
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    connection.Close();
                }
            }
            else
            {
                MessageBox.Show(LocalizedStrings.Instance["NotMatch"]);
            }
        }

        private List<Control> AllChildren(DependencyObject parent)
        {
            var list = new List<Control> { };
            for (int count = 0; count < VisualTreeHelper.GetChildrenCount(parent); count++)
            {
                var child = VisualTreeHelper.GetChild(parent, count);
                if (child is Control)
                {
                    list.Add(child as Control);
                }
                list.AddRange(AllChildren(child));
            }
            return list;
        }
        private void clear() {
            List<Control> children = AllChildren(wind);

            foreach (object ctl in children)
            {
                string msg = ctl.ToString();

                if (ctl.GetType() == typeof(TextBox))
                {
                    if (((TextBox)ctl).Name != "jmbTxtBox")
                        ((TextBox)ctl).Text = String.Empty;

                }


            }
        }
        private void Delete_click(object sender, RoutedEventArgs e)
        {
            clear();
          
        }

    }
}
