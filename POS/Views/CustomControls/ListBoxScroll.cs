using System.Windows.Controls;

namespace Pos.Views.CustomControls
{
    internal class ListBoxScroll : ListBox
    {
        public ListBoxScroll()
        {
            this.SelectionChanged += new SelectionChangedEventHandler(this.ListBoxScroll_SelectionChanged);
        }

        private void ListBoxScroll_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.ScrollIntoView(this.SelectedItem);
        }
    }
}