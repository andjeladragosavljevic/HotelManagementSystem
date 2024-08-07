using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
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
using ProjekatB.View;
using ProjekatB.Model;
using System.Windows.Controls.Ribbon;
using ProjekatB.Resources;
using System.Configuration;

namespace ProjekatB
{
    public partial class UpdateRoom : Window
    {

        private readonly string connectionString = ConfigurationManager.ConnectionStrings["hci"].ConnectionString;
        public UpdateRoom()
        {
            InitializeComponent();

            MySqlConnection connection = new MySqlConnection(connectionString);
            connection.Open();
            MySqlCommand command = new MySqlCommand("SELECT * FROM KATEGORIJA", connection);
            MySqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                string name = reader.GetString("Naziv");
                cmbCategoryList.Items.Add(name);
            }
            reader.Close();
            connection.Close();
        }

        private void btn_update_Click(object sender, RoutedEventArgs e)
        {
            MySqlConnection connection = new MySqlConnection(connectionString);

            try
            {
                Room room = new()
                {
                    roomName = nametxtBox.Text,
                    Description = descripttxtBox.Text,
                    MaxCapacity = Int32.Parse(maxCapacitytxtBox.Text),
                    Category = cmbCategoryList.Text,
                    Price = Int32.Parse(pricetxtBox.Text),
                    Tv = bTV.IsChecked.Value,
                    Wifi = bWifi.IsChecked.Value,
                    Phone = bPhone.IsChecked.Value

                };


                RoomsView view = new RoomsView();


                connection.Open();

                MySqlCommand command = new MySqlCommand("UPDATE SOBA SET Kapacitet=@Kapacitet, TV=@TV, WiFi=@Wifi, Telefon=@Telefon, Opis=@Opis, Cijena=@Cijena, Kategorija=@KATEGORIJA_Naziv WHERE Ime=@Ime", connection);
                command.Parameters.AddWithValue("@Ime", room.Name);
                command.Parameters.AddWithValue("@Kapacitet", room.MaxCapacity);
                command.Parameters.AddWithValue("@TV", room.Tv);
                command.Parameters.AddWithValue("@Wifi", room.Wifi);
                command.Parameters.AddWithValue("@Telefon", room.Phone);
                command.Parameters.AddWithValue("@Opis", room.Description);
                command.Parameters.AddWithValue("@Cijena", room.Price);
                command.Parameters.AddWithValue("@KATEGORIJA_Naziv", room.Category);
                command.ExecuteNonQuery();
                MessageBox.Show(LocalizedStrings.Instance["UpdatedRoom"], "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                clear();

            }
            catch (Exception)
            {
                MessageBox.Show(LocalizedStrings.Instance["Required"], "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
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
                if (ctl.GetType() == typeof(RibbonCheckBox))
                {
                    ((RibbonCheckBox)ctl).IsChecked = false;

                }
                if (ctl.GetType() == typeof(ComboBox))
                {
                    ((ComboBox)ctl).Text = String.Empty;

                }
            }
        }
        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            clear();
        }
    }
}


