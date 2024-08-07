using MaterialDesignThemes.Wpf;
using MySql.Data.MySqlClient;
using ProjekatB.Resources;
using ProjekatB.Themes;
using System;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Xml;
using System.Xml.Linq;
using WPFLocalizeExtension.Engine;

namespace ProjekatB.View
{
    public partial class HomeView : UserControl
    {
        private const string API_KEY = "f154407cc7d67ab24e381f0f45586476";
        string city;
        System.Windows.Threading.DispatcherTimer Timer = new System.Windows.Threading.DispatcherTimer();
        
        public HomeView()
        {
            InitializeComponent();


            Timer.Tick += new EventHandler(Timer_Click);

            Timer.Interval = new TimeSpan(0, 0, 1);

            Timer.Start();
            try
            {
                string externalip = new WebClient().DownloadString("https://ipv4.icanhazip.com/").TrimEnd();




                String rez = GetLocation(externalip);
                if (rez != "No internet connection")
                {
                    string[] words = rez.Split(',');
                    string[] temp = words[1].Split(":");
                    city = temp[1].Replace("\"", "");
                    string[] region = words[2].Split(":");
                    string[] country = words[3].Split(":");

                    txtLocation.Text = city + ", " + region[1].Replace("\"", "") + ", " + country[1].Replace("\"", "");
                }
            }
            catch (Exception ex)
            {
                txtLocation.Text = LocalizedStrings.Instance["NoInternetConnection"];
            }
            string temp2 = Forecast();

            if (temp2 != "No internet connection")
            {
                txtTemp.Text = temp2 + "°C";
            }
            else
            {
                txtTemp.Text = "";
            }

        }
       
        private string Forecast()
        {
            string ForecastUrl = "http://api.openweathermap.org/data/2.5/forecast?q=@LOC@&mode=xml&units=metric&APPID=" + API_KEY;

            string url = ForecastUrl.Replace("@LOC@", city);


            using (WebClient client = new WebClient())
            {

                try
                {
                    return DisplayForecast(client.DownloadString(url));
                }

                catch (Exception)
                {
                    return "No internet connection";
                }
            }

        }
   
       
        private string DisplayForecast(string xml)
        {
            XmlDocument xml_doc = new XmlDocument();
            xml_doc.LoadXml(xml);

            if (xml_doc != null)
            {
                XmlNode loc_node = xml_doc.SelectSingleNode("weatherdata/location");

                foreach (XmlNode time_node in xml_doc.SelectNodes("//time"))
                {

                    DateTime? time =
                        DateTime.Parse(time_node.Attributes["from"].Value,
                            null, DateTimeStyles.AssumeUniversal);

                    if (time_node != null)
                    {
                        XmlNode temp_node = time_node.SelectSingleNode("temperature");
                        if (temp_node != null)
                        {
                            string temp = temp_node.Attributes["value"].Value;

                            return temp;
                        }
                    }

                }
            }
            return null;
        }



        public static string GetLocation(string ip)
        {
            var res = "";
            try
            {
                WebRequest request = WebRequest.Create("http://ipinfo.io/" + ip);
                using (WebResponse response = request.GetResponse())
                using (StreamReader stream = new StreamReader(response.GetResponseStream()))
                {
                    string line;
                    while ((line = stream.ReadLine()) != null)
                    {
                        res += line;
                    }
                }
            }
            catch (Exception)
            {
                res = "No internet connection";
            }
            return res;
        }


        private void Timer_Click(object sender, EventArgs e)

        {

            DateTime d;

            d = DateTime.Now;

            txtTime.Text = d.Hour + " : " + d.Minute + " : " + d.Second;
            txtDate.Content = DateTime.Now.ToLongDateString();

        }

        private void txtDate_Click(object sender, RoutedEventArgs e)
        {
            CalendarWindow calendarWindow = new CalendarWindow();
            calendarWindow.Show();

        }

        private void logout(object sender, RoutedEventArgs e)
        {
            Window.GetWindow(this).Close();
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
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
        private void UpdateConfig(string key, string value)
        {
            var configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            configFile.AppSettings.Settings[key].Value = value;

            configFile.Save();
        }
        private static void SetSetting(string key, string value)
        {
            Configuration configuration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            configuration.AppSettings.Settings[key].Value = value;
            configuration.Save(ConfigurationSaveMode.Full, true);
            ConfigurationManager.RefreshSection("appSettings");
        }
        private void Settings_Click(object sender, RoutedEventArgs e)
        {
            string connectionString = "SERVER=localHost;Port=3306;Database=hci;UserId=root;Password=root";
            MySqlConnection connection = new MySqlConnection(connectionString);
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
        }

        private void textBox1_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
