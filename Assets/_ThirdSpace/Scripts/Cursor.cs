using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;
using TMPro;

public class Cursor : MonoBehaviour
{
    [SerializeField] private RectTransform screenRect;
    [SerializeField] private RawImage image;

    private RectTransform thisRect;
    private bool isMoving;
    private Transform followPoint;

    // Start is called before the first frame update
    void Start()
    {
        image.color = Color.black;
        thisRect = image.rectTransform;
    }

    private void Update()
    {
        if(isMoving)
        {
            Vector3 newPos = followPoint.position;
            newPos.z = transform.position.z;

            transform.position = newPos;
            ClampToScreen();
        }
    }

    public void INTERACT_OnHover()
    {
        if(!isMoving)
            image.color = Color.blue;
    }

    public void INTERACT_OnHoverExit()
    {
        if(!isMoving)
            image.color = Color.black;
    }

    public void INTERACT_OnSelect(SelectEnterEventArgs args)
    {
        image.color = Color.green;
        isMoving = true;

        followPoint = args.interactorObject.GetAttachTransform(args.interactableObject);
    }

    public void INTERACT_OnRelease()
    {
        image.color = Color.black;
        isMoving = false;
    }

    public void INTERACT_OnFocus()
    {
        //simulate mouse click
        //check for any overlapping popup rects
        //only pass on effects to the "topmost" one in depth order (lowest no)
    }

    private void ClampToScreen()
    {
        Vector3 thisPos = thisRect.localPosition;
        Vector3 minPos = screenRect.rect.min - thisRect.rect.min;
        Vector3 maxPos = screenRect.rect.max - thisRect.rect.max;

        thisPos.x = Mathf.Clamp(thisRect.localPosition.x, minPos.x, maxPos.x);
        thisPos.y = Mathf.Clamp(thisRect.localPosition.y, minPos.y, maxPos.y);

        thisRect.localPosition = thisPos;
    }
}
