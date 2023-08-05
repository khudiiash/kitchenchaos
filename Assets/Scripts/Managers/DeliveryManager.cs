using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DeliveryManager : MonoBehaviour {
  
    public static DeliveryManager Instance { get; private set; }

    [SerializeField] private RecipeListSO recipeListSO;
    [SerializeField] float spawnRecipeTimerMax = 15f;

    private List<RecipeSO> waitingRecipeSOList;
    private float spawnRecipeTimer;
    private int waitingRecipeMax = 4; 
    
    public event EventHandler<OnRecipeListChangedEventArgs> OnRecipeListChanged;
    public class OnRecipeListChangedEventArgs : EventArgs {
        public List<RecipeSO> recipesList;
    }
    public event EventHandler OnRecipeSuccess;
    public event EventHandler OnRecipeFailed;
    private int successfulRecipesAmout = 0;
    
    private void Awake() {
        if (Instance != null) {
           Debug.Log("There is more than one delivery manager instance"); 
        }
        Instance = this;
        waitingRecipeSOList = new List<RecipeSO>();
    }

    private void Update() {
        spawnRecipeTimer -= Time.deltaTime;
        if (spawnRecipeTimer <= 0f) {
            spawnRecipeTimer += spawnRecipeTimerMax;
            SpawnRecipe();
        }
    } 
    
    private void SpawnRecipe() {
        if (waitingRecipeSOList.Count >= waitingRecipeMax) {
            return;
        }
        RecipeSO waitingRecipeSO = recipeListSO.recipeList[UnityEngine.Random.Range(0, recipeListSO.recipeList.Count)];
        waitingRecipeSOList.Add(waitingRecipeSO);
        UpdateVisual();
    }
    
    private void UpdateVisual() {
        OnRecipeListChanged?.Invoke(this, new OnRecipeListChangedEventArgs {
            recipesList = waitingRecipeSOList
        });
    }
    
    public void DeliverRecipe(PlateKitchenObject plateKitchenObject) {
        for (int i = 0; i < waitingRecipeSOList.Count; i++) {
            RecipeSO waitingRecipeSO = waitingRecipeSOList[i];
        
            if (plateKitchenObject.IsRecipeEqual(waitingRecipeSO)) {
                waitingRecipeSOList.RemoveAt(i);
                Destroy(plateKitchenObject.gameObject);
                UpdateVisual();
                successfulRecipesAmout++;
                OnRecipeSuccess?.Invoke(this, EventArgs.Empty);
                return;
            }
        }
        OnRecipeFailed?.Invoke(this, EventArgs.Empty);
    }

    public int GetRecipesDelivered() {
        return successfulRecipesAmout;
    }
    
}
