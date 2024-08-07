using ProjekatB.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjekatB.Model
{
    public class Reservations : ViewModelBase
    {
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
