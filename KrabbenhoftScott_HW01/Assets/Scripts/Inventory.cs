using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    UIHelper ui;
    
    int _treasure = 0;
    public int Treasure
    {
        get => _treasure;
        set { _treasure = value; ui.UpdateTreasure(_treasure); }
    }

    void Awake()
    {
        ui = GameObject.Find("Canvas").GetComponent<UIHelper>();
    }

    void Start()
    {
        ui.UpdateTreasure(_treasure);
    }
}
