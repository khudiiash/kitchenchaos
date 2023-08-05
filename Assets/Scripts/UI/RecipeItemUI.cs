using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class RecipeItemUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI descriptionText;
    [SerializeField] private Transform iconsParent;
    [SerializeField] private Transform iconPrefab;

    
    public void SetRecipeSO(RecipeSO recipeSO) {
        gameObject.SetActive(true);
        titleText.text = recipeSO.recipeName;
        descriptionText.text = recipeSO.recipeDescription;
        
        foreach (KitchenObjectSO kitchenObjectSO in recipeSO.kitchenObjectList) {
            Transform iconTransform = Instantiate(iconPrefab, iconsParent);
            iconTransform.GetComponent<Image>().sprite = kitchenObjectSO.sprite;
            iconTransform.gameObject.SetActive(true);
        }
    }
}
