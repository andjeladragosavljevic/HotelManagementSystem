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
using WPFLocalizeExtension.Engine;

namespace ProjekatB
{
    public partial class MainWindow : Window
    {
        public bool IsDarkTheme { get; set; }
        public bool IsEnglishLanguage { get; set; }
        public string username, password, JMB, firstName, lastName, email;
        ReceptionistMainWindow receptionistMainWindow1 = new ReceptionistMainWindow();
        GuestsMainWindow guestsMainWindow = new GuestsMainWindow();
        private static readonly string connectionString = ConfigurationManager.ConnectionStrings["hci"].ConnectionString;
        MySqlConnection connection = new MySqlConnection(connectionString);
        
        public static Person logged
        {
            set; get;
        }
        public MainWindow()
        {
            InitializeComponent();

            LocalizeDictionary.Instance.Culture = new CultureInfo("");
            IsEnglishLanguage = true;

        }
       
        private readonly PaletteHelper _paletteHelper = new PaletteHelper();
        private void themeToggle_Click(object sender, RoutedEventArgs e)
        {
            ITheme theme = _paletteHelper.GetTheme();
            if (IsDarkTheme = theme.GetBaseTheme() == BaseTheme.Dark)
            {
                IsDarkTheme = false;


                theme.SetBaseTheme(Theme.Light);
                ThemesController.SetTheme(ThemesController.ThemeTypes.Light);

            }
            else
            {

                IsDarkTheme = true;
                theme.SetBaseTheme(Theme.Dark);
                ThemesController.SetTheme(ThemesController.ThemeTypes.Dark);
            }
            _paletteHelper.SetTheme(theme);

        }

        private void languageToggle_Click(object sender, RoutedEventArgs e)
        {
            if (IsEnglishLanguage)
            {
                IsEnglishLanguage = false;

                LocalizeDictionary.Instance.Culture = new CultureInfo("sr-Latn-RS");
            }
            else
            {

                IsEnglishLanguage = true;
                LocalizeDictionary.Instance.Culture = new CultureInfo("");
            }
        }

        public void switchTheme(bool IsDarkTheme)
        {

            ITheme theme = _paletteHelper.GetTheme();
            if (IsDarkTheme)
            {
                IsDarkTheme = false;
                theme.SetBaseTheme(Theme.Light);


            }
            else
            {
                IsDarkTheme = true;
                theme.SetBaseTheme(Theme.Dark);

            }
            _paletteHelper.SetTheme(theme);
        }
        private void btn_exit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            DragMove();
        }
       
        private void loginBtn_Click(object sender, RoutedEventArgs e)
        {
            string naziv = txtUserName.Text;
            string lozinka = txtPassword.Password.ToString();
            bool isLogin = false;
            try
            {
                connection.Open();
                MySqlCommand command = new MySqlCommand("SELECT * FROM OSOBA", connection);
                MySqlCommand command1 = new MySqlCommand("SELECT * FROM ZAPOSLENI", connection);
                DataTable dataTable1 = new DataTable();
                dataTable1.Load(command1.ExecuteReader());
                DataTable dataTable = new DataTable();
                dataTable.Load(command.ExecuteReader());
            
            foreach (DataRow row in dataTable.Rows)
            {
                string temp_username = row["Username"].ToString();
                string temp_password = row["Password"].ToString();
                string temp_jmb = row["JMB"].ToString();
                string ime = row["Ime"].ToString();
                string prezime = row["Prezime"].ToString();
                string email = row["Email"].ToString();
                string theme = row["Tema"].ToString();
                string jezik = row["Jezik"].ToString();
                string zaposleni = "";
                bool z = false;

                if (temp_username == naziv && temp_password == lozinka)
                {

                    LocalizeDictionary.Instance.Culture = new CultureInfo(jezik);
                    isLogin = true;
                    Person person = new Person(ime, prezime, temp_jmb, email, temp_username, temp_password);
                    Logged.SetLogged(person);


                    foreach (DataRow row2 in dataTable1.Rows)
                    {
                        zaposleni = row2["JMB"].ToString();
                        if (zaposleni == temp_jmb)
                        {
                            z = true; break;
                        }
                    }
                    if (z)
                    {
                        this.Hide();
                        
                        if (theme == "Dark")
                            ThemesController.SetTheme(ThemesController.ThemeTypes.Dark);
                        else if (theme == "Blue")
                            ThemesController.SetTheme(ThemesController.ThemeTypes.Blue);
                        else
                            ThemesController.SetTheme(ThemesController.ThemeTypes.Light);

                        receptionistMainWindow1.Show();


                    }

                    else
                    {
                        this.Hide();
                        
                        if (theme == "Dark")
                            ThemesController.SetTheme(ThemesController.ThemeTypes.Dark);
                        else if (theme == "Blue")
                            ThemesController.SetTheme(ThemesController.ThemeTypes.Blue);
                        else
                            ThemesController.SetTheme(ThemesController.ThemeTypes.Light);

                        guestsMainWindow.Show();
                    }


                }


            }
            if (!isLogin)
            {
                MessageBox.Show(LocalizedStrings.Instance["WrongInfo"]);
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


        private void signUpBtn_Click(object sender, RoutedEventArgs e)
        {
            SigneUpPage signeUpPage = new SigneUpPage();
            signeUpPage.Show();

        }
    }
}

