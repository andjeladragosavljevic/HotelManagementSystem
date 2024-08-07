using MySql.Data.MySqlClient;
using ProjekatB.Model;
using ProjekatB.Resources;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Ribbon;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ProjekatB
{

    public partial class SigneUpPage : Window
    {
        private static readonly string connectionString = ConfigurationManager.ConnectionStrings["hci"].ConnectionString;
        MySqlConnection connection = new MySqlConnection(connectionString);
        public List<Person> persons = new List<Person>();
        public SigneUpPage()
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
            if (firstnameTxtBox.Text != "" && lastnameTxtBox.Text != "" && jmbTxtBox.Text != "" && txtUsername.Text != "" && txtPassword.Password.ToString() != "")
            {

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

                    connection.Open();

                    MySqlCommand command = new MySqlCommand("INSERT INTO OSOBA (JMB, Ime, Prezime, Email, Username, Password) values (@JMB, @Ime, @Prezime, @Email, @Username, @Password)", connection);
                    command.Parameters.AddWithValue("@JMB", person.Jmb);
                    command.Parameters.AddWithValue("@Ime", person.FirstName);
                    command.Parameters.AddWithValue("@Prezime", person.LastName);
                    command.Parameters.AddWithValue("@Email", person.Email);
                    command.Parameters.AddWithValue("@Username", person.Username);
                    command.Parameters.AddWithValue("@Password", person.Password);
                    try
                    {
                        DataTable dataTable1 = new DataTable();
                        dataTable1.Load(command.ExecuteReader());
                        MySqlCommand command1 = new MySqlCommand("INSERT INTO GOST (JMB) values (@JMB)", connection);
                        command1.Parameters.AddWithValue("@JMB", person.Jmb);
                        command1.ExecuteNonQuery();
                        persons.Add(person);
                        MessageBox.Show(LocalizedStrings.Instance["Profile"], "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                        clear();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "...");
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
            else
            {
                MessageBox.Show(LocalizedStrings.Instance["Required"], "Warning", MessageBoxButton.OK, MessageBoxImage.Error);
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
        public void clear()
        {
            List<Control> children = AllChildren(wind);

            foreach (object ctl in children)
            {
                string msg = ctl.ToString();

                if (ctl.GetType() == typeof(TextBox))
                {
                    if (((TextBox)ctl).Name != "nametxtBox")
                        ((TextBox)ctl).Text = String.Empty;

                }
                if (ctl.GetType() == typeof(RibbonCheckBox))
                {
                    ((RibbonCheckBox)ctl).IsChecked = false;

                }
                if (ctl.GetType() == typeof(PasswordBox))
                {
                    ((PasswordBox)ctl).Password = String.Empty;

                }
            }
        }
        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            clear();

        }
    }
}
