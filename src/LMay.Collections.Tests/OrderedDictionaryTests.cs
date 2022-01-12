using AutoFixture;
using NUnit.Framework;
using System.Collections;

namespace LMay.Collections.Tests;

public class Tests
{
    [Test]
    public void Add()
    {
        var fixture = new Fixture();

        var collection = fixture.CreateMany<KeyValuePair<string, string>>(51).ToArray();

        var expected = fixture.Create<KeyValuePair<string, string>>();

        var target = new OrderedDictionary<string, string>(collection)
        {
            { expected.Key, expected.Value }
        };

        var actual = target[^1];
        var actual2 = target[expected.Key];

        Assert.AreEqual(expected, actual);
        Assert.AreEqual(expected.Value, actual2);
    }

    [Test]
    public void AddPair()
    {
        var fixture = new Fixture();

        var collection = fixture.CreateMany<KeyValuePair<string, string>>(51).ToArray();

        var item = fixture.Create<KeyValuePair<string, string>>();

        var target = new OrderedDictionary<string, string>(collection)
        {
            item
        };

        var actual = target[^1];
        var actual2 = target[item.Key];

        Assert.AreEqual(item, actual);
        Assert.AreEqual(item.Value, actual2);
    }

    [Test]
    public void Clear()
    {
        var fixture = new Fixture();

        var collection = fixture.CreateMany<KeyValuePair<string, string>>(2);

        var target = new OrderedDictionary<string, string>(collection);

        target.Clear();

        const int expected = 0;
        var actual = target.Count;

        Assert.AreEqual(expected, actual);
    }

    [Test]
    public void ConstructorCapacity()
    {
        _ = new OrderedDictionary<string, string>(5);

        Assert.Pass();
    }

    [Test]
    public void ConstructorCapacityComparer()
    {
        var comparer = StringComparer.CurrentCultureIgnoreCase;

        var target = new OrderedDictionary<string, string>(0, comparer);

        Assert.AreEqual(comparer, target.Comparer);
    }

    [Test]
    public void ConstructorCollection()
    {
        var fixture = new Fixture();

        var collection = fixture.Create<IEnumerable<KeyValuePair<string, string>>>();

        var target = new OrderedDictionary<string, string>(collection);

        Assert.IsTrue(target.SequenceEqual(collection));
    }

    [Test]
    public void ConstructorCollectionComparer()
    {
        var fixture = new Fixture();

        var collection = fixture.Create<IEnumerable<KeyValuePair<string, string>>>();

        var comparer = StringComparer.CurrentCultureIgnoreCase;

        var target = new OrderedDictionary<string, string>(collection, comparer);

        Assert.IsTrue(target.SequenceEqual(collection));
        Assert.AreEqual(comparer, target.Comparer);
    }

    [Test]
    public void ConstructorComparer()
    {
        var comparer = StringComparer.CurrentCultureIgnoreCase;

        var target = new OrderedDictionary<string, string>(comparer);

        Assert.AreEqual(comparer, target.Comparer);
    }

    [Test]
    public void ConstuctorEmpty()
    {
        _ = new OrderedDictionary<string, string>();

        Assert.Pass();
    }

    [Test]
    public void Contains([Random(0, 50, 5)] int index)
    {
        var fixture = new Fixture();

        var collection = fixture.CreateMany<KeyValuePair<string, string>>(51).ToArray();

        var item = collection[index];

        var target = new OrderedDictionary<string, string>(collection);

        const bool expected = true;

        var actual = target.Contains(item);

        Assert.AreEqual(expected, actual);
    }

    [Test]
    public void ContainsKey([Random(0, 50, 5)] int index)
    {
        var fixture = new Fixture();

        var collection = fixture.CreateMany<KeyValuePair<string, string>>(51).ToArray();

        var item = collection[index];

        var target = new OrderedDictionary<string, string>(collection);

        const bool expected = true;

        var actual = target.ContainsKey(item.Key);

        Assert.AreEqual(expected, actual);
    }

    [Test]
    public void ContainsKeyNonExisting()
    {
        var fixture = new Fixture();

        var collection = fixture.CreateMany<KeyValuePair<string, string>>(51).ToArray();

        var key = fixture.Create<string>();

        var target = new OrderedDictionary<string, string>(collection);

        const bool expected = false;

        var actual = target.ContainsKey(key);

        Assert.AreEqual(expected, actual);
    }

    [Test]
    public void ContainsNonExisting()
    {
        var fixture = new Fixture();

        var collection = fixture.CreateMany<KeyValuePair<string, string>>(51).ToArray();

        var item = fixture.Create<KeyValuePair<string, string>>();

        var target = new OrderedDictionary<string, string>(collection);

        const bool expected = false;

        var actual = target.Contains(item);

        Assert.AreEqual(expected, actual);
    }

    [Test]
    public void CopyTo([Random(0, 10, 5)] int index)
    {
        var fixture = new Fixture();

        var collection = fixture.CreateMany<KeyValuePair<string, string>>(20).ToArray();

        var array = new KeyValuePair<string, string>[50];

        var target = new OrderedDictionary<string, string>(collection);

        target.CopyTo(array, index);

        var expected = collection;

        var actual = array.Skip(index).Take(20);

        Assert.IsTrue(actual.SequenceEqual(expected));
    }

    [Test]
    public void Count([Random(10, 100, 5)] int itemCount)
    {
        var fixture = new Fixture();

        var collection = fixture.CreateMany<KeyValuePair<string, string>>(itemCount);

        var target = new OrderedDictionary<string, string>(collection);

        Assert.AreEqual(itemCount, target.Count);
    }

    [Test]
    public void GetEnumerator()
    {
        var fixture = new Fixture();

        var collection = fixture.CreateMany<KeyValuePair<string, string>>(51).ToArray();

        var target = new OrderedDictionary<string, string>(collection);

        var index = 0;

        var enumerator = (target as IEnumerable)!.GetEnumerator();

        while (enumerator.MoveNext())
        {
            Assert.AreEqual(collection[index], enumerator.Current);
            index++;
        }
    }

    [Test]
    public void GetEnumeratorT()
    {
        var fixture = new Fixture();

        var collection = fixture.CreateMany<KeyValuePair<string, string>>(51).ToArray();

        var target = new OrderedDictionary<string, string>(collection);

        var index = 0;

        var enumerator = target.GetEnumerator();

        while (enumerator.MoveNext())
        {
            Assert.AreEqual(collection[index], enumerator.Current);
            index++;
        }
    }

    [Test]
    public void IndexGet([Random(0, 50, 5)] int index)
    {
        var fixture = new Fixture();

        var collection = fixture.CreateMany<KeyValuePair<string, string>>(51).ToArray();

        var target = new OrderedDictionary<string, string>(collection);

        var expected = collection[index];

        var actual = target[index];

        Assert.AreEqual(expected, actual);
    }

    [Test]
    public void IndexOf([Random(0, 50, 5)] int index)
    {
        var fixture = new Fixture();

        var collection = fixture.CreateMany<KeyValuePair<string, string>>(51).ToArray();

        var target = new OrderedDictionary<string, string>(collection);

        var item = collection[index];

        var expected = index;

        var actual = target.IndexOf(item);

        Assert.AreEqual(expected, actual);
    }

    [Test]
    public void IndexOfKey([Random(0, 50, 5)] int index)
    {
        var fixture = new Fixture();

        var collection = fixture.CreateMany<KeyValuePair<string, string>>(51).ToArray();

        var target = new OrderedDictionary<string, string>(collection);

        var item = collection[index];

        var expected = index;

        var actual = target.IndexOfKey(item.Key);

        Assert.AreEqual(expected, actual);
    }

    [Test]
    public void IndexSet([Random(0, 50, 5)] int index)
    {
        var fixture = new Fixture();

        var collection = fixture.CreateMany<KeyValuePair<string, string>>(51).ToArray();

        var expected = fixture.Create<KeyValuePair<string, string>>();

        var target = new OrderedDictionary<string, string>(collection)
        {
            [index] = expected
        };

        var actual = target[index];

        Assert.AreEqual(expected, actual);
    }

    [Test]
    public void Insert([Random(0, 50, 5)] int index)
    {
        var fixture = new Fixture();

        var collection = fixture.CreateMany<KeyValuePair<string, string>>(51).ToArray();

        var expected = fixture.Create<KeyValuePair<string, string>>();

        var target = new OrderedDictionary<string, string>(collection);

        target.Insert(index, expected.Key, expected.Value);

        var actual = target[index];
        var actual2 = target[expected.Key];

        Assert.AreEqual(expected, actual);
        Assert.AreEqual(expected.Value, actual2);
    }

    [Test]
    public void InsertPair([Random(0, 50, 5)] int index)
    {
        var fixture = new Fixture();

        var collection = fixture.CreateMany<KeyValuePair<string, string>>(51).ToArray();

        var expected = fixture.Create<KeyValuePair<string, string>>();

        var target = new OrderedDictionary<string, string>(collection);

        target.Insert(index, expected);

        var actual = target[index];
        var actual2 = target[expected.Key];

        Assert.AreEqual(expected, actual);
        Assert.AreEqual(expected.Value, actual2);
    }

    [Test]
    public void KeyGet([Random(0, 50, 5)] int index)
    {
        var fixture = new Fixture();

        var collection = fixture.CreateMany<KeyValuePair<string, string>>(51).ToArray();

        var target = new OrderedDictionary<string, string>(collection);

        var item = collection[index];

        var actual = target[item.Key];
        var expected = item.Value;

        Assert.AreEqual(expected, actual);
    }

    [Test]
    public void Keys([Random(10, 100, 5)] int itemCount)
    {
        var fixture = new Fixture();

        var collection = fixture.CreateMany<KeyValuePair<string, string>>(itemCount).ToArray();

        var target = new OrderedDictionary<string, string>(collection);

        Assert.AreEqual(collection.Select(x => x.Key), target.Keys);
    }

    [Test]
    public void KeySet([Random(0, 50, 5)] int index)
    {
        var fixture = new Fixture();

        var collection = fixture.CreateMany<KeyValuePair<string, string>>(51).ToArray();

        var key = collection[index].Key;

        var expected = fixture.Create<string>();

        var target = new OrderedDictionary<string, string>(collection)
        {
            [key] = expected
        };

        var actual = target[index].Value;

        Assert.AreEqual(expected, actual);
    }

    [Test]
    public void RemoveAt([Random(0, 50, 5)] int index)
    {
        var fixture = new Fixture();

        var collection = fixture.CreateMany<KeyValuePair<string, string>>(51).ToArray();

        var expected = collection[index + 1];

        var target = new OrderedDictionary<string, string>(collection);

        target.RemoveAt(index);

        var actual = target[index];

        Assert.AreEqual(expected, actual);
    }

    [Test]
    public void RemoveKey([Random(0, 50, 5)] int index)
    {
        var fixture = new Fixture();

        var collection = fixture.CreateMany<KeyValuePair<string, string>>(51).ToArray();

        var item = collection[index];

        var target = new OrderedDictionary<string, string>(collection);

        var result = target.Remove(item.Key);

        Assert.AreEqual(result, true);
        Assert.AreEqual(target.Count, 50);
        Assert.AreEqual(-1, target.IndexOfKey(item.Key));
    }

    [Test]
    public void RemoveKeyNonExisting()
    {
        var fixture = new Fixture();

        var collection = fixture.CreateMany<KeyValuePair<string, string>>(51).ToArray();

        var target = new OrderedDictionary<string, string>(collection);

        var actual = target.Remove("");

        Assert.AreEqual(false, actual);
    }

    [Test]
    public void RemovePair([Random(0, 50, 5)] int index)
    {
        var fixture = new Fixture();

        var collection = fixture.CreateMany<KeyValuePair<string, string>>(51).ToArray();

        var item = collection[index];

        var target = new OrderedDictionary<string, string>(collection);

        var result = target.Remove(item);

        Assert.AreEqual(result, true);
        Assert.AreEqual(target.Count, 50);
        Assert.AreEqual(-1, target.IndexOfKey(item.Key));
    }

    [Test]
    public void RemovePairNonExisting()
    {
        var fixture = new Fixture();

        var collection = fixture.CreateMany<KeyValuePair<string, string>>(51).ToArray();

        var item = fixture.Create<KeyValuePair<string, string>>();

        var target = new OrderedDictionary<string, string>(collection);

        const bool expected = false;

        var actual = target.Remove(item);

        Assert.AreEqual(expected, actual);
    }

    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void TryGetValue([Random(0, 50, 5)] int index)
    {
        var fixture = new Fixture();

        var collection = fixture.CreateMany<KeyValuePair<string, string>>(51).ToArray();

        var item = collection[index];

        var target = new OrderedDictionary<string, string>(collection);

        var expected = item.Value;

        var result = target.TryGetValue(item.Key, out var actual);

        Assert.IsTrue(result);
        Assert.AreEqual(expected, actual);
    }

    [Test]
    public void TryGetValueNonExisting()
    {
        var fixture = new Fixture();

        var collection = fixture.CreateMany<KeyValuePair<string, string>>(51).ToArray();

        var key = fixture.Create<string>();

        var target = new OrderedDictionary<string, string>(collection);

        var actual = target.TryGetValue(key, out var _);

        Assert.IsFalse(actual);
    }

    [Test]
    public void Values([Random(10, 100, 5)] int itemCount)
    {
        var fixture = new Fixture();

        var collection = fixture.CreateMany<KeyValuePair<string, string>>(itemCount).ToArray();

        var target = new OrderedDictionary<string, string>(collection);

        Assert.AreEqual(collection.Select(x => x.Value), target.Values);
    }
}