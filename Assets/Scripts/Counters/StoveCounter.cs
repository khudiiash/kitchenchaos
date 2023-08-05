using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class StoveCounter : BaseCounter {
    
    [SerializeField] private FryingRecipeSO[] fryingRecipeSOArray;

    private float fryingTimer = 0f; 
    private int fryingProgress;
    public State state;
    
    private FryingRecipeSO fryingRecipeSO;
    public enum State {
        Idle,
        Frying,
        Fried,
        Burned
    }
    
    public event EventHandler<OnFryingProgressEventArgs> OnFryingProgress;
    public event EventHandler<OnStateChangeEventArgs> OnStateChange;
    public class OnStateChangeEventArgs : EventArgs {
        public State state;
    }
    public class OnFryingProgressEventArgs : EventArgs {
        public float progressNormalized;
    }
    
    private void Start() {
        state = State.Idle;
    }
    
    private void Update() {
        if (HasKitchenObject()) {
            switch (state) {
                case State.Idle:
                    break;
                case State.Frying:
                    fryingTimer += Time.deltaTime;
                    if (fryingTimer > fryingRecipeSO.fryingTimerMax) {
                        // Fried
                        fryingTimer = 0f;
                        GetKitchenObject().DestroySelf();
                        KitchenObject.SpawnKitchenObject(fryingRecipeSO.output, this);
                        UpdateState(State.Fried);
                    }
                    UpdateProgress(fryingTimer / fryingRecipeSO.fryingTimerMax);
                    break;
                case State.Fried:
                    if (fryingTimer > fryingRecipeSO.fryingTimerMax) {
                        fryingRecipeSO = GetFryingRecipeSOWithInput(GetKitchenObject().GetKitchenObjectSO());
                        GetKitchenObject().DestroySelf();
                        KitchenObject.SpawnKitchenObject(fryingRecipeSO.output, this);
                        UpdateState(State.Burned);
                    } else {
                        fryingTimer += Time.deltaTime;
                    }
                    UpdateProgress(fryingTimer / fryingRecipeSO.fryingTimerMax);
                    break;
                case State.Burned:
                    break;

            }
        } else if (state != State.Idle) {
           UpdateState(State.Idle);
           UpdateProgress(0);
        }
    }
    

    public override void Interact(Player player) {
        if (!HasKitchenObject()) {
            // There is no kitchen object here
            if (player.HasKitchenObject() && HasRecipeWithInput(player.GetKitchenObject().GetKitchenObjectSO())) {
                player.GetKitchenObject().SetKitchenObjectParent(this);
                fryingRecipeSO = GetFryingRecipeSOWithInput(GetKitchenObject().GetKitchenObjectSO());
                UpdateState(State.Frying);
            } else {
                // Player has nothing
            }
        } else {
            // There is a kitchen object here
            if (player.HasKitchenObject()) {
                // Player has kitchen object
                if (player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject)) {
                    if (plateKitchenObject.TryAddIngredient(GetKitchenObject().GetKitchenObjectSO())) {
                        GetKitchenObject().DestroySelf();
                    }
                }
            } else {
                // Give kitchen object back to player
                GetKitchenObject().SetKitchenObjectParent(player);
                fryingTimer = 0f;
                UpdateProgress(0f);
            }
        }
    }
    
    private void UpdateProgress(float progress) {
        OnFryingProgress?.Invoke(this, new OnFryingProgressEventArgs {
            progressNormalized = progress,
        });
    }
    
    private void UpdateState(State state) {
        this.state = state;
        OnStateChange?.Invoke(this, new OnStateChangeEventArgs {
            state = state
        });
    }
    

  public override void InteractAlternate(Player player) {
      if (HasKitchenObject() && HasRecipeWithInput(GetKitchenObject().GetKitchenObjectSO())) {
        // Fry the kitchenObject
        FryingRecipeSO fryingRecipeSO = GetFryingRecipeSOWithInput(GetKitchenObject().GetKitchenObjectSO());

        if (fryingProgress >= fryingRecipeSO.fryingTimerMax) {
            GetKitchenObject().DestroySelf();
            KitchenObject.SpawnKitchenObject(fryingRecipeSO.output, this);
        }
      }
  }
  
  private bool HasRecipeWithInput(KitchenObjectSO inputKitchenObjectSO) {
    return !!GetFryingRecipeSOWithInput(inputKitchenObjectSO);
  }
  
  private FryingRecipeSO GetFryingRecipeSOWithInput(KitchenObjectSO inputKitchenObjectSO) {
      foreach (FryingRecipeSO fryingRecipeSO in fryingRecipeSOArray) {
        if (fryingRecipeSO.input == inputKitchenObjectSO) {
            return fryingRecipeSO;
        }
      }
      return null;
  }

}
