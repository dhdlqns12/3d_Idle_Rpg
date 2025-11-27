using System;
using System.Numerics;
using UnityEngine;

[Serializable]
public class BigIntegerSerializable
{
    [SerializeField] private string value = "0";

    public BigInteger Value
    {
        get
        {
            if (BigInteger.TryParse(value, out BigInteger result))
                return result;
            return 0;
        }
        set
        {
            this.value = value.ToString();
        }
    }

    public BigIntegerSerializable(string value = "0")
    {
        this.value = value;
    }

    public static implicit operator BigInteger(BigIntegerSerializable b) => b.Value;
}