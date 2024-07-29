using UnityEngine;
using UnityEngine.UI;


public static class TweenColorChange
{
    // Start is called before the first frame update
    private static Image currentImage;
    public static void ChangeColor(GameObject gameObject, Color newColor, float duration, LeanTweenType easing = LeanTweenType.linear)
    {
        Image image = gameObject.GetComponent<Image>();
        currentImage = image;
        LeanTween.value(gameObject, setColorCallback, image.color, newColor, duration).setEase(easing);
    }
    private static void setColorCallback(Color c)
    {
            currentImage.color = c;

            var tempColor = currentImage.color;
            tempColor.a = 1f;
            currentImage.color = tempColor;
    }
}
