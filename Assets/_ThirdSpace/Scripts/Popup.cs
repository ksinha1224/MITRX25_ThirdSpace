using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Popup : MonoBehaviour
{
    public int DepthLayer { get; private set; }
    [field: SerializeField] public RectTransform rectTransform { get; private set; }
    [SerializeField] private RawImage contentImg;
    [SerializeField] private TMP_Text contentTxt;
}
