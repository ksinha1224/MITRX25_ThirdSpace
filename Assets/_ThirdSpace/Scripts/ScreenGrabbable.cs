using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenGrabbable : MonoBehaviour
{
    [field: SerializeField] public RectTransform rectTransform;

    public void UpdatePositionBy(Vector3 diff)
    {
        rectTransform.localPosition += diff;
    }
}
