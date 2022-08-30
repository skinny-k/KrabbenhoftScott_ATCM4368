using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIHelper : MonoBehaviour
{
    public TMP_Text _health;
    public TMP_Text _treasure;

    void Awake()
    {
        _health   = transform.GetChild(0).GetComponent<TMP_Text>();
        _treasure = transform.GetChild(1).GetComponent<TMP_Text>();
    }

    public void UpdateHealth(int health)
    {
        _health.text   = "Health:      " + health;
    }

    public void UpdateTreasure(int treasure)
    {
        _treasure.text = "Treasure:    " + treasure;
    }
}
