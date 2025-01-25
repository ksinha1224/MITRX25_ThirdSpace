using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Popup : MonoBehaviour
{
    [field: SerializeField] public int DepthLayer { get; private set; }
    [field: SerializeField] public RectTransform rectTransform { get; private set; }
    [SerializeField] private RawImage contentImg;
    [SerializeField] private TMP_Text contentTxt;

    bool moveWithCursor = false;
    RectTransform followTransform;

    private Vector3 lastFollowPos;

    public void StartMoving(RectTransform toFollow)
    {
        moveWithCursor = true;
        followTransform = toFollow;
        lastFollowPos = toFollow.localPosition;

        Debug.Log($"selected {gameObject.name}");
    }
    public void StopMoving()
    {
        moveWithCursor = false;
    }

    public void UpdatePositionBy(Vector3 diff)
    {
        rectTransform.localPosition += diff;
    }

    //private void Update()
    //{
    //    if(moveWithCursor)
    //    {
    //        Vector3 diff = followTransform.localPosition - lastFollowPos;
            

    //        lastFollowPos = followTransform.localPosition;
    //    }
    //}
}
