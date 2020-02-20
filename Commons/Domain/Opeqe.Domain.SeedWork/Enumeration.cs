using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

public abstract class Enumeration : IComparable
{
    public int Id
    {
        get;
        private set;
    }

    public string Name
    {
        get;
        private set;
    }

    public string Description
    {
        get;
        private set;
    }

    public string PersianName
    {
        get;
        private set;
    }

    protected Enumeration(int id, string name, string persianName, string description = "")
    {
        Id = id;
        if (name == null)
        {
            throw new ArgumentNullException("name");
        }
        Name = name;
        PersianName = persianName;
        Description = description;
    }

    public override string ToString()
    {
        return Name;
    }

    public static IEnumerable<T> GetAll<T>() where T : Enumeration
    {
        FieldInfo[] fields = typeof(T).GetFields(BindingFlags.DeclaredOnly | BindingFlags.Static | BindingFlags.Public);
        return (from f in fields
                select f.GetValue(null)).Cast<T>();
    }

    public override bool Equals(object obj)
    {
        Enumeration enumeration = obj as Enumeration;
        if (enumeration == null)
        {
            return false;
        }
        bool flag = GetType().Equals(obj.GetType());
        bool flag2 = Id.Equals(enumeration.Id);
        return flag && flag2;
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }

    public static int AbsoluteDifference(Enumeration firstValue, Enumeration secondValue)
    {
        return Math.Abs(firstValue.Id - secondValue.Id);
    }

    public static T FromValue<T>(int value) where T : Enumeration
    {
        return Parse(value, "value", (T item) => item.Id == value);
    }

    public static T FromDisplayName<T>(string displayName) where T : Enumeration
    {
        return Parse(displayName, "display name", (T item) => item.Name == displayName);
    }

    private static T Parse<T, K>(K value, string description, Func<T, bool> predicate) where T : Enumeration
    {
        T val = GetAll<T>().FirstOrDefault(predicate);
        if (val == null)
        {
            throw new InvalidOperationException($"'{value}' is not a valid {description} in {typeof(T)}");
        }
        return val;
    }

    public int CompareTo(object other)
    {
        return Id.CompareTo(((Enumeration)other).Id);
    }
}
