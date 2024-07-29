using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftToRight : MonoBehaviour
{
    public float distance = 2f;
    public float duration = 0.5f;
    public bool flip = false;
    // Start is called before the first frame update
    void Start()
    {
        if (flip)
            distance *= -1;
        Vector3 position = transform.position;
        LeanTween.moveX(gameObject, position.x + distance, duration).setEaseInBack().setLoopPingPong();
    }
}
