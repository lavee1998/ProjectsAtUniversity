using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using TodoList2.Desktop.Model;
using Persistence;

namespace TodoList2.Desktop.ViewModel
{
    public class TodoListViewModel : ViewModelBase
    {
        private ObservableCollection<Category> _lists;
        private ObservableCollection<Product> _items;
        private readonly ITodoListService _service;
        private Product _item;

        public ObservableCollection<Category> Lists
        {
            get => _lists;
            set
            {
                _lists = value;
                OnPropertyChanged();
            }
        }
   

        public ObservableCollection<Product> Items
        {
            get => _items;
            set
            {
                _items = value;
                OnPropertyChanged();
            }
        }
        /*
        public int _modelnumber;
        public int ModelNumber
        {
            get => _modelnumber;
            set
            {
                _modelnumber = value;
                OnPropertyChanged();
            }
        }*/

        public DelegateCommand SelectCommand { get; private set; }
        public DelegateCommand PlusCommand { get; private set; }
        public DelegateCommand MinusCommand { get; private set; }
        public DelegateCommand EnableCommand { get; private set; }

        public DelegateCommand OrdersCommand { get; private set; }

        public DelegateCommand LogOutCommand { get; private set; }
        public event EventHandler LogOutApplication;
        public event EventHandler OrdersEvent;


        public TodoListViewModel(ITodoListService service)
        {
            _service = service;
            LoadAsync();

            SelectCommand = new DelegateCommand(LoadItems);
            PlusCommand = new DelegateCommand(param => MyPlusCommand((int)param));
            MinusCommand = new DelegateCommand(param => MyMinusCommand((int)param));
            EnableCommand = new DelegateCommand(param => MyEnableCommand((int)param));
            LogOutCommand = new DelegateCommand(param => MyLogOutCommand());
            OrdersCommand = new DelegateCommand(param => MyOrdersCommand());
        }

        public void MyOrdersCommand()
        {
            if (OrdersEvent != null)
                OrdersEvent(this, EventArgs.Empty);
        }

    


        public void MyLogOutCommand()
        {
            if (LogOutApplication != null)
                LogOutApplication(this, EventArgs.Empty);
        }

        public async void LoadItems(object param)
        {
            try
            {
                Items = new ObservableCollection<Product>(await _service.LoadItemsAsync(((Category) param)._categoryid));
                OnPropertyChanged();
            }
            catch (NetworkException ex)
            {
                OnMessageApplication($"Váratlan hiba történt! ({ex.Message})");
            }
        }

        public async void MyPlusCommand(int param)
        {
            Console.WriteLine("\nHELLO\n");
            try
            {
                Console.WriteLine("\nHEL " +param  + "\n");
                Items = new ObservableCollection<Product>(await _service.PlusItemsAsync(param));
                OnPropertyChanged();
            }
            catch (NetworkException ex)
            {
                OnMessageApplication($"Váratlan hiba történt! ({ex.Message})");
            }
        }

        public async void MyMinusCommand(int param)
        {
            try
            {
                Items = new ObservableCollection<Product>(await _service.MinusItemsAsync(param));
            }
            catch (NetworkException ex)
            {
                OnMessageApplication($"Váratlan hiba történt! ({ex.Message})");
            }
        }

        public async void MyEnableCommand(int param)
        {
            try
            {
                Items = new ObservableCollection<Product>(await _service.EnableItemsAsync(param));
            }
            catch (NetworkException ex)
            {
                OnMessageApplication($"Váratlan hiba történt! ({ex.Message})");
            }
        }

        public async void LoadAsync()
        {
            try
            {
                Lists = new ObservableCollection<Category>(await _service.LoadListsAsync());
            }
            catch (NetworkException ex)
            {
                OnMessageApplication($"Váratlan hiba történt! ({ex.Message})");
            }
        }
    }
}