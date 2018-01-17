using System.Collections.Generic;

namespace FlowNetworkToolKit.Core.Utils.ListExtensions
{
    public static class ListExtensions
    {
        //list extension moves list item at given index to the front of the list
        public static void MoveItemAtIndexToFront<T>(this List<T> list, int index)
        {
            T item = list[index];
            for (int i = index; i > 0; i--)
                list[i] = list[i - 1];
            list[0] = item;
        }
    }
}
