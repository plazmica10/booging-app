using BookingApp.Application.UseCases;
using BookingApp.Command;
using BookingApp.Domain.IRepository;
using BookingApp.Domain.Model;
using BookingApp.DTO.Guide;
using BookingApp.WPF.View.Guide;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace BookingApp.WPF.ViewModel.Guide
{
    public class PopupSuccessViewModel: ViewModelBase
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
