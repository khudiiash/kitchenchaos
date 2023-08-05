using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IKitchenObjectParent {
    
    public Transform GetKitchenObjectFollowTransform();
    
    public KitchenObject GetKitchenObject();
    
    public void ClearKitchenObject();     
    public void SetKitchenObject(KitchenObject kitchenObject);    
    public bool HasKitchenObject();

}