using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChainButtonEffects : MonoBehaviour
{
    public GameObject flowers;
    public GameObject redChain;
    public GameObject pinkChain;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void CorrectAnimation(Action callback)
    {
        Quaternion flowersRotation = flowers.transform.rotation;
        RectTransform redRT = (RectTransform)redChain.transform;
        RectTransform pinkRT = (RectTransform)pinkChain.transform;

        LeanTween.rotateAround(flowers, Vector3.forward, 360f, 1f).setEaseInCirc();
        LeanTween.scale(flowers, Vector2.zero, 1f);
        LeanTween.scale(gameObject, Vector2.zero, 1f).setDestroyOnComplete(true);

        LockToPosition(redRT, 2f);
        LockToPosition(pinkRT, 2f, callback);
    }

    public void IncorrectAnimation(Action callback)
    {
        RectTransform flowersRotation = (RectTransform)flowers.transform;

        LeanTween.rotateAround(flowers, Vector3.forward, 180f, 1f).setEaseOutBack().setOnComplete(() => 
        {
            LeanTween.rotate(flowers, Vector3.zero, 0.5f).setEaseInBack();
        });

        Vector3 originalPosition = gameObject.transform.localPosition;

        LeanTween.moveLocalX(gameObject, originalPosition.x + 3f, 1f / 4).setEase(LeanTweenType.easeShake)
            .setLoopPingPong(2)
            .setOnComplete(() => transform.localPosition = originalPosition).setOnComplete(callback);
    }

    private void LockToPosition(RectTransform targetRect, float duration, Action callback = null)
    {
        Vector3 originalPosition = targetRect.anchoredPosition;

        // Move to (0, 0) over the specified duration
        LeanTween.move(targetRect, Vector2.zero, duration).setEaseOutBack().setOnComplete(callback);
    }
}
