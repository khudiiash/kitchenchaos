using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System;

public class CuttingCounterVisual : MonoBehaviour {
    
    Animator animator;
    private string CUT = "Cut";
    
    [SerializeField] private CuttingCounter cuttingCounter;
    [SerializeField] private Image barImage;
    [SerializeField] private Canvas progressBar;
    private void Awake() {
        animator = GetComponent<Animator>();
    }
    private void Start() {
        progressBar.enabled = false;
        cuttingCounter.OnPlayerCut += OnPlayerCut;
    }

    private void OnPlayerCut(object sender, CuttingCounter.OnPlayerCutEventArgs e) {
        float progress = e.progressNormalized;
        barImage.fillAmount = progress;
        progressBar.enabled = progress > 0 && progress < 1.0;            
        if (progress > 0) animator.SetTrigger(CUT);
    }


}
