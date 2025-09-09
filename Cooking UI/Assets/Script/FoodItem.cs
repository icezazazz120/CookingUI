using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class FoodItem
{
    public string name;
    public Sprite icon;
    public int stars;

    public List<IngredientRequirement> requirements; // วัตถุดิบที่ใช้ทำ
}