using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System;

public class StoveCounterVisual : MonoBehaviour {
    
    private Image barImage;
    
    
    [SerializeField] private StoveCounter fryingCounter;
    [SerializeField] private Image[] bars;

    [SerializeField] private Canvas progressBar;
    [SerializeField] private GameObject[] enalbedObjects;
    
    private StoveCounter.State state;
    private StoveCounter.State[] states;

    private void Start() {
        progressBar.enabled = false;
        fryingCounter.OnStateChange += OnStateChange;
        fryingCounter.OnFryingProgress += OnFryingProgress;
        barImage = bars[0];
        state = StoveCounter.State.Idle;
        states = new StoveCounter.State[3] {StoveCounter.State.Frying, StoveCounter.State.Fried, StoveCounter.State.Burned};
    }
    
    private void OnStateChange(object sender, StoveCounter.OnStateChangeEventArgs e) {
        if (e.state != state) {
            barImage.gameObject.SetActive(false);
            barImage = bars[Array.IndexOf(states, e.state)];
            barImage.gameObject.SetActive(true);
        }
    }

    private void OnFryingProgress(object sender, StoveCounter.OnFryingProgressEventArgs e) {
        float progress = e.progressNormalized;
        progressBar.enabled = progress > 0;
        barImage.fillAmount = progress;

        foreach(GameObject enabledObject in enalbedObjects) {
            enabledObject.SetActive(progress > 0f);
        }
    }


}
