using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace FileTagManager.WPF.Helpers
{
    public static class ObservableCollectionHelper
    {
        public static void AddRange<T>(this ObservableCollection<T> collection, IEnumerable<T> items)
        {
            if (items == null) return;
            foreach (var item in items)
            {
                collection.Add(item);
            }
        }
    }
}
