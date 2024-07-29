using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floating : MonoBehaviour
{
    public float floatHeight = 10f; // Height of the floating motion
    public float floatDuration = 2.0f; // Duration for one complete float cycle

    private Vector3 startPosition;

    void Start()
    {
        startPosition = transform.position;
        StartFloating();
    }

    void StartFloating()
    {
        // Start floating upwards
        LeanTween.moveY(gameObject, startPosition.y + floatHeight, floatDuration)
                 .setEase(LeanTweenType.easeInOutSine)
                 .setLoopPingPong();
    }
}
