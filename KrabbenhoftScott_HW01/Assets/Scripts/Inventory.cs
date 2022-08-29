using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    private int _treasure = 0;
    public int Treasure
    {
        get => _treasure;
        set => _treasure = value;
    }
}
