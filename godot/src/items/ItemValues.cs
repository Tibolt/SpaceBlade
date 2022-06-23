using Godot;
using System;
using System.Collections.Generic;

public struct Values
{
    public int FrameNumber;
    public float ItemRarity;
    public int ItemValue;
}

public class ItemDict : Dictionary<String, Vals>
{
    public string Key;
    public int FrameNum;
    public float ItemRar;
    public int ItemVal;
    public ItemDict()
    {

    } 
    public ItemDict(string key, int frame, float rarity, int value)
    {
        Key = key;
        FrameNum = frame;
        ItemRar = rarity;
        ItemVal = value;
    }
    public void Add(string key, int frame, float rarity, int value)
    {
        Values val;
        val.FrameNumber = frame;
        val.ItemRarity = rarity;
        val.ItemValue = value;
        Vals va = new Vals(frame, rarity, value);
        Key = key;
        FrameNum = frame;
        ItemRar = rarity;
        ItemVal = value;
        // this.Add(key,va);
    }

    public string ContainsRarity(float rarity)
    {
        foreach(var x in this)
        {
            if(rarity < x.Value.ItemRarity)
            {
                GD.Print(x.Key);
                return x.Key;
            }
        }
        return null;

    }
}