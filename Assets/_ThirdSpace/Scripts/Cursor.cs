using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;
using TMPro;
using System.Linq;

public class Cursor : MonoBehaviour
{
    [SerializeField] private RectTransform screenRect;
    [SerializeField] private RawImage image;

    private RectTransform thisRect;
    private bool isMoving;
    private Transform followPoint;

    private Vector3 lastLocalPos;
    private Popup toMove;

    // Start is called before the first frame update
    void Start()
    {
        image.color = Color.black;
        thisRect = image.rectTransform;
    }

    private void Update()
    {
        MovementUpdate();
    }

    void MovementUpdate()
    {
        if (!isMoving)
            return;

        Vector3 newPos = followPoint.position;
        newPos.z = transform.position.z;

        transform.position = newPos;
        ClampToScreen();

        if (toMove == null)
            return;

        Vector3 popupDiff = thisRect.localPosition - lastLocalPos;
        toMove.UpdatePositionBy(popupDiff);

        lastLocalPos = thisRect.localPosition;
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

    public void INTERACT_OnActivate()
    {
        //simulate mouse click
        //check for any overlapping popup rects
        //only pass on effects to the "topmost" one in depth order (highest on top- instantiation order)

        List<Popup> intersectedPopups = new List<Popup>();
        foreach(Popup popup in GameManager.Instance.ActivePopups)
        {
            bool intersects = RectsIntersect(thisRect, popup.rectTransform);
            Debug.Log($"popup {popup.gameObject.name} intersects? {intersects}");

            if(intersects)
            {
                intersectedPopups.Add(popup);
            }
        }

        intersectedPopups = intersectedPopups.OrderByDescending(popup => popup.DepthLayer).ToList();
        //intersectedPopups[0].StartMoving(thisRect);

        toMove = intersectedPopups[0];
        lastLocalPos = thisRect.localPosition;
    }

    public void INTERACT_OnDeactivate()
    {
        toMove = null;
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

    private bool RectsIntersect(RectTransform a, RectTransform b)
    {
        Rect worldRectA = WorldRect(a);
        Rect worldRectB = WorldRect(b);
        return worldRectA.Overlaps(worldRectB);
    }

    //ref: https://stackoverflow.com/questions/42043017/check-if-ui-elements-recttransform-are-overlapping
    public Rect WorldRect(RectTransform rectTransform)
    {
        Vector2 sizeDelta = rectTransform.sizeDelta;
        float rectTransformWidth = sizeDelta.x * rectTransform.lossyScale.x;
        float rectTransformHeight = sizeDelta.y * rectTransform.lossyScale.y;

        Vector3 position = rectTransform.position;
        return new Rect(position.x - rectTransformWidth / 2f, position.y - rectTransformHeight / 2f, rectTransformWidth, rectTransformHeight);
    }
}
