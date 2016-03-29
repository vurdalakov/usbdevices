namespace Vurdalakov
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;

    // xmlns:vurdalakov="clr-namespace:Vurdalakov"
    // <TreeView vurdalakov:SelectedItem="{Binding TreeViewSelectedItem}" />
    public class TreeViewSelectedItemBehavior : DependencyObject
    {
        public static readonly DependencyProperty SelectedItemProperty =
            DependencyProperty.Register("SelectedItem", typeof(Object), typeof(TreeViewSelectedItemBehavior), new UIPropertyMetadata(null, OnSelectedItemChanged));

        public static Object GetSelectedItem(DependencyObject dependencyObject)
        {
            return dependencyObject.GetValue(SelectedItemProperty);
        }

        public static void SetSelectedItem(DependencyObject dependencyObject, Object value)
        {
            dependencyObject.SetValue(SelectedItemProperty, value);
        }

        private static void OnSelectedItemChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var item = e.NewValue as TreeViewItem;

            if (item != null)
            {
                item.SetValue(TreeViewItem.IsSelectedProperty, true);
            }
        }

        public static readonly DependencyProperty SelectedItemChangedProperty =
            DependencyProperty.RegisterAttached("SelectedItemChanged", typeof(ICommand), typeof(TreeViewSelectedItemBehavior));

        public static ICommand GetSelectedItemChanged(DependencyObject dependencyObject)
        {
            return dependencyObject.GetValue(SelectedItemProperty) as ICommand;
        }

        public static void SetSelectedItemChanged(DependencyObject dependencyObject, ICommand value)
        {
            dependencyObject.SetValue(SelectedItemProperty, value);
        }
    }
}
