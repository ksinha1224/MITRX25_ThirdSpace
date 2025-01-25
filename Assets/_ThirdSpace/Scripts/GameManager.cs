using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameState CurrentState { get; private set; }
    public Action<GameState> OnGameStateChange;

    [SerializeField] private Computer computer;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;

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
}

public enum GameState { NotPlaying, Cutscene, PlayerInteraction }
