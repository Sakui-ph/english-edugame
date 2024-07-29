using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GRAPHIC_PANELS;
using UnityEngine.UI;

public class TestGraphicLayers : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Running());

    }

    IEnumerator Running() {
        Debug.Log(GraphicPanelManager.instance.GetPanel("Background").panelName);
        GraphicPanel panel = GraphicPanelManager.instance.GetPanel("Background");
        GraphicLayer layer = panel.GetLayer(0, true);

        yield return new WaitForSeconds(1);


        layer.SetTexture("Backgrounds/classroom");

        yield return new WaitForSeconds(1);

        Texture blendTex = Resources.Load<Texture>("Graphics/Transition Effects/feathers");
        layer.SetTexture("Backgrounds/lungmen", blendingTexture: blendTex);
        yield return new WaitForSeconds(1);

        layer.currentGraphic.FadeOut();

        yield return new WaitForSeconds(1);
        

        layer.SetVideo("Graphics/BG Videos/Fantasy Landscape", blendingTexture: blendTex);

        yield return new WaitForSeconds(1);

        layer.currentGraphic.FadeOut();
    }

}
