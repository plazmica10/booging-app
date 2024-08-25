using BookingApp.Command;
using System.Windows;
using System.Windows.Input;

namespace BookingApp.WPF.ViewModel.Guide
{
    public class PopupSuccessViewModel : ViewModelBase
    {

        public Window Window;

        public string Title { get; set; }
        public string Message { get; set; }

        public ICommand CloseCommand => new FunctionCommand(() => Window.Close());

        public PopupSuccessViewModel(Window window, string title, string message)
        {
            Window = window;
            Title = title;
            Message = message;
            
        }
    }
}
