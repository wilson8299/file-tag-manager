using FileTagManager.Domain.Models;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace FileTagManager.WPF.AttachedProperties
{
    public class ListBoxScrollProperty : BaseAttachedProperty<ListBoxScrollProperty, bool>
    {
        private static ListBox listBox = null;
        private static ICommand command = null;
        private static bool canExecute = false;
        private static FileInfoModel prevFile = new FileInfoModel();

        public override void OnValueChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            listBox = sender as ListBox;
           
            if (listBox == null) return;

            if (e.NewValue is bool && ((bool)e.NewValue))
            {
                listBox.Loaded += new RoutedEventHandler(listBoxLoaded);
            }
            else
            {
                listBox.Loaded -= new RoutedEventHandler(listBoxLoaded);
            }
        }

        private static void listBoxLoaded(object sender, RoutedEventArgs e)
        {
            var scrollViewer = GetFirstChildOfType<ScrollViewer>(listBox);

            if (scrollViewer != null)
            {
                scrollViewer.ScrollChanged += new ScrollChangedEventHandler(scrollViewerScrollChanged);
            }
        }

        private static T GetFirstChildOfType<T>(DependencyObject dependencyObject) where T : DependencyObject
        {
            if (dependencyObject == null) return null;

            for (var i = 0; i < VisualTreeHelper.GetChildrenCount(dependencyObject); i++)
            {
                var child = VisualTreeHelper.GetChild(dependencyObject, i);
                var result = (child as T) ?? GetFirstChildOfType<T>(child);
                if (result != null) return result;
            }
            return null;
        }

        private static void scrollViewerScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            if (listBox.Items.IsEmpty) return;

            var scrollViewer = sender as ScrollViewer;

            if (prevFile != listBox.Items[0])
            {
                scrollViewer.ScrollToTop();
            }
            prevFile = listBox.Items[0] as FileInfoModel;

            if (scrollViewer.VerticalOffset < scrollViewer.ScrollableHeight)
            {
                canExecute = true;
            }

            if (scrollViewer != null && canExecute && scrollViewer.VerticalOffset == scrollViewer.ScrollableHeight)
            {
                command = ListBoxScrollCommand.GetValue(listBox);
                command.Execute(listBox);
                canExecute = false;
            }
        }
    }

    public class ListBoxScrollCommand : BaseAttachedProperty<ListBoxScrollCommand, ICommand> { }
}
