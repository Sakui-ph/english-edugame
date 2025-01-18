using System;
using Unity;
using Unity.VisualScripting;
using UnityEngine;

public class Draggable : MonoBehaviour {
    // NOTE: THIS IS DEVELOPED WITH THE FALLING DIRECTION AS POSITIVE Z
    [SerializeField] private Rigidbody rb;
    [SerializeField] private RaycastHit hit;

    public delegate void DragStateChangeDelegate(bool newState);
    public event DragStateChangeDelegate OnDragStateChangeEvent;
    private bool isDragging = false;
    public bool IsDragging {
        get {
            return isDragging;
        }
        set {
            if (value == isDragging)
                return;
            isDragging = value;
            if (OnDragStateChangeEvent != null) {
                OnDragStateChangeEvent.Invoke(IsDragging);
            }
        }
    }
    private bool isFalling;
    public bool IsFalling {
        get {
            return isFalling;
        }
        set {
            if (value == isFalling)
                return;
            isFalling = value;
        }
    }
    public GameObject parent;
    private float initialZ = 0;
    public float lift = 3.0f;
    public float fallingThreshold = 0;

    void Start() {
        rb = GetComponent<Rigidbody>();
        OnDragStateChangeEvent += OnDragStateChange;
    }

    void Update() {
        DetectMouseClick();
        DetectFalling();
    }

    void FixedUpdate()
    {
        HandleDragging();
    }

    private void HandleDragging() {
        if (IsDragging)
        {
            Vector3 targetPos;
            targetPos = hit.point;
            targetPos.z = initialZ + -lift;
            rb.position = targetPos;
            parent.transform.position = targetPos;
        }
    }
    

    private void DetectMouseClick() {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Physics.Raycast(ray, out hit);
        if (hit.rigidbody == rb) {
            if (Input.GetMouseButtonUp(0)) IsDragging = false;
            if (Input.GetMouseButtonDown(0)) IsDragging = true;
        } else IsDragging = false;
    }

    private void DetectFalling() {
        if (rb.velocity.z > fallingThreshold)
            IsFalling = true;
        else
            IsFalling = false;
    }

    private void OnDragStateChange(bool value) {
        if (value) {
            rb.useGravity = false;
            if (!isFalling)
                initialZ = hit.point.z;
        }
        else
            rb.useGravity = true;
    }
}
