using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public Dictionary<string, int> items = new Dictionary<string, int>();

    void Start()
    {
        // test
        addItem("ไข่", 50);
        addItem("ผัก", 50);
        addItem("แครอท", 50);
        addItem("ข้าว", 50);
    }

    // เพิ่มของเข้าสต็อก
    public void addItem(string name, int amount)
    {
        if (!items.ContainsKey(name))
            items[name] = 0;
        items[name] += amount;
    }

    // เช็คว่ามีพอไหม
    public bool hasEnough(string name, int amount)
    {
        return items.ContainsKey(name) && items[name] >= amount;
    }

    // ใช้ของ (หักลบ)
    public bool useItem(string name, int amount)
    {
        if (hasEnough(name, amount))
        {
            items[name] -= amount;
            return true;
        }
        return false;
    }
}

