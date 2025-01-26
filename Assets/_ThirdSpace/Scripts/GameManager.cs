using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameState CurrentState { get; private set; }
    public static Action<GameState> OnGameStateChange;

    public List<ScreenGrabbable> Grabbables { get; private set; }
    public List<Popup> ActivePopups { get; private set; }

    [SerializeField] private GameObject editorSim;

    [SerializeField] private Computer computer;
    [SerializeField] private List<Folder> folders;

    //[field: SerializeField] public List<PopupData> testContent { get; private set; }
    [field: SerializeField] public List<PopupData> popupContent { get; private set; }

    public RectTransform StartButton;

    [SerializeField] private GameObject tutorialPopup;
    [SerializeField] private TMP_Text tutorialText;
    [SerializeField] private RawImage tutorialImg;
    [SerializeField] private Texture tutorialStep0;
    [SerializeField] private Texture tutorialStep1;
    [SerializeField] private Texture tutorialStep2;

    private int lastOpCode = -1;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;

#if UNITY_EDITOR
        editorSim.SetActive(true); //if this is active in build a fatal error crashes the game immediately???
#else
        editorSim.SetActive(false);
#endif
        OnGameStateChange += UpdateGameState;

        ActivePopups = new List<Popup>();
        Grabbables = new List<ScreenGrabbable>();

        for(int i = 0; i < folders.Count; i++)
        {
            folders[i].Init((MediaClassification)i);
            Grabbables.Add(folders[i].grabbable);
        }

        var count = popupContent.Count;
        var last = count - 1;
        for (var i = 0; i < last; ++i)
        {
            var r = UnityEngine.Random.Range(i, count);
            var tmp = popupContent[i];
            popupContent[i] = popupContent[r];
            popupContent[r] = tmp;
        }
    }

    private void Start()
    {
        StartCoroutine(computer.FadeScreenOn());
    }

    public void PlayClick()
    {
        computer.PlayClick();
    }

    public void StartGame()
    {
        StartCoroutine(computer.StartGame());
    }

    public void TutorialUpdate(int opCode)
    {
        //0 = unhovered
        //1 = hovered / unselected
        //2 = selected / deactivated
        //3 = activated NO POPUP
        //4 = activated WITH POPUP
        //5 = moved into folder (complete)

        if (opCode <= lastOpCode) return;

        lastOpCode = opCode;

        if (opCode < 0 || opCode > 5)
        {
            Debug.LogError($"unknown tutorial opcode {opCode}");
            return;
        }

        StopAllCoroutines();

        string newText = string.Empty;
        Texture toChange = null;
        switch(opCode)
        {
            case 0:
                newText = "Point to the <b>cursor</b> with your controller.";
                toChange = tutorialStep0;
                break;
            case 1:
                newText = "Use the hand grips to move the <b>cursor</b> around the <b>screen</b>.";
                toChange = tutorialStep1;
                break;
            case 2:
                newText = "While holding the grip, use the index trigger to select <b>popups</b> on the screen.";
                tutorialPopup.gameObject.SetActive(true);
                toChange = tutorialStep2;
                break;
            case 3:
                newText = "Try selecting the <b>popup</b>.";
                break;
            case 4:
                newText = "Drag the <b>popup</b> to its folder to <b>identify</b> it.";
                break;
            case 5:
                newText = "That's the flow. Good work.";
                OnGameStateChange?.Invoke(GameState.NotPlaying);
                break;
        }

        StartCoroutine(computer.ChangeText(tutorialText, newText));
        if (toChange != null) ;
            StartCoroutine(computer.ChangeImage(tutorialImg, toChange));
    }

    public void AddPopup(Popup toAdd)
    {
        ActivePopups.Add(toAdd);
        Grabbables.Add(toAdd.grabbable);
    }

    public void RemovePopup(Popup toRemove)
    {
        ActivePopups.Remove(toRemove);
        Grabbables.Remove(toRemove.grabbable);
        

        if(ActivePopups.Count == 0)
        {
            OnGameStateChange?.Invoke(GameState.NotPlaying);
            computer.EndGame();
        }
    }

    public Folder GetFolder(MediaClassification toGrab)
    {
        return folders.Where(folder => folder.classification == toGrab).First();
    }

    private void UpdateGameState(GameState newState)
    {
        CurrentState = newState;
    }
}

public enum GameState { NotPlaying, Tutorial, Cutscene, PlayerInteraction }
