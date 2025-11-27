using System;

[Serializable]
public class Inventory
{
    public ItemSO itemData;
    public int count;

    public Inventory(ItemSO data, int amount)
    {
        itemData = data;
        count = amount;
    }
}