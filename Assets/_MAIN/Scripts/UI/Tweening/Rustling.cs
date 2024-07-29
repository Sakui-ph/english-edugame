using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rustling : MonoBehaviour
{
    public float rustleAmount = 5f;     // Maximum rustle in degrees
    public float rustleDuration = 0.5f; // Duration of each rustle
    public float intervalMin = 1f;      // Minimum interval between rustles
    public float intervalMax = 3f;      // Maximum interval between rustles

    private void Start()
    {
        StartRustling();
    }

    private void StartRustling()
    {
        float randomAngle = Random.Range(-rustleAmount, rustleAmount);
        float randomInterval = Random.Range(intervalMin, intervalMax);

        LeanTween.rotateZ(gameObject, randomAngle, rustleDuration)
            .setEaseInOutSine()
            .setOnComplete(() =>
            {
                LeanTween.rotateZ(gameObject, 0f, rustleDuration)
                    .setEaseInOutSine()
                    .setDelay(randomInterval)
                    .setOnComplete(StartRustling);
            });
    }
}
