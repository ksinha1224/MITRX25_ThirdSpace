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
    [SerializeField] private RectTransform cursorParentRect;
    [SerializeField] private RawImage outline;
    [SerializeField] private RawImage icon;

    [SerializeField] private Texture cursorOpen;
    [SerializeField] private Texture cursorClosed;

    [SerializeField] private Color defaultColor;
    [SerializeField] private Color selectedColor;

    private bool isMoving;
    private Transform followPoint;

    private Vector3 lastLocalPos;
    private ScreenGrabbable toMove;

    // Start is called before the first frame update
    void Start()
    {
        icon.texture = cursorOpen;
        icon.color = defaultColor;

        outline.enabled = false;
    }

    private void Update()
    {
        if(GameManager.Instance.CurrentState == GameState.PlayerInteraction || GameManager.Instance.CurrentState == GameState.Tutorial)
        {
            MovementUpdate();
        }
    }

    private void MovementUpdate()
    {
        if (!isMoving)
            return;

        Vector3 newPos = followPoint.position;
        newPos.z = transform.position.z;

        transform.position = newPos;
        ClampToScreen();

        if (toMove == null)
            return;

        Vector3 popupDiff = cursorParentRect.localPosition - lastLocalPos;
        toMove.UpdatePositionBy(popupDiff);

        lastLocalPos = cursorParentRect.localPosition;
    }

    #region xr interaction events
    public void INTERACT_OnHover()
    {
        if (!isMoving)
            outline.enabled = true;

        if(GameManager.Instance.CurrentState == GameState.Tutorial)
        {
            GameManager.Instance.TutorialUpdate(1);
        }
    }

    public void INTERACT_OnHoverExit()
    {
        if(!isMoving)
            outline.enabled = false;

        if (GameManager.Instance.CurrentState == GameState.Tutorial)
        {
            GameManager.Instance.TutorialUpdate(0);
        }
    }

    public void INTERACT_OnSelect(SelectEnterEventArgs args)
    {
        outline.enabled = false;

        icon.color = selectedColor;
        isMoving = true;

        followPoint = args.interactorObject.GetAttachTransform(args.interactableObject);

        if (GameManager.Instance.CurrentState == GameState.Tutorial)
        {
            GameManager.Instance.TutorialUpdate(2);
        }
    }

    public void INTERACT_OnRelease()
    {
        icon.color = defaultColor;
        isMoving = false;
    }

    public void INTERACT_OnActivate()
    {
        icon.texture = cursorClosed;

        List<ScreenGrabbable> intersectedGrabbables = new List<ScreenGrabbable>();
        foreach(ScreenGrabbable grabbable in GameManager.Instance.Grabbables)
        {
            bool intersects = cursorParentRect.WorldIntersects(grabbable.rectTransform);
            if(intersects)
            {
                intersectedGrabbables.Add(grabbable);
            }
        }

        if (intersectedGrabbables.Count == 0)
        {
            if (GameManager.Instance.CurrentState == GameState.Tutorial)
            {
                GameManager.Instance.TutorialUpdate(3);
            }

            return;
        }

        toMove = intersectedGrabbables.OrderByDescending(popup => popup.transform.GetSiblingIndex()).ToList()[0];
        lastLocalPos = cursorParentRect.localPosition;

        toMove.transform.SetAsLastSibling(); //brings to front of render order, simulating "focusing" the popup

        if (GameManager.Instance.CurrentState == GameState.Tutorial)
        {
            GameManager.Instance.TutorialUpdate(4);
        }
    }

    public void INTERACT_OnDeactivate()
    {
        icon.texture = cursorOpen;

        if (toMove.gameObject.TryGetComponent(out Popup popup)) //this is gross sorry
        {
            popup.IdentifyCheck();
        }

        toMove = null;
    }

    #endregion

    private void ClampToScreen()
    {
        Vector3 thisPos = cursorParentRect.localPosition;
        Vector3 minPos = screenRect.rect.min - cursorParentRect.rect.min;
        Vector3 maxPos = screenRect.rect.max - cursorParentRect.rect.max;

        thisPos.x = Mathf.Clamp(cursorParentRect.localPosition.x, minPos.x, maxPos.x);
        thisPos.y = Mathf.Clamp(cursorParentRect.localPosition.y, minPos.y, maxPos.y);

        cursorParentRect.localPosition = thisPos;
    }
}
