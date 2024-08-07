using MySql.Data.MySqlClient;
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

    public partial class UpdateReservation : Window
    {
        private readonly string connectionString = ConfigurationManager.ConnectionStrings["hci"].ConnectionString;
        public UpdateReservation()
        {
            InitializeComponent();
        }

        private void submit_click(object sender, RoutedEventArgs e)
        {

            MySqlConnection connection = new MySqlConnection(connectionString);


            connection.Open();
            DateTime start = DateTime.Now;
            DateTime end = DateTime.Now;

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
                catch { MessageBox.Show(LocalizedStrings.Instance["DateFormat"]); }

            
               
            }

            try
            {
                MySqlCommand command = new MySqlCommand("UPDATE  REZERVACIJA SET DatumPrijave=@DatumOd, DatumOdjave=@DatumDo, ImeSobe = @ImeSobe WHERE IdRezervacije = @IdRezervacije", connection);
                command.Parameters.AddWithValue("@DatumOd", start);
                command.Parameters.AddWithValue("@DatumDo", end);
                command.Parameters.AddWithValue("@ImeSobe", Room.Text);
                command.Parameters.AddWithValue("@IdRezervacije", Id.Text);

                command.ExecuteNonQuery();

                MessageBox.Show(LocalizedStrings.Instance["Booked"], "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                clear();
            }
            catch
            {
                MessageBox.Show(LocalizedStrings.Instance["BookedAlr"], "Information", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            finally
            {
                connection.Close();
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
