using System.Collections.Generic;
using System.Linq;

namespace AspNetCoreLocalLog.Data
{
  public class CircularBuffer<T>
  {
    private readonly T[] items;
    private bool bufferIsFull = false;
    private int nextPosition = 0;

    public CircularBuffer(int length)
    {
      items = new T[length];
    }

    public IEnumerable<T> All() =>
      bufferIsFull ? 
        EnumerateLastPortionOfItemsFirst() : // full buffers start at the next position
        TrimItemsToFilledSize(); // nonfull buffers always start at 0

    private T[] TrimItemsToFilledSize() => items[..nextPosition];

    private IEnumerable<T> EnumerateLastPortionOfItemsFirst() => 
      items[nextPosition..].Concat(items[..nextPosition]);

    public void Push(T item)
    {
      StoreItemInNextPosition(item);
      IncrementNextPosition();
    }

    private void StoreItemInNextPosition(T item) => items[nextPosition] = item;

    private void IncrementNextPosition()
    {
      nextPosition++;
      if (nextPosition == items.Length)
      {
        nextPosition = 0;
        bufferIsFull = true;
      }
    }
    
    public void Clear()
    {
      nextPosition = 0;
      bufferIsFull = false;
    }
  }
}