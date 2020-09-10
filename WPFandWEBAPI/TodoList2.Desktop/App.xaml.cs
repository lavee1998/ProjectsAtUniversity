using System;
using System.Configuration;
using System.Windows;
using TodoList2.Desktop.Model;
using TodoList2.Desktop.View;
using TodoList2.Desktop.ViewModel;

namespace TodoList2.Desktop
{
    public partial class App : Application
    {
        private ITodoListService _service;
        private TodoListViewModel _mainViewModel;
        private OrdersViewModel _ordersViewModel;
        private LoginViewModel _loginViewModel;
        private MainWindow _view;
        private OrdersWindow _orderview;
        private LoginWindow _loginView;

        public App()
        {
            Startup += App_Startup;
        }

        private void App_Startup(object sender, StartupEventArgs e)
        {
            _service = new TodoListService(ConfigurationManager.AppSettings["baseAddress"]);

            _loginViewModel = new LoginViewModel(_service);

            _loginViewModel.ExitApplication += ViewModel_ExitApplication;
            _loginViewModel.MessageApplication += ViewModel_MessageApplication;
            _loginViewModel.LoginSuccess += ViewModel_LoginSuccess;
            _loginViewModel.LoginFailed += ViewModel_LoginFailed;

            _loginView = new LoginWindow
            {
                DataContext = _loginViewModel
            };
            _loginView.Show();
        }

        public async void App_Exit(object sender, ExitEventArgs e)
        {
            if (_service.IsUserLoggedIn)
            {
                await _service.LogoutAsync();
            }
        }

        private void ViewModel_ExitApplication(object sender, EventArgs e)
        {
            Shutdown();
        }

        private void ViewModel_LoginSuccess(object sender, EventArgs e)
        {
            _mainViewModel = new TodoListViewModel(_service);
            _mainViewModel.MessageApplication += ViewModel_MessageApplication;
            _mainViewModel.LogOutApplication += new EventHandler(ViewModel_LogOutApplication);
            _mainViewModel.OrdersEvent += new EventHandler(ViewModel_OrdersApplication);
            _view = new MainWindow
            {
                DataContext = _mainViewModel
            };

            _view.Show();
            _loginView.Close();
        }
        private void ViewModel_FromOrder(object sender, EventArgs e)
        {
            _mainViewModel = new TodoListViewModel(_service);
            _mainViewModel.MessageApplication += ViewModel_MessageApplication;
            _mainViewModel.LogOutApplication += new EventHandler(ViewModel_LogOutApplication);
            _mainViewModel.OrdersEvent += new EventHandler(ViewModel_OrdersApplication);
            _view = new MainWindow
            {
                DataContext = _mainViewModel
            };

            _view.Show();
            _orderview.Close();
        }
        private void ViewModel_OrdersApplication(object sender, System.EventArgs e)
        {
            _ordersViewModel = new OrdersViewModel(_service);
            _ordersViewModel.MessageApplication += ViewModel_MessageApplication;
            _ordersViewModel.LogOutApplication += new EventHandler(ViewModel_LogOutFromOrderApplication);
            _ordersViewModel.CategoriesEvent += new EventHandler(ViewModel_FromOrder);
            _orderview = new OrdersWindow
            {
                DataContext = _ordersViewModel
            };
            _orderview.Show();
            _view.Close();
      
        }
        private void ViewModel_LogOutApplication(object sender, System.EventArgs e)
        {
            _service = new TodoListService(ConfigurationManager.AppSettings["baseAddress"]);
            _loginViewModel = new LoginViewModel(_service);

            _loginViewModel.ExitApplication += ViewModel_ExitApplication;
            _loginViewModel.MessageApplication += ViewModel_MessageApplication;
            _loginViewModel.LoginSuccess += ViewModel_LoginSuccess;
            _loginViewModel.LoginFailed += ViewModel_LoginFailed;

            _loginView = new LoginWindow
            {
                DataContext = _loginViewModel
            };
            _loginView.Show();
            _view.Close();
          
        }

        private void ViewModel_LogOutFromOrderApplication(object sender, System.EventArgs e)
        {
            _service = new TodoListService(ConfigurationManager.AppSettings["baseAddress"]);
            _loginViewModel = new LoginViewModel(_service);

            _loginViewModel.ExitApplication += ViewModel_ExitApplication;
            _loginViewModel.MessageApplication += ViewModel_MessageApplication;
            _loginViewModel.LoginSuccess += ViewModel_LoginSuccess;
            _loginViewModel.LoginFailed += ViewModel_LoginFailed;

            _loginView = new LoginWindow
            {
                DataContext = _loginViewModel
            };
            _loginView.Show();
            _orderview.Close();

        }

        private void ViewModel_LogOutApplicationfromOrder(object sender, System.EventArgs e)
        {
            _service = new TodoListService(ConfigurationManager.AppSettings["baseAddress"]);
            _loginViewModel = new LoginViewModel(_service);

            _loginViewModel.ExitApplication += ViewModel_ExitApplication;
            _loginViewModel.MessageApplication += ViewModel_MessageApplication;
            _loginViewModel.LoginSuccess += ViewModel_LoginSuccess;
            _loginViewModel.LoginFailed += ViewModel_LoginFailed;

            _loginView = new LoginWindow
            {
                DataContext = _loginViewModel
            };
            _loginView.Show();
            _view.Close();

        }


        private void ViewModel_LoginFailed(object sender, EventArgs e)
        {
            MessageBox.Show("A bejelentkezés sikertelen!", "Bank", MessageBoxButton.OK, MessageBoxImage.Asterisk);
        }

        private void ViewModel_MessageApplication(object sender, MessageEventArgs e)
        {
            MessageBox.Show(e.Message, "Bank", MessageBoxButton.OK, MessageBoxImage.Asterisk);
        }
    }
}