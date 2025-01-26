using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Computer : MonoBehaviour
{
    [SerializeField] private Canvas screen;
    [SerializeField] private Transform popupParent;

    [SerializeField] private CanvasGroup warningGroup;
    [SerializeField] private CanvasGroup tutorialGroup;
    [SerializeField] private CanvasGroup startGroup;

    [SerializeField] private TMP_Text tutorialText;
    [SerializeField] private RawImage tutorialImg;
    [SerializeField] private Texture tutorialStep1;
    [SerializeField] private Texture tutorialStep2;

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

    public IEnumerator FadeScreenOn()
    {
        yield return null;
    }

    public void BTN_StartGame()
    {
        //fade start button off
        //first popup comes on, mild notification sound
        //mouse clicks it
        //clutter cascade begins (loop of popups -> click -> more popups)
        //ends after ? time
    }

    public IEnumerator ChangeText(string changeTo)
    {
        yield return FadeGraphic(tutorialText, false);
        tutorialText.text = changeTo;
        yield return FadeGraphic(tutorialText, true);
    }

    private IEnumerator FadeGraphic(Graphic graphic, bool on)
    {
        if (on)
        {
            while (graphic.color.a < 1)
            {
                Color newColor = graphic.color;
                newColor.a += 0.1f;
                yield return new WaitForSeconds(0.1f);
            }
        }
        else
        {
            while (graphic.color.a > 0)
            {
                Color newColor = graphic.color;
                newColor.a -= 0.1f;
                yield return new WaitForSeconds(0.1f);
            }
        }
    }
}
