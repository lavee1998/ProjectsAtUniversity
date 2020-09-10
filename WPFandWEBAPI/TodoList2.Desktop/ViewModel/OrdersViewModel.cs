using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using TodoList2.Desktop.Model;
using Persistence;
using Persistence.DTOs;
using System.Windows.Controls;
using System.Windows;

namespace TodoList2.Desktop.ViewModel
{
    public class OrdersViewModel : ViewModelBase
    {
        private ObservableCollection<CompletedVM> _completedlists;
        private ObservableCollection<Customer> _customers;
        private ObservableCollection<Order> _orders;
        private readonly ITodoListService _service;
        private ObservableCollection<OrderDTO> _orderDTO;
        private Customer _item;

        public bool _iscompleted;

        public String SearchName { get; set; }
        public ObservableCollection<OrderDTO> OrderDTOs
        {
            get { return _orderDTO; }
            private set
            {
                if (_orderDTO != value)
                {
                    _orderDTO = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool IsCompleted
        {
            get => _iscompleted;
            set
            {
                _iscompleted = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<CompletedVM> CompletedLists
        {
            get => _completedlists;
            set
            {
                _completedlists = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Customer> Customers
        {
            get => _customers;
            set
            {
                _customers = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Order> Orders
        {
            get => _orders;
            set
            {
                _orders = value;
                OnPropertyChanged();
            }
        }


        public DelegateCommand SelectCommand { get; private set; }
      

        public DelegateCommand OrdersCommand { get; private set; }
        public DelegateCommand SearchCommand { get; private set; }
        public DelegateCommand SearchDateCommand { get; private set; }
        public DelegateCommand LogOutfromOrderCommand { get; private set; }
        public DelegateCommand FinalizationCommand { get; private set; }
        public DelegateCommand CategoriesCommand { get; private set; }
        public event EventHandler LogOutApplication;
        public event EventHandler LogOutFromOrderApplication;
        public event EventHandler OrdersEvent;
        public event EventHandler CategoriesEvent;


        public OrdersViewModel(ITodoListService service)
        {
            _service = service;
            LoadItems();
            LoadAsync();

            SelectCommand = new DelegateCommand(param => LoadProducts((CompletedVM)param));
            SearchCommand = new DelegateCommand(param => SearchOrders(param as TextBox));
            SearchDateCommand = new DelegateCommand(param => SearchDateOrders(param as DatePicker));
            FinalizationCommand = new DelegateCommand(param => FinalizationOrder((int)param));
            LogOutfromOrderCommand = new DelegateCommand(param => MyLogOutCommand());
            OrdersCommand = new DelegateCommand(param => MyOrdersCommand());
            CategoriesCommand = new DelegateCommand(param => MyCategoriesCommand());
        }

        public async void SearchDateOrders(DatePicker param)
        {
            try
            {
                OrderDTOs = new ObservableCollection<OrderDTO>(await _service.SearchDateOrdersAsync(Convert.ToDateTime(param.SelectedDate)));
                OnPropertyChanged();

            }
            catch (NetworkException ex)
            {
                OnMessageApplication($"Váratlan hiba történt! ({ex.Message})");
            }
        }

        public async void SearchOrders(TextBox param)
        {
            try
            {
                OrderDTOs = new ObservableCollection<OrderDTO>(await _service.SearchOrdersAsync((param.Text).ToString()));
                OnPropertyChanged();

            }
            catch (NetworkException ex)
            {
                OnMessageApplication($"Váratlan hiba történt! ({ex.Message})");
            }
        }

        public void MyOrdersCommand()
        {
            if (OrdersEvent != null)
                OrdersEvent(this, EventArgs.Empty);
        }
        public void MyIsItCompleted(bool b)
        {
            IsCompleted = b;
        }
        public void MyCategoriesCommand()
        {
            if(CategoriesEvent != null)
            {
                CategoriesEvent(this, EventArgs.Empty);
            }
        }

        public void MyLogOutCommand()
        {
            if (LogOutApplication != null)
                LogOutApplication(this, EventArgs.Empty);
        }
        public void MyLogOutFromOrderCommand()
        {
            if (LogOutApplication != null)
                LogOutFromOrderApplication(this, EventArgs.Empty);
        }

        public async void LoadItems()
        {
            try
            {
                OrderDTOs = new ObservableCollection<OrderDTO>(await _service.LoadOrdersAsync());

               
                OnPropertyChanged();
            }
            catch (NetworkException ex)
            {
                OnMessageApplication($"Váratlan hiba történt! ({ex.Message})");
            }
        }

        public async void LoadProducts(CompletedVM Completed)
        {
            try
            {
               OrderDTOs = new ObservableCollection<OrderDTO>(await _service.LoadProductsAsync(Completed.IsItCompleted));


                OnPropertyChanged();
            }
            catch (NetworkException ex)
            {
                OnMessageApplication($"Váratlan hiba történt! ({ex.Message})");
            }
        }

        public async void FinalizationOrder(int Id)
        {
            string messageBoxText = "Véglegesíti az ehhez a termékhez tartozó rendelést? (Lásd azonosító)";
            string caption = "Véglegesít";
            MessageBoxButton button = MessageBoxButton.YesNoCancel;
            MessageBoxImage icon = MessageBoxImage.Warning;
            MessageBoxResult result = MessageBox.Show(messageBoxText, caption, button, icon);

            switch (result)
            {
                case MessageBoxResult.Yes:
                    try
                    {
                        OrderDTOs = new ObservableCollection<OrderDTO>(await _service.FinalizationOrdersAsync(Id));


                        OnPropertyChanged();
                    }
                    catch (NetworkException ex)
                    {
                        OnMessageApplication($"Váratlan hiba történt! ({ex.Message})");
                    }
                    break;
                case MessageBoxResult.No:
            
                    break;
                case MessageBoxResult.Cancel:
                    break;
            }
          
        }
        public void LoadAsync()
        {
            try
            {
                CompletedLists = new ObservableCollection<CompletedVM>();
                CompletedLists.Add(new CompletedVM { Text = "Teljesített", IsItCompleted = true });
                CompletedLists.Add(new CompletedVM { Text = "Nem teljesített", IsItCompleted = false });

            }
            catch (NetworkException ex)
            {
                OnMessageApplication($"Váratlan hiba történt! ({ex.Message})");
            }
        }
    }
}