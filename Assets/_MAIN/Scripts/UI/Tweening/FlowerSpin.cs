using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowerSpin : MonoBehaviour
{
    bool animating = false;
    // Update is called once per frame

    void Start()
    {
        RectTransform rt = (RectTransform)transform;
        LeanTween.scale(rt, Vector2.zero, 0).setOnComplete(() => {
            animating = true;
            LeanTween.rotate(gameObject,  transform.rotation * new Vector3(0, 0, 720), Random.Range(10f, 20f)).setEaseInOutBack().setOnComplete(() => animating = false);
        });
        LeanTween.scale(rt, Vector2.one, 3);
    }

    void Update()
    {
        if (Time.frameCount % 200 == 0 && !animating)
            if (Random.Range(1, 100) > 74)
            {
                int rotationValue = Random.Range(720, 2024);
                if (Random.Range(1, 2) == 2)
                    rotationValue *= -1;
                Vector3 targetRotation = transform.rotation * new Vector3(0, 0, rotationValue);

                animating = true;
                LeanTween.rotate(gameObject, targetRotation, Random.Range(10f, 20f)).setEaseInOutBack().setOnComplete(() => animating = false);
            }
                
    }
}
