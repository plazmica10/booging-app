using BookingApp.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;

namespace BookingApp.WPF.ViewModel.Guide
{
    public class SignOutPopupViewModel : ViewModelBase
    {
        public Window Window;

        public string Title { get; set; }
        public string Message { get; set; }

        public ICommand CloseCommand => new FunctionCommand(() => Window.Close());
        public ICommand SignOutCommand => new FunctionCommand(() => System.Windows.Application.Current.Shutdown());
        public SignOutPopupViewModel(Window window, string title, string message)
        {
            Window = window;
            Title = title;
            Message = message;
        }
    }
}
