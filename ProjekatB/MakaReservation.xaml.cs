using MySql.Data.MySqlClient;
using ProjekatB.Resources;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
using WPFLocalizeExtension.Engine;

namespace ProjekatB
{

    public partial class MakaReservation : Window
    {
        private readonly string connectionString = ConfigurationManager.ConnectionStrings["hci"].ConnectionString;
        public MakaReservation()
        {
            InitializeComponent();
        }

        private void submit_click(object sender, RoutedEventArgs e)
        {


            MySqlConnection connection = new MySqlConnection(connectionString);


            connection.Open();

            DateTime start = DateTime.Now;
            DateTime end = DateTime.Now;

            bool booked = false;

            try
            {
               start = DateTime.ParseExact(dateFrom.Text, "d.M.yyyy.", System.Globalization.CultureInfo.CurrentCulture);
               end = DateTime.ParseExact(dateTo.Text, "d.M.yyyy.", System.Globalization.CultureInfo.CurrentCulture);
            }
            catch
            {
                try
                {
                    start = DateTime.ParseExact(dateFrom.Text, "M/d/yyyy", System.Globalization.CultureInfo.CurrentCulture);
                    end = DateTime.ParseExact(dateTo.Text, "M/d/yyyy", System.Globalization.CultureInfo.CurrentCulture);
                }
                catch { MessageBox.Show(LocalizedStrings.Instance["DateFormat"]);  }

            }
            MySqlCommand command2 = new MySqlCommand("select DatumPrijave, DatumOdjave from REZERVACIJA where ImeSobe=@ImeSobe;", connection);
            command2.Parameters.AddWithValue("@ImeSobe", Room.Text) ;
            MySqlDataReader reader = command2.ExecuteReader();
            while (reader.Read())
            {
               
                DateTime datumOd = reader.GetDateTime("DatumPrijave");
                DateTime datumDo = reader.GetDateTime("DatumOdjave");
               
                if(datumDo < start && datumOd < end)
                {
                    
                    booked = false;
                }
                else
                {
                    booked=true;
                    break;
                }
            }
            reader.Close();
            if (!booked)
            {
                try
                {
                    MySqlCommand command = new MySqlCommand("INSERT INTO REZERVACIJA (DatumPrijave, DatumOdjave, ImeSobe) VALUES (@DatumOd, @DatumDo, @ImeSobe)", connection);
                    command.Parameters.AddWithValue("@DatumOd", start);
                    command.Parameters.AddWithValue("@DatumDo", end);
                    command.Parameters.AddWithValue("@ImeSobe", Room.Text);

                    command.ExecuteNonQuery();
                    try
                    {
                        MySqlCommand command1 = new MySqlCommand("insert INTO hci.gost_has_rezervacija (REZERVACIJA_IdRezervacije, GOST_JMB) select IdRezervacije, @GOST_JMB from hci.Rezervacija where IdRezervacije = @@identity", connection);
                        command1.Parameters.AddWithValue("@GOST_JMB", Logged.GetLogged().Jmb);

                        command1.ExecuteNonQuery();
                        MessageBox.Show(LocalizedStrings.Instance["Booked"], "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                        clear();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString());
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                    // MessageBox.Show(LocalizedStrings.Instance["BookedAlr"], "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                finally
                {

                    connection.Close();
                }
            }
            else
            {
                MessageBox.Show(LocalizedStrings.Instance["BookedAlr"]);
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
                if (ctl.GetType() == typeof(DatePicker))
                {
                    ((DatePicker)ctl).Text = "";

                }

            }
        }

        private void cancel_click(object sender, RoutedEventArgs e)
        {
            clear();
        }
    }
}
