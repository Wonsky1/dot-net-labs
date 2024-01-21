using Lab1;

namespace Lab2;

[TestFixture]
public class CustomCollectionTests
{
    [Test]
    public void Add_ItemToCollection_CollectionContainsItem()
    {
        var collection = new CustomCollection<int> { 42 };

        Assert.That(collection, Does.Contain(42));
    }

    [Test]
    public void Remove_ItemFromCollection_CollectionDoesNotContainItem()
    {
        var collection = new CustomCollection<int> { 42 };

        collection.Remove(42);

        Assert.That(collection, Does.Not.Contain(42));
    }

    [Test]
    public void Clear_CollectionIsCleared_CollectionIsEmpty()
    {
        // Arrange
        var collection = new CustomCollection<int>
        {
            1,
            2,
            3
        };

        collection.Clear();

        Assert.That(collection, Is.Empty);
    }

    [Test]
    public void GetEnumerator_ReturnsEnumerator_EnumeratesCollection()
    {
        var collection = new CustomCollection<int>
        {
            1,
            2,
            3
        };

        using var enumerator = collection.GetEnumerator();

        Assert.Multiple(() =>
        {
            Assert.That(enumerator.MoveNext(), Is.True);
            Assert.That(enumerator.Current, Is.EqualTo(1));
        });
        Assert.Multiple(() =>
        {
            Assert.That(enumerator.MoveNext(), Is.True);
            Assert.That(enumerator.Current, Is.EqualTo(2));
        });
        Assert.Multiple(() =>
        {
            Assert.That(enumerator.MoveNext(), Is.True);
            Assert.That(enumerator.Current, Is.EqualTo(3));
        });
        Assert.That(enumerator.MoveNext(), Is.False);
    }
}
