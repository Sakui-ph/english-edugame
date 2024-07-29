using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swaying : MonoBehaviour
{
    public float swayAmount = 10f; // Maximum sway in degrees
    public float swaySpeed = 2f;   // Base speed of the sway
    public float randomAmount = 2f; // Randomness in sway amount
    public float randomSpeed = 0.5f; // Randomness in sway speed

    private void Start()
    {
        StartSwaying();
    }

    private void StartSwaying()
    {
        float randomSway = swayAmount + Random.Range(-randomAmount, randomAmount);
        float randomDuration = swaySpeed + Random.Range(-randomSpeed, randomSpeed);

        LeanTween.rotateZ(gameObject, randomSway, randomDuration)
            .setEaseInOutSine()
            .setLoopPingPong()
            .setOnComplete(StartSwaying);
    }
}
