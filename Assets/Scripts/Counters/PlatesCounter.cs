using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class PlatesCounter : BaseCounter {
    
    private float spawnPlateTimer;
    
    public event EventHandler OnPlateSpawned;
    public event EventHandler OnPlateTaken;
    
    [SerializeField] float spawnTime = 5f;
    [SerializeField] KitchenObjectSO plateObjectSO;
    
    
    private int platesSpawnedAmout;
    private int platesSpawnedAmoutMax = 5;
    
    private void Update() {
        spawnPlateTimer += Time.deltaTime; 
        if (spawnPlateTimer > spawnTime && platesSpawnedAmout < platesSpawnedAmoutMax) {
            spawnPlateTimer = 0f;
            OnPlateSpawned?.Invoke(this, EventArgs.Empty);
            platesSpawnedAmout += 1;
        }
    }
    public override void Interact(Player player) {
        if (player.HasKitchenObject()) return;
        if (platesSpawnedAmout == 0) return;
        KitchenObject.SpawnKitchenObject(plateObjectSO, player);
        platesSpawnedAmout -= 1;
        OnPlateTaken?.Invoke(this, EventArgs.Empty);
    }
}
