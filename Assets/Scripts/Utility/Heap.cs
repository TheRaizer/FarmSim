using System;

/// <class name="Heap">
///     <summary>
///         Heap data structure. Priority Queue.
///         <remarks>
///				<para>When using CompareTo:</para>
///				<para>1 = higher priority.</para>
///				<para>0 = equal priority.</para>
///				<para>-1 = lesser priority.</para>
///         </remarks>
///     </summary>
///		<typeparam name="T">Some type that implements the IHeapItem interface.</typeparam>
/// </class>
public class Heap<T> where T : IHeapItem<T>
{
    private readonly T[] items;
    /// <summary>
    ///		Represents the size of the heap.
    /// </summary>
    private int currentItemCount;

    public Heap(int maxHeapSize)
    {
        items = new T[maxHeapSize];
    }

    public void Add(T item)
    {
        // currentItemCount is used as an index as it is already 1 above the last added items index. (It represents the size of the heap)
        item.HeapIndex = currentItemCount;
        items[currentItemCount] = item;
        SortUp(item);

        // add 1 to make it represent the size of the heap again.
        currentItemCount++;
    }

    public T RemoveFirst()
    {
        // store the highest priority item.
        T firstItem = items[0];

        // subtract before indexing so that currentItemCount becomes the index of the last item.
        currentItemCount--;

        // move the last item in the heap to be the new first item.
        items[0] = items[currentItemCount];

        // set its index to 0
        items[0].HeapIndex = 0;

        // sort the new first item down the heap.
        SortDown(items[0]);

        // currentItemCount is back to representing the size of the heap instead of the last items index
        return firstItem;
    }

    public void UpdateItem(T item)
    {
        SortUp(item);
    }

    public int Count
    {
        get
        {
            return currentItemCount;
        }
    }

    public bool Contains(T item)
    {
        return Equals(items[item.HeapIndex], item);
    }

    /// <summary>
    ///		Sorts a given item down the heap. Occurs when removing the highest priority item.
    ///		<para>
    ///			The reason for picking the higher priority child is because we want any items with higher priority closer to the top of the heap.
    ///		<para/>
    ///			By swapping the item with its higher priority child, that child gets closer to the top.
    ///		</para>
    /// </summary>
    /// <param name="item">The <see cref="IHeapItem{T}"/> item that is to be sorted down the heap.</param>
    void SortDown(T item)
    {
        while (true)
        {
            int childIndexLeft = item.HeapIndex * 2 + 1;
            int childIndexRight = item.HeapIndex * 2 + 2;
            if (childIndexLeft < currentItemCount)
            {
                int swapIndex = childIndexLeft;

                if (childIndexRight < currentItemCount)
                {
                    // if the left child has a lower priority than the right child
                    if (items[childIndexLeft].CompareTo(items[childIndexRight]) < 0)
                    {
                        swapIndex = childIndexRight;
                    }
                }

                // check if it is a valid swap in accordance to priority
                if (item.CompareTo(items[swapIndex]) < 0)
                {
                    Swap(item, items[swapIndex]);
                }
                else
                {
                    return;
                }

            }
            else
            {
                return;
            }

        }
    }

    /// <summary>
    ///		Sorts a given item up the heap. Occurs when adding an item.
    /// </summary>
    /// <param name="item">The <see cref="IHeapItem{T}"/> item that is to be sorted up the heap.</param>
    void SortUp(T item)
    {
        int parentIndex = (item.HeapIndex - 1) / 2;

        while (true)
        {
            T parentItem = items[parentIndex];

            // if the item has a higher priority than the parent swap them
            if (item.CompareTo(parentItem) > 0)
            {
                Swap(item, parentItem);
            }
            else
            {
                break;
            }

            parentIndex = (item.HeapIndex - 1) / 2;
        }
    }

    void Swap(T itemA, T itemB)
    {
        items[itemA.HeapIndex] = itemB;
        items[itemB.HeapIndex] = itemA;

        int itemAIndex = itemA.HeapIndex;

        itemA.HeapIndex = itemB.HeapIndex;
        itemB.HeapIndex = itemAIndex;
    }
}

public interface IHeapItem<T> : IComparable<T>
{
    int HeapIndex
    {
        get;
        set;
    }
}
