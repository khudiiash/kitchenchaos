using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IKitchenObjectParent {
    public static Player Instance { get; private set; }

    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private GameInput gameInput;
    [SerializeField] private LayerMask countersLayerMask; 
    [SerializeField] private Transform kitchenObjectHoldPoint; 
    
    private KitchenObject kitchenObject;


    public event EventHandler OnPickup;
    public event EventHandler<OnSelectedCounterChangedEventArgs> OnSelectedCounterChanged; 
    public class OnSelectedCounterChangedEventArgs : EventArgs {
        public BaseCounter selectedCounter;
    }

    private Vector3 lastInteractDir;
    private float PLAYER_HEIGHT = 2f;
    private float PLAYER_RADIUS = 0.7f;
    private float INTERACT_DIST = 2f;
    private BaseCounter selectedCounter;
    
    private bool isWalking;
    
    private void Awake() {
        if (Instance != null) {
           Debug.Log("There is more than one player instance"); 
        }
        Instance = this;
    }

    private void Start() {
       gameInput.OnInteractAction += GameInput_OnInteractAction;
       gameInput.OnInteractAlternateAction += GameInput_OnInteractAlternateAction;
    }
    
    private void Update() {
        if (!GameManager.Instance.IsGamePlaying()) return;
        HandleMovement();
        HandleInteractions();
    }   
    
    private void GameInput_OnInteractAction(object sender, System.EventArgs e) {
        if (!GameManager.Instance.IsGamePlaying()) return;
        selectedCounter?.Interact(this);
    }
    
    private void GameInput_OnInteractAlternateAction(object sender, System.EventArgs e) {
        if (!GameManager.Instance.IsGamePlaying()) return;
        selectedCounter?.InteractAlternate(this);
    }
    
    private void HandleInteractions() {
        Vector2 inputVector = gameInput.GetMovementVectorNormalized(); 
        Vector3 moveDir = new Vector3(inputVector.x, 0, inputVector.y);

        if (moveDir != Vector3.zero) {
            lastInteractDir = moveDir;
        }
        var interact = Physics.Raycast(transform.position, lastInteractDir, out RaycastHit raycastHit, INTERACT_DIST, countersLayerMask);
        if (interact) {
            if (raycastHit.transform.TryGetComponent(out BaseCounter baseCounter)) {
                if (baseCounter != selectedCounter) {
                    SetSelectedCounter(baseCounter);
                }
            } else if (selectedCounter != null) {
                selectedCounter = null;
                SetSelectedCounter(null);

            }
        } else if (selectedCounter != null) {
            selectedCounter = null;
            SetSelectedCounter(null);
        } 
    }
    
    
    private void HandleMovement() {
        Vector2 inputVector = gameInput.GetMovementVectorNormalized(); 

        Vector3 moveDir = new Vector3(inputVector.x, 0, inputVector.y);
        float moveDistance = moveSpeed * Time.deltaTime;
        bool canMove = CanMoveInDirection(moveDir, moveDistance);

        if (!canMove) {
           // Slide along the wall
           Vector3 moveDirX = new Vector3(moveDir.x, 0, 0).normalized;
           Vector3 moveDirZ = new Vector3(0, 0, moveDir.z).normalized;
           bool canMoveX = moveDir.x != 0 && CanMoveInDirection(moveDirX, moveDistance);
           bool canMoveZ = moveDir.z != 0 && CanMoveInDirection(moveDirZ, moveDistance);
           if (canMoveX) moveDir = moveDirX;
           if (canMoveZ) moveDir = moveDirZ;
           canMove = canMoveX || canMoveZ;
        } 

        if (canMove) {
            transform.position += moveDir * moveDistance;
        }
        isWalking = moveDir != Vector3.zero;
        float rotateSpeed = 10f;
        transform.forward = Vector3.Slerp(transform.forward, moveDir, Time.deltaTime * rotateSpeed);
    }
    
    private bool CanMoveInDirection(Vector3 dir, float moveDistance) {
        return !Physics.CapsuleCast( transform.position, transform.position + Vector3.up * PLAYER_HEIGHT, PLAYER_RADIUS, dir, moveDistance);
    }
    
    public bool IsWalking() {
       return isWalking; 
    }
    
    private void SetSelectedCounter(BaseCounter selectedCounter) {
        this.selectedCounter = selectedCounter;

        OnSelectedCounterChanged?.Invoke(this, new OnSelectedCounterChangedEventArgs {
            selectedCounter = selectedCounter
        });
    }
    
    public Transform GetKitchenObjectFollowTransform() {
        return kitchenObjectHoldPoint;
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
            OnPickup?.Invoke(this, EventArgs.Empty);
        }
    }
    
    public bool HasKitchenObject() {
        return this.kitchenObject != null;
    }
    
}
