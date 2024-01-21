using System.Collections;

namespace Lab1;

public class CustomCollection<T> : IEnumerable<T>, ICollection<T>, ICollection
{
    private Node<T>? _head;
    private Node<T>? _tail;

    public event EventHandler<ItemAddedEventArgs<T>> ItemAdded = delegate { };
    public event EventHandler<ItemRemovedEventArgs<T>> ItemRemoved = delegate { };
    public event EventHandler<CollectionClearedEventArgs> CollectionCleared = delegate { };

    public void Add(T item)
    {
        var newNode = new Node<T>(item);

        if (_head == null)
        {
            _head = newNode;
            _tail = newNode;
        }
        else
        {
            _tail!.Next = newNode;
            _tail = newNode;
        }

        ItemAdded?.Invoke(this, new ItemAddedEventArgs<T>(item));
    }

    public void CopyTo(T[] array, int arrayIndex)
    {
        var current = _head;
        while (current != null)
        {
            array[arrayIndex++] = current.Value;
            current = current.Next;
        }
    }

    public bool Remove(T item)
    {
        var current = _head;
        Node<T>? previous = null;

        while (current != null)
        {
            if (EqualityComparer<T>.Default.Equals(current.Value, item))
            {
                if (previous != null)
                {
                    previous.Next = current.Next;

                    if (current.Next == null)
                        _tail = previous;
                }
                else
                {
                    _head = current.Next;

                    if (_head == null)
                        _tail = null;
                }

                ItemRemoved?.Invoke(this, new ItemRemovedEventArgs<T>(item));
                return true;
            }

            previous = current;
            current = current.Next;
        }

        return false;
    }

    public void CopyTo(Array array, int index)
    {
        var current = _head;
        while (current != null)
        {
            array.SetValue(current.Value, index++);
            current = current.Next;
        }
    }

    public int Count
    {
        get
        {
            var current = _head;
            var count = 0;

            while (current != null)
            {
                count++;
                current = current.Next;
            }

            return count;
        }
    }

    public bool IsSynchronized { get; } = false;
    public object SyncRoot { get; } = new object();

    public bool IsReadOnly { get; } = false;

    public void Clear()
    {
        _head = null;
        _tail = null;

        CollectionCleared?.Invoke(this, new CollectionClearedEventArgs());
    }

    public bool Contains(T item)
    {
        var current = _head;

        while (current != null)
        {
            if (EqualityComparer<T>.Default.Equals(current.Value, item))
                return true;

            current = current.Next;
        }

        return false;
    }

    public IEnumerator<T> GetEnumerator()
    {
        var current = _head;
        while (current != null)
        {
            yield return current.Value;
            current = current.Next;
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    private class Node<TNode>
    {
        public TNode Value { get; }
        public Node<TNode> Next { get; set; } = null!;

        public Node(TNode value)
        {
            Value = value;
        }
    }
}
