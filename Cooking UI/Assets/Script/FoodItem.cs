using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class FoodItem
{
    public string name;
    public Sprite icon;
    public int stars;
    public float cookTime;

    public List<IngredientRequirement> requirements; // �ѵ�شԺ������
}