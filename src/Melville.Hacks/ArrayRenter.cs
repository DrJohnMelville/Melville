using System;
using System.Buffers;

namespace Melville.Hacks;

public static class ArrayRenter
{
    public static RentedArray<T> RentHandle<T>(this ArrayPool<T> pool, int size)
    {
        var arr = pool.Rent(size);
        return new RentedArray<T>(arr, pool);
    }


    public class RentedArray<T>(T[] array, ArrayPool<T> pool) : IDisposable
    {
        public T[] Array => array;
        public void Dispose() => pool.Return(array);

        public static implicit operator T[](RentedArray<T> rentedArray) => rentedArray.Array;
        public static implicit operator Span<T>(RentedArray<T> rentedArray) => rentedArray.Array;

        public static implicit operator Memory<T>(RentedArray<T> rentedArray) =>
            rentedArray.Array.AsMemory();

        public Span<T> AsSpan(int origin, int length) => array.AsSpan(origin, length);
        public Memory<T> AsMemory(int origin, int length) => array.AsMemory(origin, length);

        public ref T this[int index] => ref array[index];
    }
}