using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjekatB.ViewModel;
namespace ProjekatB.Model
{
    public class Room : ViewModelBase
    {
        public string roomName;
        public string Name
        {
            get { return roomName; }
            set
            {
                roomName = value;
                onPropertyChanged("Name");
            }
        }
        public string description;
        public string Description
        {
            get { return description; }
            set
            {
                description = value;
                onPropertyChanged("description");
            }
        }
        public int maxCapacity;
        public int MaxCapacity
        {
            get { return maxCapacity; }
            set
            {
                maxCapacity = value;
                onPropertyChanged("maxCapacity");
            }
        }
        public string category;
        public string Category
        {
            get { return category; }
            set
            {
                category = value;
                onPropertyChanged("category");
            }
        }
        public double price;
        public double Price
        {
            get { return price; }
            set
            {
                price = value;
                onPropertyChanged("Price");
            }
        }
        public bool tv;
        public bool Tv
        {
            get { return tv; }
            set
            {
                tv = value;
                onPropertyChanged("TV");
            }
        }

        public bool wifi;
        public bool Wifi
        {
            get { return wifi; }
            set
            {
                wifi = value;
                onPropertyChanged("Wifi");
            }
        }
        public bool phone;
        public bool Phone
        {
            get { return phone; }
            set
            {
                phone = value;
                onPropertyChanged("Phone");
            }
        }

        public Room(string name, string description, int maxCapacity, string category, double price, bool tv, bool wifi, bool phone)
        {
            Name = name;
            Description = description;
            MaxCapacity = maxCapacity;
            Category = category;
            Price = price;
            Tv = tv;
            Wifi = wifi;
            Phone = phone;
        }

        public Room()
        {
        }
    }
}
