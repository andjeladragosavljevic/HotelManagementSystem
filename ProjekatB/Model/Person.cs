using ProjekatB.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjekatB.Model
{
    public class Person : ViewModelBase
    {
        private string theme;
        public string Theme
        {
            get { return theme; }
            set { theme = value; }
        }
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
        private string password;

        public Person()
        {
        }

        public Person(string firstName, string lastName, string jmb, string email, string username, string password)
        {
            FirstName = firstName;
            LastName = lastName;
            Jmb = jmb;
            Email = email;
            Username = username;
            Password = password;
        }

        public string Password
        {
            get { return password; }
            set
            {
                password = value;
                onPropertyChanged("Password");
            }
        }


    }
}
