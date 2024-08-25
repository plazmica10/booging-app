using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows;

namespace BookingApp.Command
{
    public static class FrameNavigationBehavior
    {
        public static readonly DependencyProperty NavigatedCommandProperty =
            DependencyProperty.RegisterAttached("NavigatedCommand", typeof(ICommand), typeof(FrameNavigationBehavior), new PropertyMetadata(null, OnNavigatedCommandChanged));

        public static ICommand GetNavigatedCommand(DependencyObject obj)
        {
            return (ICommand)obj.GetValue(NavigatedCommandProperty);
        }

        public static void SetNavigatedCommand(DependencyObject obj, ICommand value)
        {
            obj.SetValue(NavigatedCommandProperty, value);
        }

        private static void OnNavigatedCommandChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var frame = d as Frame;
            if (frame == null)
                return;

            if (e.NewValue is ICommand command)
            {
                frame.Navigated += (sender, args) =>
                {
                    if (command.CanExecute(args.Content))
                    {
                        command.Execute(args.Content);
                    }
                };
            }
        }
    }
}
