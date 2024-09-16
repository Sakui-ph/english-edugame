using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ANImage : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        CanvasGroup cg = GetComponent<CanvasGroup>();

        cg.alpha = 0;
        LeanTween.alphaCanvas(cg, 1, 1).setEaseInCubic().setOnComplete(() => {

            LeanTween.alphaCanvas(cg, 0, 1).setDelay(2f).setOnComplete(() => 
            {
                GameSystemSL.services.gameSystem.CheckForPlayerList();
            });
        });
    }



}
