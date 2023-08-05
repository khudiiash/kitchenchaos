using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlateKitchenObject : KitchenObject {

    private List<KitchenObjectSO> kitchenObjectSOList;
    public event EventHandler<OnIngredientAddedEventArgs> OnIngredientAdded;
    public class OnIngredientAddedEventArgs : EventArgs {
        public KitchenObjectSO kitchenObjectSO;
    } 
    
    [SerializeField] private List<KitchenObjectSO> validKitchenObjectSOList;
    
    private void Awake() {
        kitchenObjectSOList = new List<KitchenObjectSO>();
    }
    public bool TryAddIngredient(KitchenObjectSO kitchenObjectSO) {
        if (!validKitchenObjectSOList.Contains(kitchenObjectSO)) return false;

        if (kitchenObjectSOList.Contains(kitchenObjectSO)) {
            // already have this ingredient
            return false;
        } else {
            kitchenObjectSOList.Add(kitchenObjectSO);
            OnIngredientAdded?.Invoke(this, new OnIngredientAddedEventArgs {
                kitchenObjectSO = kitchenObjectSO
            });
           return true; 
        }
    }
    
    public List<KitchenObjectSO> GetKitchenObjectSOList() {
        return kitchenObjectSOList;
    }
    
    public bool IsRecipeEqual(RecipeSO recipeSO) {
        if (recipeSO.kitchenObjectList.Count != kitchenObjectSOList.Count) {
            return false;
        }
        foreach (KitchenObjectSO kitchenObjectSO in kitchenObjectSOList) {
            if (!recipeSO.kitchenObjectList.Contains(kitchenObjectSO)) {
                return false;
            }
        }
        return true;
    }
}
 