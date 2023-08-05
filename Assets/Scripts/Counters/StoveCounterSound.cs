using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoveCounterSound : MonoBehaviour {
    
    [SerializeField] private StoveCounter stoveCounter;
    private AudioSource audioSource;

    private void Awake() {
        audioSource = GetComponent<AudioSource>();
    }
    
    private void Start() {
        stoveCounter.OnStateChange += StoveCounter_OnStateChange;
    }
    
    private void StoveCounter_OnStateChange(object sender, StoveCounter.OnStateChangeEventArgs e) {
        bool isFrying = e.state != StoveCounter.State.Idle;
        if (isFrying) {
            audioSource.Play();
        } else {
            audioSource.Pause();
        }
    }
    
    
}
