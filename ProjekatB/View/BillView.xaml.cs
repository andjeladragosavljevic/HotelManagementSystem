using MaterialDesignThemes.Wpf;
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
using System.Windows.Threading;
using WPFLocalizeExtension.Engine;

namespace ProjekatB.View
{

    public partial class BillView : UserControl
    {
        string jmb;
        public App CurrentApplication { get; set; }
        private readonly PaletteHelper _paletteHelper = new PaletteHelper();
        DataTable dataTable = new DataTable();
        ResourceDictionary resourceDictionary = new ResourceDictionary();
        List<string> bill_items = new List<string>();

        private static readonly string connectionString = ConfigurationManager.ConnectionStrings["hci"].ConnectionString;
        MySqlConnection connection = new MySqlConnection(connectionString);

        public BillView()
        {

            InitializeComponent();
            
          
            connection.Open();
            try
            {
                MySqlCommand command = new MySqlCommand("select * from hci.OSOBA O INNER JOIN hci.GOST_HAS_REZERVACIJA H ON O.JMB = H.GOST_JMB INNER JOIN hci.REZERVACIJA R ON R.IdRezervacije = H.REZERVACIJA_IdRezervacije GROUP BY JMB;", connection);
                MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    string JMB = reader.GetString("JMB");
                    cmbIDList.Items.Add(JMB);
                }
               
                dataTable.Columns.Add("Room Name", typeof(string));
                dataTable.Columns.Add("Rate", typeof(string));
                dataTable.Columns.Add("Duration", typeof(string));
                dataTable.Columns.Add("Subtotal", typeof(string));
                dataGrid.DataContext = dataTable;
                ThemesController.ThemeTypes themeTypes = ThemesController.CurrentTheme;
                switch (themeTypes)
                {
                    case ThemesController.ThemeTypes.Dark:
                        cmbIDList.Foreground = new SolidColorBrush(Colors.White);
                        cmbResIDList.Foreground = new SolidColorBrush(Colors.White);
                        break;
                    case ThemesController.ThemeTypes.Light:
                        cmbIDList.Foreground = new SolidColorBrush(Colors.Black);
                        cmbResIDList.Foreground = new SolidColorBrush(Colors.Black);
                        break;
                    case ThemesController.ThemeTypes.Blue:
                        cmbIDList.Foreground = new SolidColorBrush(Colors.White);
                        cmbResIDList.Foreground = new SolidColorBrush(Colors.White);
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            { connection.Close(); }
           

        }

        private void Settings_Click(object sender, RoutedEventArgs e)
        {
           
            connection.Open();

            MainWindow main = new MainWindow();
            switch (int.Parse(((MenuItem)sender).Uid))
            {
                case 0:
                    {
                       
                        ThemesController.SetTheme(ThemesController.ThemeTypes.Dark);
                        MySqlCommand command = new MySqlCommand("update osoba set Tema=@Tema WHERE JMB=@JMB; ", connection);
                        command.Parameters.AddWithValue("@JMB", Logged.GetLogged().Jmb);
                        command.Parameters.AddWithValue("@Tema", "Dark");
                        command.ExecuteNonQuery();
                        cmbIDList.Foreground = new SolidColorBrush(Colors.White);
                        cmbResIDList.Foreground = new SolidColorBrush(Colors.White);
                        break;


                    }
                case 1:
                    {
                        ITheme theme = _paletteHelper.GetTheme();
                        theme.SetBaseTheme(Theme.Dark);
                        ThemesController.SetTheme(ThemesController.ThemeTypes.Blue);
                        MySqlCommand command = new MySqlCommand("update osoba set Tema=@Tema WHERE JMB=@JMB; ", connection);
                        command.Parameters.AddWithValue("@JMB", Logged.GetLogged().Jmb);
                        command.Parameters.AddWithValue("@Tema", "Blue");
                        command.ExecuteNonQuery();
                        cmbIDList.Foreground = new SolidColorBrush(Colors.White);
                        cmbResIDList.Foreground = new SolidColorBrush(Colors.White);
                        break;
                    }

                case 2:
                    {
                        ITheme theme = _paletteHelper.GetTheme();
                        theme.SetBaseTheme(Theme.Light);
                        ThemesController.SetTheme(ThemesController.ThemeTypes.Light);
                        MySqlCommand command = new MySqlCommand("update osoba set Tema=@Tema WHERE JMB=@JMB; ", connection);
                        command.Parameters.AddWithValue("@JMB", Logged.GetLogged().Jmb);
                        command.Parameters.AddWithValue("@Tema", "Light");
                        command.ExecuteNonQuery();
                        cmbIDList.Foreground = new SolidColorBrush(Colors.Black);
                        cmbResIDList.Foreground = new SolidColorBrush(Colors.Black);
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

        private void cmbIDList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            dataTable.Clear();
            bill_items.Clear();

            Cancel.IsEnabled = true;
            cmbIDList.IsEnabled = false;
            print_btn.IsEnabled = false;
            Total.Text = "";
            jmb = cmbIDList.SelectedItem as string;
            if (jmb != "")
            {
              
                connection.Open();
                MySqlCommand command = new MySqlCommand("select REZERVACIJA_IdRezervacije from hci.GOST_HAS_REZERVACIJA where GOST_JMB=@JMB;", connection);

                command.Parameters.AddWithValue("@JMB", jmb);
                MySqlDataReader reader = command.ExecuteReader();
                cmbResIDList.Items.Clear();
                while (reader.Read())
                {
                    cmbResIDList.Items.Add(reader.GetString(0));

                }
                reader.Close();
                MySqlCommand command1 = new MySqlCommand("select Ime, Prezime from hci.OSOBA where JMB=@JMB;", connection);
                command1.Parameters.AddWithValue("@JMB", jmb);
                MySqlDataReader reader1 = command1.ExecuteReader();
                reader1.Read();
                guestName.Text = reader1.GetString("Ime") + " " + reader1.GetString("Prezime");
                string dateTime = DateTime.Now.ToString();


            }
           connection.Close();
        }



        private void textBox1_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void btn_submit_Click(object sender, RoutedEventArgs e)
        {
            cmbResIDList.Items.Clear();
            cmbIDList.IsEnabled = true;
            print_btn.IsEnabled=true;
            Cancel.IsEnabled = false;
            double total = 0;
            string[] subtotals = dataTable.AsEnumerable().Select(s => s.Field<string>("Subtotal")).ToArray<string>();
            foreach (string t in subtotals)
            {
                
                double temp = Double.Parse(t);
                total += temp;
                
            }
            DateTime date = DateTime.Now;
            try
            {
                connection.Open();
                MySqlCommand command3 = new MySqlCommand("insert into racun (UkupanIznos, Datum, GOST_JMB, ZAPOSLENI_JMB) VALUES (@total, @datum, @GOST_JMB, @ZAPOSLENI_JMB);", connection);
                command3.Parameters.AddWithValue("@total", total);

                command3.Parameters.AddWithValue("@datum", date);
                command3.Parameters.AddWithValue("@GOST_JMB", jmb);
                command3.Parameters.AddWithValue("@ZAPOSLENI_JMB", Logged.GetLogged().Jmb);
                command3.ExecuteNonQuery();


                Total.Text = "BAM " + total.ToString();
                foreach (string bill in bill_items)
                {
                    
                    MySqlCommand command5 = new MySqlCommand("update stavka_na_racunu set IdRacuna=@@Identity where IdStavke=@Id;", connection);
                    command5.Parameters.AddWithValue("@Id", bill);
                    command5.ExecuteNonQuery();

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
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
        private void clear()
        {
            print_btn.IsEnabled = false;
            cmbIDList.IsEnabled = true;
            Total.Text = "";
            dataTable.Clear();
            List<Control> children = AllChildren(wind);

            foreach (object ctl in children)
            {
                string msg = ctl.ToString();

                if (ctl.GetType() == typeof(ComboBox))
                {
                    ((ComboBox)ctl).SelectedItem = String.Empty;

                }
              

            }
        }
        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            clear();
        }

        private void cmbIDList_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {

        }

        private void cmbResIDList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
           

            connection.Open();
            if (cmbResIDList.IsDropDownOpen)
            {
                string selected = (sender as ComboBox).SelectedItem as string;
                if (selected != "")
                {
                    MySqlCommand command = new MySqlCommand("select ImeSobe, DatumPrijave, DatumOdjave from hci.REZERVACIJA where IdRezervacije=@Id;", connection);

                    command.Parameters.AddWithValue("@Id", selected);
                    MySqlDataReader reader = command.ExecuteReader();
                    string ImeSobe = null;
                    DateTime DatumPrijave = DateTime.Now, DatumOdjave = DateTime.Now;
                    while (reader.Read())
                    {
                        ImeSobe = reader.GetString("ImeSobe");

                        DatumPrijave = DateTime.Parse(reader.GetString("DatumPrijave"));

                        DatumOdjave = DateTime.Parse(reader.GetString("DatumOdjave"));
                    }

                    string duration = (DatumOdjave - DatumPrijave).Days.ToString();

                    reader.Close();
                    MySqlCommand command1 = new MySqlCommand("select Cijena from hci.SOBA where Ime=@ImeSobe;", connection);
                    command1.Parameters.AddWithValue("@ImeSobe", ImeSobe);
                    reader = command1.ExecuteReader();
                    string cijena = null;
                    while (reader.Read())
                    {

                        cijena = reader.GetDouble("Cijena").ToString();
                    }
                   
                    reader.Close();
                  
                    string subtotal = (Double.Parse(cijena) * Double.Parse(duration)).ToString();
                    string[] row = new string[] { ImeSobe, cijena, duration, subtotal };
                    MySqlCommand command4 = new MySqlCommand("SELECT IdStavke FROM REZERVACIJA where IdRezervacije=@id ", connection);
                    command4.Parameters.AddWithValue("@id", selected);
                    string idStavke = command4.ExecuteScalar().ToString();
                  
                        if (idStavke == "")
                        {
                            MySqlCommand command2 = new MySqlCommand("INSERT INTO STAVKA_NA_RACUNU (Iznos) values (@subtotal)", connection);

                            command2.Parameters.AddWithValue("@subtotal", subtotal);


                            command2.ExecuteNonQuery();
                        
                            MySqlCommand command3 = new MySqlCommand("UPDATE REZERVACIJA set IdStavke=@@Identity where IdRezervacije=@Id", connection);
                            command3.Parameters.AddWithValue("@Id", selected);

                            command3.ExecuteNonQuery();
                            dataTable.Rows.Add(row);

                            dataGrid.DataContext = dataTable;
                        MySqlCommand cmd= new MySqlCommand("SELECT IdStavke from REZERVACIJA WHERE IdStavke=@@Identity;", connection);
                        bill_items.Add(cmd.ExecuteScalar().ToString());
               
                        

                    }
                    else
                        {
                            MessageBox.Show(LocalizedStrings.Instance["AlreadyInBill"]);
                        }
                    }
                
            }
          
            connection.Close();
        }
       
       
        private void print_Click(object sender, RoutedEventArgs e)
        {

            PrintDialog printDlg = new System.Windows.Controls.PrintDialog();



            if (printDlg.ShowDialog() == true)

            {
                Brush old = mygrid.Background;

                mygrid.Background = new SolidColorBrush(Colors.White);

                printDlg.PrintVisual(mygrid, "INVOICE");

                mygrid.Background = old;

            }
        }
    }
}
