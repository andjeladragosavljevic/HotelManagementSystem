﻿using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
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
using ProjekatB.Model;
using FastMember;
using System.Reflection;
using ProjekatB.Themes;
using WPFLocalizeExtension.Engine;
using System.Globalization;
using ProjekatB.Resources;
using System.Configuration;

namespace ProjekatB.View
{
    public partial class RoomsView : UserControl
    {
       
        List<Room> roomsList = new List<Room>();
        AddRoom addRoom = new AddRoom();
        DataTable dataTable = new DataTable();
        private  List<Room> rooms;
        public Person loged = new Person();

        private static readonly string connectionString = ConfigurationManager.ConnectionStrings["hci"].ConnectionString;
        MySqlConnection connection = new MySqlConnection(connectionString);

        public List<Room> Rooms
        {
            set { rooms = value; }
            get
            {
                if(rooms == null)
                {
                  
                    rooms = new List<Room>();
                }
                return rooms;
            }
        }
        public RoomsView()
        {

            InitializeComponent();
            try
            {
                connection.Open();
                MySqlCommand command = new MySqlCommand("SELECT * FROM SOBA", connection);

                dataTable.Load(command.ExecuteReader());
                rv.DataContext = dataTable;

                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    Room room = new Room();
                    room.Name = dataTable.Rows[i]["Ime"].ToString();
                    room.Description = dataTable.Rows[i]["Opis"].ToString();
                    room.maxCapacity = Int32.Parse(dataTable.Rows[i]["Kapacitet"].ToString());
                    room.Price = double.Parse(dataTable.Rows[i]["Cijena"].ToString());
                    room.Category = dataTable.Rows[i]["Kategorija"].ToString();
                    room.Tv = Convert.ToBoolean(dataTable.Rows[i]["TV"]);
                    room.Wifi = Convert.ToBoolean(dataTable.Rows[i]["Wifi"]);
                    room.Phone = Convert.ToBoolean(dataTable.Rows[i]["Telefon"]);
                    roomsList.Add(room);


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

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            AddRoom addRoom = new AddRoom();
            addRoom.Show();
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            var selectedItem = rv.SelectedItem;
           
            if (selectedItem != null)
            {
                DataRowView dataRowView = rv.SelectedItem as DataRowView;
                if (dataRowView != null) { 
                string naziv = dataRowView["Ime"].ToString();
                    if (naziv != null)
                    {
                        dataTable.Rows.Remove(dataRowView.Row);
                        dataTable.AcceptChanges();
                        string connectionString = "SERVER=localHost;Port=3306;Database=hci;UserId=root;Password=root";
                        MySqlConnection connection = new MySqlConnection(connectionString);
                        connection.Open();
                        MySqlCommand command = new MySqlCommand("DELETE FROM SOBA WHERE Ime=@Ime", connection);
                        command.Parameters.AddWithValue("@Ime", naziv);
                        dataTable.Load(command.ExecuteReader());
                    }
                }
            }
            else
            {
                MessageBox.Show(LocalizedStrings.Instance["Select"], "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);

            }

        }

        private void Update_Click(object sender, RoutedEventArgs e)
        {
            var selectedItem = rv.SelectedItem;
            if (selectedItem != null)
            {
                DataRowView dataRowView = rv.SelectedItem as DataRowView;
                if (dataRowView != null)
                {
                    string naziv = dataRowView["Ime"].ToString();
                    string kapacitet = dataRowView["Kapacitet"].ToString();
                    bool tv = Convert.ToBoolean(dataRowView["TV"]);
                    bool wifi = Convert.ToBoolean(dataRowView["Wifi"]);
                    bool phone = Convert.ToBoolean(dataRowView["Telefon"]);
                    string opis = dataRowView["Opis"].ToString();
                    string cijena = dataRowView["Cijena"].ToString();
                    string kategorija = dataRowView["Kategorija"].ToString();

                    UpdateRoom updateRoom = new UpdateRoom();

                    updateRoom.nametxtBox.Text = naziv;
                    updateRoom.nametxtBox.IsEnabled = false;


                    updateRoom.bTV.IsChecked = wifi;
                    updateRoom.bPhone.IsChecked = phone;
                    updateRoom.bWifi.IsChecked = wifi;
                    updateRoom.bWifi.IsChecked = wifi;
                    updateRoom.bPhone.IsChecked = phone;
                    updateRoom.descripttxtBox.Text = opis;
                    updateRoom.maxCapacitytxtBox.Text = kapacitet;
                    updateRoom.pricetxtBox.Text = cijena;
                    updateRoom.cmbCategoryList.Text = kategorija;
                    updateRoom.Show();
                }
            }
            else
            {
                MessageBox.Show(LocalizedStrings.Instance["Select"], "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {

            string connectionString = "SERVER=localHost;Port=3306;Database=hci;UserId=root;Password=root";
            MySqlConnection connection = new MySqlConnection(connectionString);
            connection.Open();
            MySqlCommand command = new MySqlCommand("SELECT * FROM SOBA", connection);
            dataTable.Load(command.ExecuteReader());
            rv.DataContext = null;
            rv.DataContext = dataTable;

        }

        private void logout(object sender, RoutedEventArgs e)
        {
            
            Application.Current.MainWindow.Close();
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
        }

        private void Profile_Click(object sender, RoutedEventArgs e)
        {
            UpdateProfile update = new UpdateProfile();

            //  MainWindow mainWindow = new MainWindow(); 

            update.firstnameTxtBox.Text = Logged.GetLogged().FirstName;
            update.lastnameTxtBox.Text = Logged.GetLogged().LastName;
            update.emailTxtBox.Text = Logged.GetLogged().Email;
            update.jmbTxtBox.Text = Logged.GetLogged().Jmb;
            update.jmbTxtBox.IsEnabled = false;
            update.txtUsername.Text = Logged.GetLogged().Username;

            update.Show();


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
            e.Handled = true;

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

                    var filtered = roomsList.Where(x => x.Name.ToLower().StartsWith(tbx.Text));

                    if (filtered.Any())
                    {
                        var list = new List<Room>(filtered);

                        DataTable dataTable1 = ToDataTable<Room>(list);
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

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}


