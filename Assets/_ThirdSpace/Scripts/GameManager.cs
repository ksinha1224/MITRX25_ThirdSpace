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

    [field: SerializeField] public List<Popup> ActivePopups { get; private set; }

    [SerializeField] private GameObject editorSim;
    [SerializeField] private Computer computer;
    [SerializeField] private List<Folder> folders;

    [field: SerializeField] public List<PopupData> testContent { get; private set; }

    [SerializeField] private CanvasGroup warningGroup;
    [SerializeField] private CanvasGroup tutorialGroup;
    [SerializeField] private CanvasGroup startGroup;

    [SerializeField] private TMP_Text tutorialText;
    [SerializeField] private RawImage tutorialImg;
    [SerializeField] private Texture tutorialStep1;
    [SerializeField] private Texture tutorialStep2;

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
    }

    private void Start()
    {
        //todo: intro and tutorial flow

        //OnGameStateChange?.Invoke(GameState.PlayerInteraction);
        //computer.Init();

        StartCoroutine(IntroRoutine());
    }

    private IEnumerator IntroRoutine()
    {
        yield return null;

        //computer screen fades on
        //warning fades on
        //wait a bit
        //tutorial fades on
        //run thru tutorial
        //load start screen
        //wait for button click to trigger cutscene
    }

    public void TutorialUpdate(int opCode)
    {
        //0 = unhovered
        //1 = hovered / unselected
        //2 = selected / deactivated
        //3 = activated NO POPUP
        //4 = activated WITH POPUP
        //5 = moved into folder (complete)

        if (opCode < 0 || opCode > 5)
        {
            Debug.LogError($"unknown tutorial opcode {opCode}");
            return;
        }

        StopAllCoroutines();

        string newText = string.Empty;
        switch(opCode)
        {
            case 0:
                newText = "Point to the <b>cursor</b> with your controller.";
                break;
            case 1:
                newText = "Use the hand grips to move the <b>cursor</b> around the <b>screen</b>.";
                break;
            case 2:
                newText = "While holding the grip, use the index trigger to select <b>popups</b> on the screen.";
                break;
            case 3:
                newText = "Try selecting the <b>popup</b>.";
                break;
            case 4:
                newText = "Drag the <b>popup</b> to its folder to <b>identify</b> it.";
                break;
            case 5:
                newText = "That's the flow. Good work.";
                break;
        }

        StartCoroutine(ChangeText(newText));
    }

    public void AddPopup(Popup toAdd)
    {
        ActivePopups.Add(toAdd);
    }

    public void RemovePopup(Popup toRemove)
    {
        ActivePopups.Remove(toRemove);
    }

    public Folder GetFolder(MediaClassification toGrab)
    {
        return folders.Where(folder => folder.classification == toGrab).First();
    }

    private IEnumerator ChangeText(string changeTo)
    {
        yield return FadeGraphic(tutorialText, false);
        tutorialText.text = changeTo;
        yield return FadeGraphic(tutorialText, true);
    }

    private IEnumerator FadeGraphic(Graphic graphic, bool on)
    {
        if (on)
        {
            while (graphic.color.a < 1)
            {
                Color newColor = graphic.color;
                newColor.a += 0.1f;
                yield return new WaitForSeconds(0.1f);
            }
        }
        else
        {
            while (graphic.color.a > 0)
            {
                Color newColor = graphic.color;
                newColor.a -= 0.1f;
                yield return new WaitForSeconds(0.1f);
            }
        }
    }

    private void UpdateGameState(GameState newState)
    {
        CurrentState = newState;
    }
}

public enum GameState { NotPlaying, Tutorial, Cutscene, PlayerInteraction }
