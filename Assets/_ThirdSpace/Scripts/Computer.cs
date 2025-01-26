using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Computer : MonoBehaviour
{
    [SerializeField] private Canvas screen;
    [SerializeField] private Transform popupParent;
    [SerializeField] private AudioSource sfx;

    [SerializeField] private AudioClip click;

    [SerializeField] private GameObject startPopup;
    [SerializeField] private Cursor cursor; //cutscene only

    [SerializeField] private RawImage bg;
    [SerializeField] private CanvasGroup warningGroup;
    [SerializeField] private CanvasGroup instructionsGroup;
    [SerializeField] private CanvasGroup tutorialGroup;
    [SerializeField] private CanvasGroup startGroup;
    [SerializeField] private GameObject endText;

    [SerializeField] private Popup[] popupPrefabs; //0 = tweet template, 1 = picture template, 2 = video template

    float timer;
    int popupIndex = 0;

    public void PlayClick()
    {
        sfx.PlayOneShot(click);
    }

    public IEnumerator FadeScreenOn()
    {
        popupParent.gameObject.SetActive(false);
        GameManager.OnGameStateChange?.Invoke(GameState.NotPlaying);

        yield return FadeGraphic(bg, true);

        yield return FadeGroup(warningGroup, true);
        yield return new WaitForSeconds(3f);

        yield return FadeGroup(warningGroup, false);
        
        StartCoroutine(FadeGroup(instructionsGroup, true));
        yield return new WaitForSeconds(3f);

        yield return FadeGroup(instructionsGroup, false);

        //nixing tutorial bc its not working idk
        //StartCoroutine(FadeGroup(tutorialGroup, true));
        //cursor.gameObject.SetActive(true);

        //GameManager.Instance.TutorialUpdate(0);
        //GameManager.OnGameStateChange?.Invoke(GameState.Tutorial);

        //while (GameManager.Instance.CurrentState == GameState.Tutorial)
        //    yield return null;

        //yield return FadeGroup(tutorialGroup, false);

        cursor.gameObject.SetActive(true);
        StartCoroutine(FadeGroup(startGroup, true));
    }

    public IEnumerator StartGame()
    {
        GameManager.OnGameStateChange?.Invoke(GameState.Cutscene);
        yield return FadeGroup(startGroup, false);

        popupParent.gameObject.SetActive(true);
        startPopup.SetActive(true);
        sfx.Play();

        Vector3 diff = startPopup.transform.position - cursor.transform.position;
        while (cursor.transform.position != startPopup.transform.position)
        {
            cursor.rectTransform.position += diff / 20;
            yield return new WaitForSeconds(0.1f);
        }

        PlayClick();

        for(int i = 0; i < 5; i++)
        {
            SpawnPopup();
            yield return new WaitForSeconds(0.5f);
        }

        startPopup.SetActive(false);
        GameManager.OnGameStateChange?.Invoke(GameState.PlayerInteraction);
    }

    public void EndGame()
    {
        popupParent.gameObject.SetActive(false);
        endText.SetActive(true);
        StartCoroutine(FadeGroup(startGroup, true));
    }

    private void Update()
    {
        if (GameManager.Instance.CurrentState == GameState.PlayerInteraction)
        {
            timer += Time.deltaTime;
            if (timer > 5f)
            {
                timer = 0f;
                SpawnPopup();
            }
        }
    }

    void SpawnPopup()
    {
        PopupData data = GameManager.Instance.popupContent[popupIndex];

        Vector2 randomPos = new Vector2(Random.Range(-550, 550), Random.Range(-210, 210));
        Popup toSpawn = Instantiate(popupPrefabs[(int)data.popupType], popupParent);

        toSpawn.Init(data, randomPos);

        GameManager.Instance.AddPopup(toSpawn);
        sfx.Play();

        popupIndex++;

        if(popupIndex == GameManager.Instance.popupContent.Count - 1)
        {
            popupIndex = 0;
        }
    }

    public IEnumerator ChangeText(TMP_Text text, string changeTo)
    {
        yield return FadeGraphic(text, false);
        text.text = changeTo;
        yield return FadeGraphic(text, true);
    }

    public IEnumerator ChangeImage(RawImage image, Texture changeTo)
    {
        yield return FadeGraphic(image, false);
        image.texture = changeTo;
        yield return FadeGraphic(image, true);
    }

    private IEnumerator FadeGroup(CanvasGroup group, bool on)
    {
        if (on)
        {
            while (group.alpha < 1)
            {
                group.alpha += 0.1f;
                yield return new WaitForSeconds(0.1f);
            }
        }
        else
        {
            while (group.alpha > 0)
            {
                
                group.alpha -= 0.1f;
                yield return new WaitForSeconds(0.1f);
            }
        }
    }

    private IEnumerator FadeGraphic(Graphic graphic, bool on)
    {
        if (on)
        {
            while (graphic.color.a< 1)
            {
                Color newColor = graphic.color;
                newColor.a += 0.1f;
                graphic.color = newColor;
                yield return new WaitForSeconds(0.1f);
            }
        }
        else
        {
            while (graphic.color.a > 0)
            {
                Color newColor = graphic.color;
                newColor.a -= 0.1f;
                graphic.color = newColor;
                yield return new WaitForSeconds(0.1f);
            }
        }
    }
}
