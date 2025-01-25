using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Folder : MonoBehaviour
{
    [field: SerializeField] public MediaClassification classification { get; private set; }
    [field: SerializeField] public RectTransform rectTransform { get; private set; }
}
