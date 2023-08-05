using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuttingCounter : BaseCounter {

    [SerializeField] CuttingRecipeSO[] cuttingRecipeSOArray; 

    public static event EventHandler OnAnyCut;
        
    public event EventHandler<OnPlayerCutEventArgs> OnPlayerCut;
    public class OnPlayerCutEventArgs : EventArgs {
        public float progressNormalized;
    }
    
    private int cuttingProgress;
    public override void Interact(Player player) {
        if (!HasKitchenObject()) {
            // There is no kitchen object here
            if (player.HasKitchenObject() && HasRecipeWithInput(player.GetKitchenObject().GetKitchenObjectSO())) {
                player.GetKitchenObject().SetKitchenObjectParent(this);
                UpdateProgress(0);
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
                UpdateProgress(0);
                GetKitchenObject().SetKitchenObjectParent(player);
            }
        }
    }
    
    private void UpdateProgress(int progress) {
        cuttingProgress = progress;
        CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(GetKitchenObject().GetKitchenObjectSO());
        if (!cuttingRecipeSO) return;

        int maxProgress = cuttingRecipeSO.cuttingProgressMax;
        if (cuttingProgress > maxProgress) return;

        OnPlayerCut?.Invoke(this, new OnPlayerCutEventArgs {
            progressNormalized = maxProgress > 0 ? (float)cuttingProgress / cuttingRecipeSO.cuttingProgressMax : 0
        });
        OnAnyCut?.Invoke(this, EventArgs.Empty);
    }

      public override void InteractAlternate(Player player) {
          if (HasKitchenObject() && HasRecipeWithInput(GetKitchenObject().GetKitchenObjectSO())) {
            // Cut the kitchenObject
            UpdateProgress(cuttingProgress + 1);
            CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(GetKitchenObject().GetKitchenObjectSO());

            if (cuttingProgress >= cuttingRecipeSO.cuttingProgressMax) {
                GetKitchenObject().DestroySelf();
                KitchenObject.SpawnKitchenObject(cuttingRecipeSO.output, this);
            }
          }
      }
      
      private bool HasRecipeWithInput(KitchenObjectSO inputKitchenObjectSO) {
        return !!GetCuttingRecipeSOWithInput(inputKitchenObjectSO);
      }
      
      private KitchenObjectSO GetOutputForInput(KitchenObjectSO inputKitchenObjectSO) {
          CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(inputKitchenObjectSO);
          if (!cuttingRecipeSO) return null;
          return cuttingRecipeSO.output;
      }
      
      private CuttingRecipeSO GetCuttingRecipeSOWithInput(KitchenObjectSO inputKitchenObjectSO) {
          foreach (CuttingRecipeSO cuttingRecipeSO in cuttingRecipeSOArray) {
            if (cuttingRecipeSO.input == inputKitchenObjectSO) {
                return cuttingRecipeSO;
            }
          }
          return null;
      }

}
