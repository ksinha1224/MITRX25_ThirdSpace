using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Computer : MonoBehaviour
{
    [SerializeField] private Canvas screen;
    [SerializeField] private Transform popupParent;

    [SerializeField] private Popup[] popupPrefabs; //0 = tweet template, 1 = picture template, 2 = video template

    public void Init()
    {
        //load blank bg screen
        //load start button / initial popup

        //slow alpha fade on

        foreach(PopupData data in GameManager.Instance.testContent)
        {
            Vector2 randomPos = new Vector2(Random.Range(-550, 550), Random.Range(-210, 210));
            Popup toSpawn = Instantiate(popupPrefabs[(int)data.popupType], popupParent);

            toSpawn.Init(data, randomPos);

            GameManager.Instance.AddPopup(toSpawn);
        }
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
