using UnityEngine;
public class DeliveryCounter : BaseCounter {
  
  static public DeliveryCounter Instance { get; private set; }
  
  private void Awake() {
    Instance = this;
  }

  public override void Interact(Player player)
  {
      if (player.HasKitchenObject()) {
        if (player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject)) {
            DeliveryManager.Instance.DeliverRecipe(plateKitchenObject);
            player.GetKitchenObject().DestroySelf();
        } 
      }
  }
}
