using Godot;
using System;


public class Vals
{
    public int FrameNumber;
    public float ItemRarity;
    public int ItemValue;
    public Vals(){}
    public Vals(int frame, float rarity, int value)
    {
        FrameNumber = frame;
        ItemRarity = rarity;
        ItemValue = value;
    }
}
