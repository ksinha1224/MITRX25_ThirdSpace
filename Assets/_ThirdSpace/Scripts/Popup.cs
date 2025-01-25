using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Popup : MonoBehaviour
{
    [field: SerializeField] public RectTransform rectTransform { get; private set; }
    [SerializeField] private RawImage contentImg;
    [SerializeField] private TMP_Text contentTxt;

    public void UpdatePositionBy(Vector3 diff)
    {
        rectTransform.localPosition += diff;
    }
}
