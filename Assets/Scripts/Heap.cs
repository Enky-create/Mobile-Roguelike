using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heap<T> where T: IHeapItem<T>
{
    private T[] items;
    private int currentItemCount;
    public Heap(int maxSize)
    {
        items = new T[maxSize];
        currentItemCount = 0;
    }

    public T RemoveFirstItem()
    {
        T firstItem = items[0];
        currentItemCount--;
        items[0] = items[currentItemCount];
        items[0].heapIndex = 0;
        
        SortDown(items[0]);
        return firstItem;
    }
    public void Add(T item)
    {
        item.heapIndex = currentItemCount;
        items[currentItemCount] = item;
        SortUp(item);
        currentItemCount++;
    }
    public bool Contains(T item)
    {
        if (item.heapIndex < currentItemCount)
        {
            return Equals(item, items[item.heapIndex]);
        }
        return false;
    }
    public void UpdateItem(T item)
    {
        SortUp(item);
    }
    public void Clear()
    {
        currentItemCount = 0;
    }
    public int Count
    {
        get { return currentItemCount; }
    }
    private void SortDown(T item)
    {
        while (true)
        {
            int leftChildIndex = item.heapIndex * 2 + 1;
            int rightChildIndex = leftChildIndex++;
            int swapindex = 0;
            if (leftChildIndex < currentItemCount)
            {
                swapindex = leftChildIndex;
                if(rightChildIndex< currentItemCount)
                {
                    if (items[leftChildIndex].CompareTo(items[rightChildIndex]) < 0)
                    {
                        swapindex = rightChildIndex;
                    }
                }
                if (item.CompareTo(items[swapindex]) < 0)
                {
                    Swap(item, items[swapindex]);
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
    private void SortUp(T item)
    {
        int parentIndex = (item.heapIndex - 1) / 2;
        while (true)
        {
            T parentItem = items[parentIndex];
            if (item.CompareTo(parentItem) > 0)
            {
                Swap(parentItem, item);
            }
            else
            {
                break;
            }
            parentIndex = (item.heapIndex - 1) / 2;
        }
    }
    private void Swap(T itemA, T itemB)
    {
        items[itemA.heapIndex] = itemB;
        items[itemB.heapIndex] = itemA;
        int itemBheapIndex = itemB.heapIndex;
        itemB.heapIndex = itemA.heapIndex;
        itemA.heapIndex = itemBheapIndex;
    }
}

public interface IHeapItem<T>: IComparable<T>
{
    public int heapIndex
    {
        get;set;
    }
}