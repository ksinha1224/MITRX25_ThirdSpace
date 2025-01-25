using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class RectExtensions
{
    public static bool WorldIntersects(this RectTransform a, RectTransform b)
    {
        Rect worldRectA = WorldRect(a);
        Rect worldRectB = WorldRect(b);
        return worldRectA.Overlaps(worldRectB);
    }

    //ref: https://stackoverflow.com/questions/42043017/check-if-ui-elements-recttransform-are-overlapping
    public static Rect WorldRect(this RectTransform rectTransform)
    {
        Vector2 sizeDelta = rectTransform.sizeDelta;
        float rectTransformWidth = sizeDelta.x * rectTransform.lossyScale.x;
        float rectTransformHeight = sizeDelta.y * rectTransform.lossyScale.y;

        Vector3 position = rectTransform.position;
        return new Rect(position.x - rectTransformWidth / 2f, position.y - rectTransformHeight / 2f, rectTransformWidth, rectTransformHeight);
    }
}
