using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FoodDatabase", menuName = "Game/Food Database")]
public class FoodDatabase : ScriptableObject
{
    public List<FoodItem> foods;
}
