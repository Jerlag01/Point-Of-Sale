using System;
using System.Collections;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace Util.MicroMvvm
{
    public sealed class MultiSelectorBehaviours
    {
        public static readonly DependencyProperty SynchronizedSelectedItems = DependencyProperty.RegisterAttached(nameof(SynchronizedSelectedItems), typeof(IList), typeof(MultiSelectorBehaviours), new PropertyMetadata((object)null, new PropertyChangedCallback(MultiSelectorBehaviours.OnSynchronizedSelectedItemsChanged)));
        private static readonly DependencyProperty SynchronizationManagerProperty = DependencyProperty.RegisterAttached("SynchronizationManager", typeof(MultiSelectorBehaviours.SynchronizationManager), typeof(MultiSelectorBehaviours), new PropertyMetadata((PropertyChangedCallback)null));

        private MultiSelectorBehaviours()
        {
        }

        public static IList GetSynchronizedSelectedItems(DependencyObject dependencyObject)
        {
            return (IList)dependencyObject.GetValue(MultiSelectorBehaviours.SynchronizedSelectedItems);
        }

        public static void SetSynchronizedSelectedItems(DependencyObject dependencyObject, IList value)
        {
            dependencyObject.SetValue(MultiSelectorBehaviours.SynchronizedSelectedItems, (object)value);
        }

        private static MultiSelectorBehaviours.SynchronizationManager GetSynchronizationManager(
          DependencyObject dependencyObject)
        {
            return (MultiSelectorBehaviours.SynchronizationManager)dependencyObject.GetValue(MultiSelectorBehaviours.SynchronizationManagerProperty);
        }

        private static void SetSynchronizationManager(
          DependencyObject dependencyObject,
          MultiSelectorBehaviours.SynchronizationManager value)
        {
            dependencyObject.SetValue(MultiSelectorBehaviours.SynchronizationManagerProperty, (object)value);
        }

        private static void OnSynchronizedSelectedItemsChanged(
          DependencyObject dependencyObject,
          DependencyPropertyChangedEventArgs e)
        {
            if (e.OldValue != null)
            {
                MultiSelectorBehaviours.GetSynchronizationManager(dependencyObject).StopSynchronizing();
                MultiSelectorBehaviours.SetSynchronizationManager(dependencyObject, (MultiSelectorBehaviours.SynchronizationManager)null);
            }
            IList newValue = e.NewValue as IList;
            Selector selector = dependencyObject as Selector;
            if (newValue == null || selector == null)
                return;
            MultiSelectorBehaviours.SynchronizationManager synchronizationManager = MultiSelectorBehaviours.GetSynchronizationManager(dependencyObject);
            if (synchronizationManager == null)
            {
                synchronizationManager = new MultiSelectorBehaviours.SynchronizationManager(selector);
                MultiSelectorBehaviours.SetSynchronizationManager(dependencyObject, synchronizationManager);
            }
            synchronizationManager.StartSynchronizingList();
        }

        private class SynchronizationManager
        {
            private readonly Selector _multiSelector;
            private TwoListSynchronizer _synchronizer;

            internal SynchronizationManager(Selector selector)
            {
                this._multiSelector = selector;
            }

            public void StartSynchronizingList()
            {
                IList synchronizedSelectedItems = MultiSelectorBehaviours.GetSynchronizedSelectedItems((DependencyObject)this._multiSelector);
                if (synchronizedSelectedItems == null)
                    return;
                this._synchronizer = new TwoListSynchronizer(MultiSelectorBehaviours.SynchronizationManager.GetSelectedItemsCollection(this._multiSelector), synchronizedSelectedItems);
                this._synchronizer.StartSynchronizing();
            }

            public void StopSynchronizing()
            {
                this._synchronizer.StopSynchronizing();
            }

            public static IList GetSelectedItemsCollection(Selector selector)
            {
                switch (selector)
                {
                    case MultiSelector _:
                        return (selector as MultiSelector).SelectedItems;
                    case ListBox _:
                        return (selector as ListBox).SelectedItems;
                    default:
                        throw new InvalidOperationException("Target object has no SelectedItems property to bind.");
                }
            }
        }
    }
}
