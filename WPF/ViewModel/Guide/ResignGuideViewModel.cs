using System;
using BookingApp.Application.UseCases;
using BookingApp.Command;
using BookingApp.Domain.Model;
using BookingApp.WPF.View.Guide;
using System.Windows;
using System.Windows.Input;
using System.Windows.Navigation;

namespace BookingApp.WPF.ViewModel.Guide
{
    public class ResignGuideViewModel : ViewModelBase
    {
        public NavigationService NavigationService;
        public User CurrentUser;

        private readonly TourManagementService _tourManagementService= new();
        public ICommand ResignCommand { get; set; }
        public ICommand GiveUpResignCommand { get; set; }
        public ResignGuideViewModel(User currentUser, NavigationService navigation)
        {
            CurrentUser = currentUser;
            NavigationService = navigation;

            ResignCommand = new FunctionCommand(ResignAction);
            GiveUpResignCommand = new FunctionCommand(GiveUpResignAction);
        }

        private void ResignAction()
        {
            _tourManagementService.ResignGuide(CurrentUser.Id);
            System.Windows.Application.Current.Shutdown();
        }

        private void GiveUpResignAction()
        {
            MainViewModel.RaiseGiveUpResignEvent();
            NavigationService.Navigate(new ToursView(CurrentUser, NavigationService));
        }
    }
}
