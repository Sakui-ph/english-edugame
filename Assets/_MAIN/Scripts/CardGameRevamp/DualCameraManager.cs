using UnityEngine;

public class DualCameraManager : MonoBehaviour
{
    [Header("Camera References")]
    public Camera orthographicCamera;  // 2D Camera
    public Camera perspectiveCamera;   // 3D Camera
    
    [Header("Layer Settings")]
    public LayerMask orthographicLayers;  // Layers for 2D elements
    public LayerMask perspectiveLayers;   // Layers for 3D elements
    
    [Header("Camera Settings")]
    [Range(0, 20)] public float orthographicSize = 5f;
    [Range(1, 179)] public float perspectiveFOV = 60f;
    public float perspectiveCameraDepth = -10f;  // Should be lower than ortho camera
    
    void Start()
    {
        if (!orthographicCamera || !perspectiveCamera)
        {
            Debug.LogError("Both cameras must be assigned!");
            return;
        }
        
        SetupCameras();
    }
    
    void SetupCameras()
    {
        // Setup Orthographic Camera (2D)
        orthographicCamera.orthographic = true;
        orthographicCamera.orthographicSize = orthographicSize;
        orthographicCamera.cullingMask = orthographicLayers;
        orthographicCamera.depth = 0;  // Renders first
        orthographicCamera.clearFlags = CameraClearFlags.SolidColor;
        
        // Setup Perspective Camera (3D)
        perspectiveCamera.orthographic = false;
        perspectiveCamera.fieldOfView = perspectiveFOV;
        perspectiveCamera.cullingMask = perspectiveLayers;
        perspectiveCamera.depth = perspectiveCameraDepth;
        perspectiveCamera.clearFlags = CameraClearFlags.Depth;  // Only renders depth, preserves color
        
        // Position cameras
        perspectiveCamera.transform.position = orthographicCamera.transform.position;
        perspectiveCamera.transform.rotation = orthographicCamera.transform.rotation;
    }
    
    // Optional: Method to follow a target (like a player)
    public void FollowTarget(Transform target, Vector3 offset)
    {
        if (target == null) return;
        
        Vector3 targetPosition = target.position + offset;
        orthographicCamera.transform.position = new Vector3(
            targetPosition.x,
            targetPosition.y,
            orthographicCamera.transform.position.z
        );
        
        perspectiveCamera.transform.position = new Vector3(
            targetPosition.x,
            targetPosition.y,
            perspectiveCamera.transform.position.z
        );
    }
}