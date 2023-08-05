using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryManagerVisual : MonoBehaviour {

   [SerializeField] private DeliveryManager deliveryManager;
   [SerializeField] private List<RecipeSO> recipesList;
   [SerializeField] private GameObject recipesParent;
   [SerializeField] private GameObject recipePrefabObject;
   
   private void Start() {
         deliveryManager.OnRecipeListChanged += DeliveryManager_OnRecipeListChanged;
         recipesList = new List<RecipeSO>();
   }
   
    private void DeliveryManager_OnRecipeListChanged(object sender, DeliveryManager.OnRecipeListChangedEventArgs e) {
            recipesList = e.recipesList;
            UpdateVisual();
    }
    
    private void UpdateVisual() {
        foreach (Transform child in recipesParent.transform) {
            if (child.gameObject == recipePrefabObject) continue;
            Destroy(child.gameObject);
        }
        
        for (int i=0; i< recipesList.Count; i++) {
            RecipeSO waitingRecipeSO = recipesList[i];
            Instantiate(recipePrefabObject, recipesParent.transform).GetComponent<RecipeItemUI>().SetRecipeSO(waitingRecipeSO);
        }
    }
   
}
