using UnityEngine;

public class Breathing : MonoBehaviour
{
    bool max = false;
    bool animating = false;
    public float maxScale = 1.5f;
    // Update is called once per frame
    void Update()
    {
        if (!max && !animating)
        {
            animating = true;
            LeanTween.scale(gameObject, new Vector3(maxScale, maxScale, maxScale), 1f).setEaseInOutBack().setOnComplete(() => {
                Reset();
            });
        }
        else if (max && !animating) {
            animating = true;
            LeanTween.scale(gameObject, new Vector3(1, 1, 1), 1f).setEaseInOutBack().setOnComplete(() => {Reset();});
        }
            
    }

    private void Reset()
    {
        max = !max;
        animating = false;
    }
}
