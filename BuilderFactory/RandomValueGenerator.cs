namespace BuilderFactory;

using System;

internal static class RandomValueGenerator
{
    private static readonly Random Random = new Random();

    public static object GetRandomValue(Type type)
    {
        if (type == typeof(string))
        {
            return GetRandomGuid().ToString();
        }
        if (type == typeof(int))
        {
            return GetRandomInt();
        }
        else if (type == typeof(double))
        {
            return GetRandomDouble();
        }
        else if (type == typeof(byte))
        {
            return GetRandomByte();
        }
        else if (type == typeof(decimal))
        {
            return GetRandomDecimal();
        }
        else if (type == typeof(bool))
        {
            return GetRandomBool();
        }
        else if (type == typeof(Guid))
        {
            return GetRandomGuid();
        }

        // Default case: for types not explicitly handled, return null
        return null;
    }

    private static int GetRandomInt()
    {
        return Random.Next();
    }

    private static double GetRandomDouble()
    {
        return Random.NextDouble();
    }

    private static byte GetRandomByte()
    {
        byte[] buffer = new byte[1];
        Random.NextBytes(buffer);
        return buffer[0];
    }

    private static decimal GetRandomDecimal()
    {
        byte scale = (byte)Random.Next(29);
        bool sign = Random.Next(2) == 1;
        return new decimal(Random.Next(), Random.Next(), Random.Next(), sign, scale);
    }

    private static bool GetRandomBool()
    {
        return Random.Next(2) == 1;
    }

    private static Guid GetRandomGuid()
    {
        return Guid.NewGuid();
    }
}

