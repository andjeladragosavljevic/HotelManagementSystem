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
    /// Interaction logic for ReservationsView.xaml
    /// </summary>
    public partial class ReservationsView : UserControl
    {
        DataTable dataTable = new DataTable();
        public List<Reservations> reservaions = new List<Reservations>();

        private static readonly string connectionString = ConfigurationManager.ConnectionStrings["hci"].ConnectionString;
        MySqlConnection connection = new MySqlConnection(connectionString);
        public ReservationsView()
        {
            InitializeComponent();
            try
            {
                connection.Open();
                MySqlCommand command = new MySqlCommand("SELECT * FROM REZERVACIJA WHERE IdRezervacije IN (select REZERVACIJA_IdRezervacije from GOST_HAS_REZERVACIJA where GOST_JMB=@GOST_JMB); ", connection);
                command.Parameters.AddWithValue("@GOST_JMB", Logged.GetLogged().Jmb);
                dataTable.Load(command.ExecuteReader());
                rv.DataContext = dataTable;
                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    Reservations reservation = new Reservations();
                    reservation.Id = Int32.Parse(dataTable.Rows[i]["IdRezervacije"].ToString());
                    reservation.DateFrom = dataTable.Rows[i]["DatumPrijave"].ToString();
                    reservation.DateTo = dataTable.Rows[i]["DatumOdjave"].ToString();
                    reservation.RoomName = dataTable.Rows[i]["ImeSobe"].ToString();
                    reservaions.Add(reservation);


                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
            finally { connection.Close(); }
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
                        ThemesController.SetTheme(ThemesController.ThemeTypes.Blue);

                        MySqlCommand command = new MySqlCommand("update osoba set Tema=@Tema WHERE JMB=@JMB; ", connection);
                        command.Parameters.AddWithValue("@JMB", Logged.GetLogged().Jmb);
                        command.Parameters.AddWithValue("@Tema", "Blue");
                        command.ExecuteNonQuery();
                        break;
                    }

                case 2:
                    {
                        ThemesController.SetTheme(ThemesController.ThemeTypes.Light);
                        MySqlCommand command = new MySqlCommand("update osoba set Tema=@Tema WHERE JMB=@JMB; ", connection);
                        command.Parameters.AddWithValue("@JMB", Logged.GetLogged().Jmb);
                        command.Parameters.AddWithValue("@Tema", "Light");
                        command.ExecuteNonQuery();
                        break;

                    }
                case 3:
                    {
                        //LocalizeDictionary.Instance.SetCurrentThreadCulture = true;
                        LocalizeDictionary.Instance.Culture = new CultureInfo("");
                        MySqlCommand command = new MySqlCommand("update osoba set Jezik=@Jezik WHERE JMB=@JMB; ", connection);
                        command.Parameters.AddWithValue("@JMB", Logged.GetLogged().Jmb);
                        command.Parameters.AddWithValue("@Jezik", "");
                        command.ExecuteNonQuery();
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
            //  Application.Current.MainWindow.Close();
            Window.GetWindow(this).Close();
            // Application.Current.MainWindow.Hide();
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
                if (tbx.Text != "")
                {

                    var filtered = reservaions.Where(x => x.RoomName.ToLower().StartsWith(tbx.Text));

                    if (filtered.Any())
                    {
                        var list = new List<Reservations>(filtered);

                        DataTable dataTable1 = ToDataTable<Reservations>(list);
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

        private void Add_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Update_Click(object sender, RoutedEventArgs e)
        {
            var selectedItem = rv.SelectedItem;
            if (selectedItem != null)
            {
                DataRowView dataRowView = rv.SelectedItem as DataRowView;
                if (dataRowView != null)
                {
                    UpdateReservation updateReservation = new UpdateReservation();
                    updateReservation.Room.Text = dataRowView["ImeSobe"].ToString();
                    updateReservation.firstName.Text = Logged.GetLogged().FirstName;
                    updateReservation.lastName.Text = Logged.GetLogged().LastName;
                    updateReservation.Jmb.Text = Logged.GetLogged().Jmb;
                    updateReservation.Id.Text = dataRowView["IdRezervacije"].ToString();
                    updateReservation.Show();
                }
            }
            else
            {
                MessageBox.Show(LocalizedStrings.Instance["Select"], "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            var selectedItem = rv.SelectedItem;

            if (selectedItem != null)
            {
                DataRowView dataRowView = rv.SelectedItem as DataRowView;
                if (dataRowView != null)
                {
                    string id = dataRowView["IdRezervacije"].ToString();
                    if (id != null)
                    {
                       
                        try
                        {
                            
                            connection.Open();
                            MySqlCommand command = new MySqlCommand("DELETE FROM REZERVACIJA WHERE IdRezervacije=@IdRezervacije", connection);
                            command.Parameters.AddWithValue("@IdRezervacije", id);
                            MySqlCommand command1 = new MySqlCommand("DELETE FROM gost_has_rezervacija WHERE REZERVACIJA_IdRezervacije=@IdRezervacije", connection);
                            command1.Parameters.AddWithValue("@IdRezervacije", id);
                            command1.ExecuteNonQuery();
                            dataTable.Load(command.ExecuteReader());
                            dataTable.Rows.Remove(dataRowView.Row);
                            dataTable.AcceptChanges();
                            MessageBox.Show(LocalizedStrings.Instance["CanceledReservation"], "", MessageBoxButton.OK, MessageBoxImage.Information);

                        }
                        catch(Exception ex)
                        {
                            MessageBox.Show(ex.ToString());
                        }
                        finally
                        {
                            connection.Close();
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show(LocalizedStrings.Instance["Select"], "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);

            }

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                connection.Open();
                MySqlCommand command = new MySqlCommand("SELECT * FROM REZERVACIJA WHERE IdRezervacije IN (select REZERVACIJA_IdRezervacije from GOST_HAS_REZERVACIJA where GOST_JMB=@GOST_JMB); ", connection);
                command.Parameters.AddWithValue("@GOST_JMB", Logged.GetLogged().Jmb);
                dataTable.Clear();
                dataTable.Load(command.ExecuteReader());

                rv.DataContext = dataTable;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                connection?.Close();
            }
        }
    }
}
