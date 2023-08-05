using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameManager : MonoBehaviour {
    
    static public GameManager Instance { get; private set; }
    public event EventHandler OnStateChange;
    
    private enum State {
        WaitingToStart,
        CountdownToStart,
        GamePlaying,
        GameOver
    }
    
    private State state;
    private float waitingToStartTimer = 1f;
    private float countdownToStartTimer = 3f;
    private float gamePlayingTimer;
    [SerializeField] private float roundTime = 120f;


    private void Awake() {
        Instance = this;
        state = State.WaitingToStart;
        gamePlayingTimer = roundTime;
    }
    
    private void UpdateState(State state) {
        this.state = state;
        OnStateChange?.Invoke(this, EventArgs.Empty);
    }
    
    private void Update() {
        switch (state) {
            case State.WaitingToStart:
                waitingToStartTimer -= Time.deltaTime;
                if (waitingToStartTimer < 0f) {
                    UpdateState(State.CountdownToStart);
                }
                break;
            case State.CountdownToStart:
                countdownToStartTimer -= Time.deltaTime;
                if (countdownToStartTimer < 0f) {
                    UpdateState(State.GamePlaying);
                }
                break;
            case State.GamePlaying:
                gamePlayingTimer -= Time.deltaTime;
                if (gamePlayingTimer < 0f) {
                    UpdateState(State.GameOver);
                }
                break;
            case State.GameOver:
                break;
                
        }
    }
    
    public bool IsGamePlaying() {
        return state == State.GamePlaying;
    }
    public bool IsCountdownToStart() {
        return state == State.CountdownToStart;
    }
    
    public bool IsGameOver() {
        return state == State.GameOver;
    }
    
    public float GetCountdownToStartTimer() {
        return countdownToStartTimer;
    }
    
    public float GetGamePlayingTimer() {
        return gamePlayingTimer;
    }
    
    public float GetRoundTime() {
        return roundTime;
    }
}

