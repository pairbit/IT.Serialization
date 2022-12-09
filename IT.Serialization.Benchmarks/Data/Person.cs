using System.Runtime.Serialization;

namespace IT.Serialization.Benchmarks.Data;

public class MyList<T> : List<T>, IEquatable<MyList<T>>
{
    public MyList()
    {
    }

    public MyList(int capacity) : base(capacity)
    {
    }

    public MyList(IEnumerable<T> collection) : base(collection)
    {
    }

    public override bool Equals(object? obj) => Equals(obj as MyList<T>);

    public bool Equals(MyList<T>? other) => ReferenceEquals(this, other) || other is not null && this.SequenceEqual(other);

    public override int GetHashCode()
    {
        var hash = new HashCode();

        foreach (var item in this) hash.Add(item);

        return hash.ToHashCode();
    }
}

[DataContract]
public record Person
{
    [DataMember(Order = 0)]
    public int Id { get; set; }

    [DataMember(Order = 1)]
    public string Name { get; set; }

    [DataMember(Order = 2)]
    public int Age { get; set; }

    [DataMember(Order = 3)]
    public bool IsDeleted { get; set; }

    [DataMember(Order = 4)]
    public DateTime Created { get; set; }

    [DataMember(Order = 5)]
    public Person Mother { get; set; }

    [DataMember(Order = 6)]
    public Person Father { get; set; }

    [DataMember(Order = 7)]
    public MyList<Address> Addresses { get; set; }

    [DataMember(Order = 8)]
    public MyList<Person> Childs { get; set; }
}