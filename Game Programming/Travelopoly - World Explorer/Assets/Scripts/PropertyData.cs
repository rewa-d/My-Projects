using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]  
public class PropertyData
{
    public int Id;
    public string Name;
    public float Cost;
    public string ColorCategory;
    public int Position;

    public PropertyData() { }

    public PropertyData(int id, string name, float cost, string colorCategory, int position)
    {
        Id = id;
        Name = name;
        Cost = cost;
        ColorCategory = colorCategory;
        Position = position;
    }
}
