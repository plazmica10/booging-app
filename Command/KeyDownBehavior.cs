using Microsoft.Xaml.Behaviors;
using System.Windows;
using System.Windows.Input;

namespace BookingApp.Command
{
    public class KeyDownBehavior : Behavior<UIElement>
    {
        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.Register(nameof(Command), typeof(ICommand), typeof(KeyDownBehavior), new PropertyMetadata(null));

        public ICommand Command
        {
            get => (ICommand)GetValue(CommandProperty);
            set => SetValue(CommandProperty, value);
        }

        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.PreviewKeyDown += OnPreviewKeyDown;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            AssociatedObject.PreviewKeyDown -= OnPreviewKeyDown;
        }

        private void OnPreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (Command?.CanExecute(e) == true)
            {
                Command.Execute(e);
            }
        }
    }
}
