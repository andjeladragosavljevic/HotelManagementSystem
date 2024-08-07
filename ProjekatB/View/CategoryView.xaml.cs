using MySql.Data.MySqlClient;
using ProjekatB.Model;
using ProjekatB.Resources;
using ProjekatB.Themes;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Reflection;
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
using WPFLocalizeExtension.Engine;

namespace ProjekatB.View
{
    /// <summary>
    /// Interaction logic for CategoryView.xaml
    /// </summary>
    public partial class CategoryView : UserControl
    {
        List<Category> categories = new List<Category>();
        List<Category> catList = new List<Category>();
        DataTable dataTable = new DataTable();
      
        public Person loged = new Person();
        public App CurrentApplication { get; set; }

        private static readonly string connectionString = ConfigurationManager.ConnectionStrings["hci"].ConnectionString;
        MySqlConnection connection = new MySqlConnection(connectionString);
        public CategoryView()
        {
            InitializeComponent();
            
            connection.Open();
            MySqlCommand command = new MySqlCommand("SELECT * FROM KATEGORIJA", connection);
            dataTable.Load(command.ExecuteReader());
            rv.DataContext = dataTable;

           
            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                Category cat = new Category();
                cat.CategoryName = dataTable.Rows[i]["Naziv"].ToString();

                catList.Add(cat);


            }
            connection.Close();
        }

        private void btn_submit_Click(object sender, RoutedEventArgs e)
        {
            Category category = new Category()
            {
                CategoryName = nametxtBox.Text
            };
            categories.Add(category);
            
            try
            {
                
                connection.Open();
                MySqlCommand command = new MySqlCommand("INSERT INTO KATEGORIJA (Naziv) VALUES (@Ime)", connection);
                command.Parameters.AddWithValue("@Ime", category.CategoryName);
                command.ExecuteNonQuery();
                dataTable.Rows.Add(category.CategoryName);
                categories.Add(category);
                clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show(LocalizedStrings.Instance["CategoryExist"], "", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            finally
            {
                connection.Close();
            }
        }
        private List<Control> AllChildren(DependencyObject parent)
        {
            List<Control> list = new List<Control> { };
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
                    ((TextBox)ctl).Text = String.Empty;

                }
                if (ctl.GetType() == typeof(DatePicker))
                {
                    ((DatePicker)ctl).Text = String.Empty;

                }

            }
        }
        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
           clear();
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            var selectedItem = rv.SelectedItem;
            
            
         
               
                if (selectedItem != null )
                {
                    DataRowView dataRowView = rv.SelectedItem as DataRowView;
                if (dataRowView != null)
                {
                    string naziv = dataRowView["Naziv"].ToString();
                    dataTable.Rows.Remove(dataRowView.Row);
                    dataTable.AcceptChanges();
                    try
                    {
                        connection.Open();
                        MySqlCommand command = new MySqlCommand("DELETE FROM KATEGORIJA WHERE Naziv=@Naziv", connection);
                        command.Parameters.AddWithValue("@Naziv", naziv);
                        dataTable.Load(command.ExecuteReader());
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

                }
                else
                {
                    MessageBox.Show("Please select item first!", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
          
        
        

        private void Settings_Click(object sender, RoutedEventArgs e)
        {
            
            connection.Open();
            switch (int.Parse(((MenuItem)sender).Uid))
            {
                case 0:
                    {
                        MySqlCommand command = new MySqlCommand("update osoba set Tema=@Tema WHERE JMB=@JMB; ", connection);
                        command.Parameters.AddWithValue("@JMB", Logged.GetLogged().Jmb);
                        command.Parameters.AddWithValue("@Tema", "Dark");
                        command.ExecuteNonQuery();

                        ThemesController.SetTheme(ThemesController.ThemeTypes.Dark);
                        
                        break;


                    }
                case 1:
                    {
                        MySqlCommand command = new MySqlCommand("update osoba set Tema=@Tema WHERE JMB=@JMB; ", connection);
                        command.Parameters.AddWithValue("@JMB", Logged.GetLogged().Jmb);
                        command.Parameters.AddWithValue("@Tema", "Blue");
                        command.ExecuteNonQuery();

                        ThemesController.SetTheme(ThemesController.ThemeTypes.Blue);
                       
                        break;
                    }

                case 2:
                    {
                        MySqlCommand command = new MySqlCommand("update osoba set Tema=@Tema WHERE JMB=@JMB; ", connection);
                        command.Parameters.AddWithValue("@JMB", Logged.GetLogged().Jmb);
                        command.Parameters.AddWithValue("@Tema", "Light");
                        command.ExecuteNonQuery();

                        ThemesController.SetTheme(ThemesController.ThemeTypes.Light); 
                        
                        break;

                    }
                case 3:
                    {
                        MySqlCommand command = new MySqlCommand("update osoba set Jezik=@Jezik WHERE JMB=@JMB; ", connection);
                        command.Parameters.AddWithValue("@JMB", Logged.GetLogged().Jmb);
                        command.Parameters.AddWithValue("@Jezik", "");
                        command.ExecuteNonQuery();
                       

                        LocalizeDictionary.Instance.Culture = new CultureInfo("");
                       
                        break;
                    }
                case 4:
                    {
                        MySqlCommand command = new MySqlCommand("update osoba set Jezik=@Jezik WHERE JMB=@JMB; ", connection);
                        command.Parameters.AddWithValue("@JMB", Logged.GetLogged().Jmb);
                        command.Parameters.AddWithValue("@Jezik", "sr-Latn-RS");
                        command.ExecuteNonQuery();
                      

                        LocalizeDictionary.Instance.Culture = new CultureInfo("sr-Latn-RS");
                        break;
                    }


            }
            connection.Close();
            e.Handled = true;
        }

        private void Profile_Click(object sender, RoutedEventArgs e)
        {

            UpdateProfile update = new UpdateProfile(); 

            update.firstnameTxtBox.Text = Logged.GetLogged().FirstName;
            update.lastnameTxtBox.Text = Logged.GetLogged().LastName;
            update.emailTxtBox.Text = Logged.GetLogged().Email;
            update.jmbTxtBox.Text = Logged.GetLogged().Jmb;
            update.jmbTxtBox.IsEnabled = false;
            update.txtUsername.Text = Logged.GetLogged().Username;

            update.Show();

        }

        private void logout(object sender, RoutedEventArgs e)
        {
            
            Application.Current.MainWindow.Close();
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();

        }
        public static DataTable ToDataTable<T>(List<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                dataTable.Columns.Add(prop.Name);
            }
            foreach (T item in items)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {
                    
                        values[i] = Props[i].GetValue(item, null);
                    
                }
                dataTable.Rows.Add(values);
            }
            return dataTable;
        }
        private void textBox1_TextChanged(object sender, TextChangedEventArgs e)
        {
            var tbx = sender as TextBox;
            if (tbx != null)
            {
                if (!string.IsNullOrEmpty(tbx.Text))
                {

                    IEnumerable<Category> filtered = catList.Where(x => x.CategoryName.ToLower().StartsWith(tbx.Text));

                    if (filtered.Any())
                    {
                        var list = new List<Category>(filtered);

                        DataTable dataTable1 = ToDataTable<Category>(list);
                        rv.DataContext = null;
                        rv.DataContext = dataTable1;
                    }

                }
                else
                {
                    rv.DataContext = dataTable;
                }
            }
        }
    }
}
