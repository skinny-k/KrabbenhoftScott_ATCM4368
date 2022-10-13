using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    Button _pauseButton;
    Image _pauseMenu;

    void Awake()
    {
        _pauseButton = transform.GetChild(0).GetComponent<Button>();
        _pauseMenu = transform.GetChild(1).GetComponent<Image>();
    }
    
    void OnEnable()
    {
        GameController.OnPause += Pause;
        GameController.OnUnpause += Unpause;
    }

    void Pause()
    {
        _pauseButton.gameObject.SetActive(false);
        _pauseMenu.gameObject.SetActive(true);
    }

    void Unpause()
    {
        _pauseButton.gameObject.SetActive(true);
        _pauseMenu.gameObject.SetActive(false);
    }

    void OnDisable()
    {
        GameController.OnPause -= Pause;
        GameController.OnUnpause -= Unpause;
    }
}
