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

        string name = "";
        switch(classification)
        {
            case MediaClassification.EngagementBait:
                name = "Engagement Bait";
                break;
            case MediaClassification.Desensitization:
            case MediaClassification.Misinformation:
                name = classification.ToString();
                break;
            case MediaClassification.ReactRadicialization:
                name = "Reactionary Radicalization";
                break;
        }
        title.text = name;
    }
}
