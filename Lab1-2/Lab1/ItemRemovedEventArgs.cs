namespace Lab1;

public class ItemRemovedEventArgs<T> : EventArgs
{
    public T Item { get; }

    public ItemRemovedEventArgs(T item)
    {
        Item = item;
    }
}
