using System.Reflection;
using System.Windows;
using System.Windows.Controls;

namespace ToolboxUI.Helpers
{
    public static class PasswordBoxHelper
    {
        public static readonly DependencyProperty BoundPasswordProperty =
            DependencyProperty.RegisterAttached("BoundPassword",
                typeof(string),
                typeof(PasswordBoxHelper),
                new FrameworkPropertyMetadata(string.Empty, OnBoundPasswordChanged));

        public static string GetBoundPassword(DependencyObject dependencyObject)
        {
            var box = dependencyObject as PasswordBox;

            if (box != null)
            {
                // this funny little dance here ensures that we've hooked the
                // PasswordChanged event once, and only once.
                box.PasswordChanged -= PasswordChanged;
                box.PasswordChanged += PasswordChanged;
            }

            return (string)dependencyObject.GetValue(BoundPasswordProperty);
        }

        public static void SetBoundPassword(DependencyObject dependencyObject, string value)
        {
            if (string.Equals(value, GetBoundPassword(dependencyObject)))
            {
                return;
            }

            dependencyObject.SetValue(BoundPasswordProperty, value);
        }

        private static void OnBoundPasswordChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            var box = dependencyObject as PasswordBox;

            if (box == null)
            {
                return;
            }

            box.Password = GetBoundPassword(dependencyObject);
        }

        private static void PasswordChanged(object sender, RoutedEventArgs e)
        {
            PasswordBox password = sender as PasswordBox;

            SetBoundPassword(password, password.Password);

            // Set cursor past the last character in the password box
            password
                .GetType()
                .GetMethod("Select", BindingFlags.Instance | BindingFlags.NonPublic)
                .Invoke(password, new object[] { password.Password.Length, 0 });
        }
    }
}
