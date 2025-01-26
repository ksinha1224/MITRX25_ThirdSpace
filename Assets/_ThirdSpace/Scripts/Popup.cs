using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Popup : MonoBehaviour
{
    [field: SerializeField] public RectTransform rectTransform { get; private set; }

    [SerializeField] private RawImage profilePic;
    [SerializeField] private RawImage contentImg;
    [SerializeField] private TMP_Text contentTxt;

    [SerializeField] private TMP_Text likeCount;
    private int likes = 0;
    private float secondCounter = 0f;
    private Coroutine videoPlaybackRoutine;

    private PopupData data;

    private void Update()
    {
        if (data.popupType != PopupType.Image)
            return;

        if(secondCounter < Mathf.Floor(Time.time))
        {
            secondCounter = Mathf.Floor(Time.time);
            likes += Random.Range(100, 2000);
            likeCount.text = likes.ToString();
        }
    }

    public void Init(PopupData toSet, Vector2 screenPos)
    {
        data = toSet;

        profilePic.texture = data.profileTexture;
        contentTxt.text = data.textContent;

        switch(data.popupType)
        {
            default:
            case PopupType.Tweet:
                break;
            case PopupType.Image:
                contentImg.texture = data.imageContent;
                break;
            case PopupType.Video:
                contentImg.texture = data.videoContent[0];
                videoPlaybackRoutine = StartCoroutine(Playback());
                break;
        }

        rectTransform.localPosition = screenPos;
    }

    public void UpdatePositionBy(Vector3 diff)
    {
        rectTransform.localPosition += diff;
    }

    public void IdentifyCheck()
    {
        RectTransform folderRect = GameManager.Instance.GetFolder(data.classificationType).rectTransform;

        if(rectTransform.WorldIntersects(folderRect))
        {
            GameManager.Instance.RemovePopup(this); //prevents this from being interacted with while minimizing
            StartCoroutine(Minimize());
        }
    }

    IEnumerator Playback()
    {
        for(int i = 0; i < data.videoContent.Length; i++)
        {
            contentImg.texture = data.videoContent[i];
            yield return new WaitForSeconds(0.1f);

            if (i == data.videoContent.Length - 1)
                i = 0;
        }
    }

    IEnumerator Minimize()
    {
        float targetScale = 0.05f;

        while(rectTransform.localScale.x > targetScale)
        {
            rectTransform.localScale -= Vector3.one * 0.05f;
            yield return new WaitForSeconds(0.1f);
        }

        gameObject.SetActive(false);

        if (videoPlaybackRoutine != null)
        {
            StopCoroutine(videoPlaybackRoutine);
            videoPlaybackRoutine = null;
        }
    }
}

[System.Serializable]
public class PopupData
{
    public PopupType popupType;
    public MediaClassification classificationType;

    public Texture profileTexture;
    public string textContent;
    public Texture imageContent;
    public Texture[] videoContent;
}

public enum PopupType { Tweet, Image, Video }
public enum MediaClassification { EngagementBait, Desensitization, Misinformation, ReactRadicialization}
