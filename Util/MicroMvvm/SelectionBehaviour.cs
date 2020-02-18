using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace Util.MicroMvvm
{
    public class SelectionBehavior
    {
        public static DependencyProperty SelectionChangedProperty = DependencyProperty.RegisterAttached("SelectionChanged", typeof(ICommand), typeof(SelectionBehavior), (PropertyMetadata)new UIPropertyMetadata(new PropertyChangedCallback(SelectionBehavior.SelectedItemChanged)));

        public static void SetSelectionChanged(DependencyObject target, ICommand value)
        {
            target.SetValue(SelectionBehavior.SelectionChangedProperty, (object)value);
        }

        private static void SelectedItemChanged(
            DependencyObject target,
            DependencyPropertyChangedEventArgs e)
        {
            if (!(target is Selector selector))
                throw new InvalidOperationException("This behavior can be attached to Selector item only.");
            if (e.NewValue != null && e.OldValue == null)
            {
                selector.SelectionChanged += new SelectionChangedEventHandler(SelectionBehavior.SelectionChanged);
            }
            else
            {
                if (e.NewValue != null || e.OldValue == null)
                    return;
                selector.SelectionChanged -= new SelectionChangedEventHandler(SelectionBehavior.SelectionChanged);
            }
        }

        private static void SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ((ICommand)((DependencyObject)sender).GetValue(SelectionBehavior.SelectionChangedProperty)).Execute(((Selector)sender).SelectedValue);
        }
    }
}