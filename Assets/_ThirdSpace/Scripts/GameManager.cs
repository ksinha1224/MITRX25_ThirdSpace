using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameState CurrentState { get; private set; }
    public Action<GameState> OnGameStateChange;

    [field: SerializeField] public List<Popup> ActivePopups { get; private set; }

    [SerializeField] private GameObject editorSim;
    [SerializeField] private Computer computer;
    [SerializeField] private List<Folder> folders;

    [field: SerializeField] public List<PopupData> testContent { get; private set; }

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
        computer.Init();
    }

    private void UpdateGameState(GameState newState)
    {
        CurrentState = newState;
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
}

public enum GameState { NotPlaying, Cutscene, PlayerInteraction }
