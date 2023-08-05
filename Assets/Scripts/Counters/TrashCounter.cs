using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TrashCounter : BaseCounter {
    public static EventHandler OnTrash;
    public override void Interact(Player player) {
        if (player.HasKitchenObject()) {
            OnTrash?.Invoke(this, EventArgs.Empty);
            player.GetKitchenObject().DestroySelf();
        }
    }
}
