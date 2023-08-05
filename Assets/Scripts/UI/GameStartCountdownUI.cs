using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class GameStartCountdownUI : MonoBehaviour {
    
    [SerializeField] private TextMeshProUGUI countdownText;
    
    private void Start() {
        GameManager.Instance.OnStateChange += GameManager_OnStateChange;
        gameObject.SetActive(false);
    }
    
    private void Update() {
        if (!GameManager.Instance.IsCountdownToStart()) {
            return;
        }
        
        float countdownTimer = GameManager.Instance.GetCountdownToStartTimer();
        countdownText.text = Mathf.CeilToInt(countdownTimer).ToString();
    }
    
    private void GameManager_OnStateChange(object sender, EventArgs e) {
        gameObject.SetActive(GameManager.Instance.IsCountdownToStart());
    }

}
