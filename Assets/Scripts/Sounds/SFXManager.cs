using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXManager : MonoBehaviour {
    
    public static SFXManager Instance { get; private set; }
    
    [SerializeField] private AudioClipRefsSO audioClipRefsSO;
    
    private void Start() {
        DeliveryManager.Instance.OnRecipeSuccess += RecipeSuccess;
        DeliveryManager.Instance.OnRecipeFailed += RecipeFailed;
        CuttingCounter.OnAnyCut += Chop;
        BaseCounter.OnAnyObjectPlaced += Drop;
        Player.Instance.OnPickup += Pickup;
        TrashCounter.OnTrash += Trash;
    }
    
    private void Awake() {
        Instance = this;
    }
    
    private void Trash(object sender, System.EventArgs e) {
        TrashCounter trashCounter = (TrashCounter)sender;
        PlaySound(audioClipRefsSO.trash, trashCounter.transform.position);
    }
    
    private void Drop(object sender, System.EventArgs e) {
        BaseCounter baseCounter = (BaseCounter)sender;
        PlaySound(audioClipRefsSO.objectDrop, baseCounter.transform.position);
    }
    
    private void Chop(object sender, System.EventArgs e) {
        CuttingCounter cuttingCounter = (CuttingCounter)sender;
        PlaySound(audioClipRefsSO.chop, cuttingCounter.transform.position);
    }
    
    private void Pickup(object sender, System.EventArgs e) {
        PlaySound(audioClipRefsSO.objectPickup, Player.Instance.transform.position);
    }
    
    private void RecipeSuccess(object sender, System.EventArgs e) {
        PlaySound(audioClipRefsSO.deliverySuccess, DeliveryCounter.Instance.transform.position);
    }
    
    private void RecipeFailed(object sender, System.EventArgs e) {
        PlaySound(audioClipRefsSO.deliveryFail, DeliveryCounter.Instance.transform.position);
    }
    private void PlaySound(AudioClip[] audioClipsArray, Vector3 position, float volume = 1f) {
        PlaySound(audioClipsArray[Random.Range(0, audioClipsArray.Length)], position, volume);
    }
    
    private void PlaySound(AudioClip audioClip, Vector3 position, float volume = 1f) {
        AudioSource.PlayClipAtPoint(audioClip, position, volume);
    }
    
    public void PlayFootsteps() {
        PlaySound(audioClipRefsSO.footsteps, Player.Instance.transform.position);
    }
}

