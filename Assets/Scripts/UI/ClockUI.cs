using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ClockUI : MonoBehaviour {

    [SerializeField] private TextMeshProUGUI clockText;
    [SerializeField] private Image activeBackground;
    
    private void Start() {
        GameManager.Instance.OnStateChange += GameManager_OnStateChange;
        gameObject.SetActive(false);
    }
    
    private void Update() {
        if (!GameManager.Instance.IsGamePlaying()) {
            return;
        }
        
        float gamePlayingTimer = GameManager.Instance.GetGamePlayingTimer();
        int minutes = Mathf.FloorToInt(gamePlayingTimer / 60f);
        int seconds = Mathf.FloorToInt(gamePlayingTimer % 60f);
        clockText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        activeBackground.fillAmount = 1 - gamePlayingTimer / GameManager.Instance.GetRoundTime();
    }
    
    private void GameManager_OnStateChange(object sender, System.EventArgs e) {
        gameObject.SetActive(GameManager.Instance.IsGamePlaying());
    }

}
