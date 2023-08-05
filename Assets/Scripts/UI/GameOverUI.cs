using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameOverUI : MonoBehaviour {

    [SerializeField] private TextMeshProUGUI recipesDeliveredText;
    private void Start() {
        GameManager.Instance.OnStateChange += GameManager_OnStateChange;
        gameObject.SetActive(false);
    }
    
    private void GameManager_OnStateChange(object sender, System.EventArgs e) {
        gameObject.SetActive(GameManager.Instance.IsGameOver());
        if (GameManager.Instance.IsGameOver()) {
            recipesDeliveredText.text = DeliveryManager.Instance.GetRecipesDelivered().ToString();
        }
    }    

}
