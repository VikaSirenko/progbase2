
    public interface ISetInt
    {
        int GetCount();
        bool Add(int value);
        bool Remove(int value);
        bool Contains(int value);
        void Clear();
        void CopyTo(int []  array);
        bool IsSubsetOf(ISetInt other);
        void IntersectWith(ISetInt other);
         
    }
