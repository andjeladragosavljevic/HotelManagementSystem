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
    public partial class AddRoom : Window
    {


        private static readonly string connectionString = ConfigurationManager.ConnectionStrings["hci"].ConnectionString;
        MySqlConnection connection = new MySqlConnection(connectionString);
        public AddRoom()
        {
            InitializeComponent();


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

        private void submit_click(object sender, RoutedEventArgs e)
        {
            RoomsView roomsView = new RoomsView();
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
                    Phone = bPhone.IsChecked.Value,

                };

                roomsView.Rooms.Add(room);

                connection.Open();

                MySqlCommand command = new MySqlCommand("INSERT INTO SOBA (Ime, Kapacitet,TV, WiFi, Telefon, Opis, Cijena, Kategorija) VALUES (@Ime, @Kapacitet, @TV, @WiFi, @Telefon, @Opis, @Cijena, @Kategorija)", connection);
                command.Parameters.AddWithValue("@Ime", room.Name);
                command.Parameters.AddWithValue("@Kapacitet", room.MaxCapacity);
                command.Parameters.AddWithValue("@TV", room.Tv);
                command.Parameters.AddWithValue("@Wifi", room.Wifi);
                command.Parameters.AddWithValue("@Telefon", room.Phone);
                command.Parameters.AddWithValue("@Opis", room.Description);
                command.Parameters.AddWithValue("@Cijena", room.Price);
                command.Parameters.AddWithValue("@Kategorija", room.Category);
                command.ExecuteNonQuery();
                MessageBox.Show(LocalizedStrings.Instance["RoomInserted"], "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                clear();

            }
            catch (Exception ex)
            {
               // MessageBox.Show(ex.ToString());
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
        private void delete_click(object sender, RoutedEventArgs e)
        {
            clear();
        }


    }
}
