using System.Collections.Concurrent;

namespace JPEG.Processor;

public class ArrayPool<T>
{
    private readonly ConcurrentBag<T[,]> _pool;
    private readonly int _arrayLength1;
    private readonly int _arrayLength2;

    public ArrayPool(int length1, int length2)
    {
        _pool = new ConcurrentBag<T[,]>();
        _arrayLength1 = length1;
        _arrayLength2 = length2;
    }

    public T[,] Rent()
    {
        if (_pool.TryTake(out var array))
        {
            return array;
        }
        return new T[_arrayLength1, _arrayLength2]; // Создаем новый массив, если пула нет
    }

    public void Return(T[,] array)
    {
        if (array.GetLength(0) == _arrayLength1 && array.GetLength(1) == _arrayLength2)
        {
            _pool.Add(array); // Возвращаем массив в пул
        }
    }
}
