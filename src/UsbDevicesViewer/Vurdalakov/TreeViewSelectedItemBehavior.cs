namespace Vurdalakov
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;

    // xmlns:vurdalakov="clr-namespace:Vurdalakov"
    // <TreeView vurdalakov:TreeViewSelectedItemBehavior.SelectedItem="{Binding TreeViewSelectedItem}" />
    public class TreeViewSelectedItemBehavior : DependencyObject
    {
        public static readonly DependencyProperty AttachProperty =
            DependencyProperty.RegisterAttached("Attach", typeof(Boolean), typeof(TreeViewSelectedItemBehavior), new UIPropertyMetadata(false, OnAttachChanged));

        public static Boolean GetAttach(DependencyObject dependencyObject)
        {
            return (Boolean)dependencyObject.GetValue(SelectedItemProperty);
        }

        public static void SetAttach(DependencyObject dependencyObject, Boolean value)
        {
            dependencyObject.SetValue(SelectedItemProperty, value);
        }

        private static void OnAttachChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var treeView = sender as TreeView;

            if (treeView != null)
            {
                var attach = (Boolean)e.NewValue;

                if (attach)
                {
                    treeView.SelectedItemChanged += OnTreeViewSelectedItemChanged;
                }
                else
                {
                    treeView.SelectedItemChanged -= OnTreeViewSelectedItemChanged;
                }
            }
        }

        private static void OnTreeViewSelectedItemChanged(Object sender, RoutedPropertyChangedEventArgs<Object> e)
        {
            var treeView = sender as TreeView;

            if (treeView != null)
            {
                SetSelectedItem(sender as DependencyObject, treeView.SelectedItem);
            }
        }

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
            var treeViewItem = e.NewValue as TreeViewItem;

            if (treeViewItem != null)
            {
                treeViewItem.SetValue(TreeViewItem.IsSelectedProperty, true);
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
