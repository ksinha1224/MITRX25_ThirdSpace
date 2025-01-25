using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Computer : MonoBehaviour
{
    [SerializeField] private Canvas screen;

    [SerializeField] private Popup popupPrefab;

    public void Init()
    {
        //load blank bg screen
        //load start button

        //slow alpha fade on
    }

    public void BTN_StartGame()
    {
        //fade start button off
        //first popup comes on, mild notification sound
        //mouse clicks it
        //clutter cascade begins (loop of popups -> click -> more popups)
        //ends after ? time
    }
}
