using MySql.Data.MySqlClient;
using ProjekatB.Model;
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
    /// Interaction logic for GuestsView.xaml
    /// </summary>
    public partial class GuestsView : UserControl
    {
        List<Guests> guests = new List<Guests>();
        DataTable dataTable = new DataTable();

        private static readonly string connectionString = ConfigurationManager.ConnectionStrings["hci"].ConnectionString;
        MySqlConnection connection = new MySqlConnection(connectionString);
        public GuestsView()
        {
            InitializeComponent();
            try
            {
                connection.Open();
                MySqlCommand command = new MySqlCommand("select * from hci.OSOBA O INNER JOIN hci.GOST_HAS_REZERVACIJA H ON O.JMB = H.GOST_JMB INNER JOIN hci.REZERVACIJA R ON R.IdRezervacije = H.REZERVACIJA_IdRezervacije;", connection);

                dataTable.Load(command.ExecuteReader());
                dataTable.Constraints.Clear();
                dataTable.Columns.Remove("Password");
                dataTable.Columns.Remove("GOST_JMB");
                dataTable.Columns.Remove("REZERVACIJA_IdRezervacije");
                dataTable.Columns.Remove("Tema");
                dataTable.Columns.Remove("Jezik");
                rv.DataContext = dataTable;
                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    Guests guest = new Guests();
                    guest.FirstName = dataTable.Rows[i]["Ime"].ToString();
                    guest.LastName = dataTable.Rows[i]["Prezime"].ToString();
                    guest.Email = dataTable.Rows[i]["Email"].ToString();
                    guest.Jmb = dataTable.Rows[i]["JMB"].ToString();
                    guest.Id = Int32.Parse(dataTable.Rows[i]["IdRezervacije"].ToString());
                    guest.Username = dataTable.Rows[i]["Username"].ToString();
                    guest.DateFrom = dataTable.Rows[i]["DatumPrijave"].ToString();
                    guest.DateTo = dataTable.Rows[i]["DatumOdjave"].ToString();
                    guest.RoomName = dataTable.Rows[i]["ImeSobe"].ToString();
                    guests.Add(guest);


                }
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
                        command.Parameters.AddWithValue("@Tema", "Dark");
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

        private void textBox1_TextChanged(object sender, TextChangedEventArgs e)
        {
            var tbx = sender as TextBox;
            if (tbx != null)
            {
                if (tbx.Text != "")
                {

                    var filtered = guests.Where(x => x.FirstName.ToLower().StartsWith(tbx.Text));

                    if (filtered.Any())
                    {
                        var list = new List<Guests>(filtered);

                        DataTable dataTable1 = ToDataTable<Guests>(list);
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
