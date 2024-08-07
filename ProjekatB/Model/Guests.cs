using ProjekatB.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjekatB.Model
{
    public class Guests : ViewModelBase
    {
        private string firstName;
        public string FirstName
        {
            get { return firstName; }
            set
            {
                firstName = value;
                onPropertyChanged("LastName");
            }
        }
        private string lastName;
        public string LastName
        {
            get { return lastName; }
            set
            {
                lastName = value;
                onPropertyChanged("FirstName");
            }
        }
        private string jmb;
        public string Jmb
        {
            get { return jmb; }
            set
            {
                jmb = value;
                onPropertyChanged("Jmb");
            }
        }
        private string email;
        public string Email
        {
            get { return email; }
            set
            {
                email = value;
                onPropertyChanged("Email");
            }
        }
        private string username;
        public string Username
        {
            get { return username; }
            set
            {
                username = value;
                onPropertyChanged("Username");
            }
        }
        private string _roomName;
        public string RoomName
        {
            get { return _roomName; }
            set
            {
                _roomName = value;
                onPropertyChanged("RoomName");
            }
        }
        private string _dateFrom;
        public string DateFrom
        {
            get { return _dateFrom; }
            set
            {
                _dateFrom = value;
                onPropertyChanged("DateFrom");
            }
        }
        private string _dateTo;
        public string DateTo
        {
            get { return _dateTo; }
            set
            {
                _dateTo = value;
                onPropertyChanged("DateTo");
            }
        }
        private int _id;
        public int Id
        {
            get { return _id; }
            set
            {
                _id = value;
                onPropertyChanged("Id");
            }
        }
    }
}
