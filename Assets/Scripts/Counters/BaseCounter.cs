using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BaseCounter : MonoBehaviour, IKitchenObjectParent {


    [SerializeField] private Transform counterTopPoint;
    private KitchenObject kitchenObject;
    public static event EventHandler OnAnyObjectPlaced;
    public virtual void Interact(Player player) {

    }
    public virtual void InteractAlternate(Player player) {
        
    }
    
    public Transform GetKitchenObjectFollowTransform() {
        return counterTopPoint;
    }
    
    public KitchenObject GetKitchenObject() {
        return kitchenObject;
    }
    
    public void ClearKitchenObject() {
        kitchenObject = null;
    }
    
    public void SetKitchenObject(KitchenObject kitchenObject) {
        this.kitchenObject = kitchenObject;
        if (kitchenObject != null) {
            OnAnyObjectPlaced?.Invoke(this, EventArgs.Empty);
        }
    }
    
    public bool HasKitchenObject() {
        return this.kitchenObject != null;
    }
}
