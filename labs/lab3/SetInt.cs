using System;
public class SetInt : ISetInt
{
    private int[] _items;
    private int _size;

    public SetInt()
    {
        this._items = new int[16];
        this._size = 0;
    }

    public bool Add(int value)
    {
        bool isContains = Contains(value);

        if (isContains)
        {
            return false;
        }

        if (_size == _items.Length)
        {
            Array.Resize(ref _items, _size * 2);
        }

        _items[_size] = value;
        _size++;
        DoInsertionSort();
        return true;
    }

    public void Clear()
    {
        for (int i = 0; i < _size; i++)
        {
            _items[i] = default;
        }

        _size = 0;

    }

    public bool Contains(int value)
    {
        int index = this.DoBinarySearch(value);
        return index >= 0;
    }

    public void CopyTo(int[] array)
    {
        if (array.Length == _size)
        {
            Array.Copy(_items, array, _size);
        }
        
        else
        {
            throw new ArgumentException("The size of the array is too small");
        }
    }

    public int GetCount()
    {
        return _size;
    }

    public void IntersectWith(ISetInt other)
    {
        int[] array = new int[0];

        for (int i = 0; i < _size; i++)
        {
            if (other.Contains(_items[i]))
            {
                Array.Resize(ref array, array.Length + 1);
                array[array.Length - 1] = _items[i];
            }
        }

        this.Clear();

        for (int i = 0; i < array.Length; i++)
        {
            this.Add(array[i]);
        }
    }

    public bool IsSubsetOf(ISetInt other)
    {
        for (int i = 0; i < this._size; i++)
        {
            if (!other.Contains(_items[i]))
            {
                return false;
            }
        }

        return true;
    }

    public bool Remove(int value)
    {
        int index = this.DoBinarySearch(value);

        if (index == -1)
        {
            return false;
        }

        for (int i = index; i < _size - 1; i++)
        {
            _items[i] = _items[i + 1];
        }

        _size--;
        return true;
    }

    private void DoInsertionSort()
    {
        for (int i = 1; i < _size; i++)
        {
            int buf = _items[i];
            int j = i - 1;

            while (j >= 0 && _items[j] > buf)
            {
                _items[j + 1] = _items[j];
                j = j - 1;
            }

            _items[j + 1] = buf;
        }
    }

    private int DoBinarySearch(int value)
    {
        int low = 0;
        int high = _size - 1;

        while (low <= high)
        {
            int mid = (low + high) / 2;
            if (_items[mid] == value)
            {
                return mid;
            }

            else if (value < _items[mid])
            {
                high = mid - 1;
            }

            else
            {
                low = mid + 1;
            }

        }

        return -1;
    }

}
