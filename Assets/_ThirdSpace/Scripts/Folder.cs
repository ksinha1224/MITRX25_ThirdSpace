using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Folder : MonoBehaviour
{
    [field: SerializeField] public ScreenGrabbable grabbable { get; private set; }
    [field: SerializeField] public MediaClassification classification { get; private set; }
    [field: SerializeField] public RectTransform rectTransform { get; private set; }

    [SerializeField] private TMP_Text title;

    public void Init(MediaClassification toSet)
    {
        classification = toSet;
        title.text = classification.ToString();
    }
}
