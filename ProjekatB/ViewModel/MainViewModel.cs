using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjekatB.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        public RelayCommand HomeViewCommand { get; set; }
        public HomeViewModel homeViewModel { get; set; }
        public RelayCommand RoomsViewCommand { get; set; }
        public RelayCommand CategoryViewCommand { get; set; }
        public RelayCommand GuestsRoomsViewCommand { get; set; }
        public RelayCommand ReservationsViewCommand { get; set; }
        public RoomsViewModel roomsViewModel { get; set; }
        public RelayCommand BillViewCommand { get; set; }
        public BillViewModel billViewModel { get; set; }
        public GuestsRoomsViewModel guestsRoomsViewModel { get; set; }
        public ReservationsViewModel reservationsViewModel { get; set; }
        public CategoryViewModel categoryViewModel { get; set; }
        public RelayCommand GuestsViewCommand { get; set; }
        public GuestsViewModel guestViewModel { get; set; }
        private object _currentView;
        public object CurrentView
        {
            get { return _currentView; }
            set
            {
                _currentView = value;
                onPropertyChanged();
            }
        }
        public MainViewModel()
        {
            homeViewModel = new HomeViewModel();
            roomsViewModel = new RoomsViewModel();
            billViewModel = new BillViewModel();
            categoryViewModel = new CategoryViewModel();
            guestsRoomsViewModel = new GuestsRoomsViewModel();
            reservationsViewModel = new ReservationsViewModel();
            guestViewModel = new GuestsViewModel();

            CurrentView = homeViewModel;
            HomeViewCommand = new RelayCommand(o => { CurrentView = homeViewModel; });
            RoomsViewCommand = new RelayCommand(o => { CurrentView = roomsViewModel; });
            BillViewCommand = new RelayCommand(o => { CurrentView = billViewModel; });
            CategoryViewCommand = new RelayCommand(o => { CurrentView = categoryViewModel; });
            GuestsRoomsViewCommand = new RelayCommand(o => { CurrentView = guestsRoomsViewModel; });
            ReservationsViewCommand = new RelayCommand(o => { CurrentView = reservationsViewModel; });
            GuestsViewCommand = new RelayCommand(o => { CurrentView = guestViewModel; });
        }
    }
}

