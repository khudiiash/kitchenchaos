using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSounds : MonoBehaviour {
    
    private Player player;
    private float footstepTimer;
    private float footstepTimerMax = 0.1f;

    private void Awake() {
        player = GetComponent<Player>();
    }
    
    private void Update() {
        if (player.IsWalking() == false) {
            return;
        }
        footstepTimer -= Time.deltaTime;
        if (footstepTimer < 0f) {
            footstepTimer += footstepTimerMax;
            SFXManager.Instance.PlayFootsteps();
        }
    }
    
}
