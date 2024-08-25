using System.Windows;
using System.Windows.Input;
using BookingApp.Application.UseCases;
using BookingApp.Domain.IRepository;
using BookingApp.Domain.Model;

namespace BookingApp.WPF.View
{
    public partial class SignInForm : Window
    {
        private readonly UserService _userService = new();

        private void ResetUsers()
        {
            // delete all users
            _userService.DeleteAll();

            // save new users / hardcoded users -> if user model changes it will be easier to modify
            User[] users = new User[]
            {
                new("2",    "2", "Ivan",   "Novakovic", "ivan@gmail.com", "0635619321", UserRole.Owner,   false, 0, "1", false),
                new("1",    "1", "Ognjen", "Stancevic", "ogi@gmail.com",  "0693511191", UserRole.Guest,   false, 0, "1", false),
                new("a",    "a", "tea",    "jov",       "tea@gmail.com",  "0629847261", UserRole.Guide,   false, 0, "1", false),
                new("b",    "b", "pera",   "peric",     "pera@gmail.com", "0613945812", UserRole.Tourist, false, 0, "1", false),
            };

            foreach (User user in users) { _userService.SaveAppend(user); }
        }

        public SignInForm()
        {
            InitializeComponent();
            DataContext = this;
            txtUsername.Focus();

            //ResetUsers();
        }

        private void ClearInput()
        {
            txtUsername.Text = "";
            txtPassword.Password = "";
            txtUsername.Focus();
        }

        private void LogIn()
        {
            User? user = _userService.Login(txtUsername.Text, txtPassword.Password);
            if (user == null) { ClearInput(); return; }

            switch (user.Role)
            {
                case UserRole.Owner:   new Owner.MainView(user).Show();   Close(); break;
                case UserRole.Guest:   new Guest.MainView(user).Show();   Close(); break;
                case UserRole.Guide:   new Guide.MainView(user).Show();   Close(); break;
                case UserRole.Tourist: new Tourist.MainView(user).Show(); Close(); break;
            }

            ClearInput();
        }

        private void txtUsername_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Enter) { txtPassword.Focus(); }
        }

        private void txtPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter) { LogIn(); }
        }

        private void btnSignIn_Click(object sender, RoutedEventArgs e)
        {
            LogIn();
        }
    }
}
