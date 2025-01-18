using UnityEngine;
public class CardGameSL : MonoBehaviour 
{
    public static CardGameSL services;

    public DualCameraManager dualCameraManager;
    
    void Awake()
    {
        if (services != null)
        {

            Destroy(this.gameObject);
            return;
        }
        services = this;
        Initialize();
    }

    public void Initialize()
    {
        dualCameraManager = GetComponentInChildren<DualCameraManager>();
    }
} 
