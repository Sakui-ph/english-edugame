using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GarbageHolder : MonoBehaviour
{
    public static GarbageHolder instance = null;
    
    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);
    }
}
