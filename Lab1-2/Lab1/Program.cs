namespace Lab1;

internal class Program
{
    private static void Main()
    {
        var customCollection = new CustomCollection<int>();

        customCollection.ItemAdded += ItemAddedHandler;
        customCollection.ItemRemoved += ItemRemovedHandler;
        customCollection.CollectionCleared += CollectionClearedHandler;

        customCollection.Add(1);
        customCollection.Add(2);
        customCollection.Add(3);

        customCollection.Remove(2);

        customCollection.Clear();

        Console.ReadLine();
    }

    private static void ItemAddedHandler(object? sender, ItemAddedEventArgs<int> e)
    {
        Console.WriteLine($"Item added: {e.Item}");
    }

    private static void ItemRemovedHandler(object? sender, ItemRemovedEventArgs<int> e)
    {
        Console.WriteLine($"Item removed: {e.Item}");
    }

    private static void CollectionClearedHandler(object? sender, CollectionClearedEventArgs e)
    {
        Console.WriteLine("Collection cleared");
    }
}
