using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatesCounterVisual : MonoBehaviour {
    [SerializeField] private PlatesCounter platesCounter;
    [SerializeField] private Transform counterTopPoint;
    [SerializeField] private Transform plateVisualPrefab;
    
    private List<GameObject> platesList;
    
    private void Start() {
        platesCounter.OnPlateSpawned += PlatesCounter_OnPlateSpawned;
        platesCounter.OnPlateTaken += PlatesCounter_OnPlateTaken;
    }
    
    private void Awake() {
        platesList = new List<GameObject>();
    }
    
    private void PlatesCounter_OnPlateSpawned(object sender, System.EventArgs e) {
        float plateOffsetY = 0.1f * platesList.Count;
        Transform plateVisualTransform = Instantiate(plateVisualPrefab, counterTopPoint);
        plateVisualTransform.localPosition = new Vector3(0, plateOffsetY, 0);
        platesList.Add(plateVisualTransform.gameObject);
    }
    
    private void PlatesCounter_OnPlateTaken(object sender, System.EventArgs e) {
        GameObject topPlate = platesList[platesList.Count - 1];
        platesList.Remove(topPlate);
        Destroy(topPlate);
    }

}
