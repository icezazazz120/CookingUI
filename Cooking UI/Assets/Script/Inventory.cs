using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public Dictionary<string, int> items = new Dictionary<string, int>();

    void Start()
    {
        // test
        addItem("��", 50);
        addItem("�ѡ", 50);
        addItem("��ͷ", 50);
        addItem("����", 50);
    }

    // �����ͧ���ʵ�͡
    public void addItem(string name, int amount)
    {
        if (!items.ContainsKey(name))
            items[name] = 0;
        items[name] += amount;
    }

    // ������վ����
    public bool hasEnough(string name, int amount)
    {
        return items.ContainsKey(name) && items[name] >= amount;
    }

    // ��ͧ (�ѡź)
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

