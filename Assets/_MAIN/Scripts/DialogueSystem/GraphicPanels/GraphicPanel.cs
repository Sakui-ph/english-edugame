using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GRAPHIC_PANELS
{
    [System.Serializable]
    public class GraphicPanel 
    {
        public string panelName;
        public GameObject rootPanel;
        private List<GraphicLayer> layers = new List<GraphicLayer>();
    
        // See if we have a layer we can render it on
        public GraphicLayer GetLayer(int layerDepth, bool createIfDoesNotExist = true)
        {
            for (int i = 0; i < layers.Count; i++)
            {
                if (layers[i].layerDepth == layerDepth)
                {
                    return layers[i];
                }
            }
    
            if (createIfDoesNotExist)
            {
                return CreateLayer(layerDepth);
            }
            return null;
        }
    
    
         // Create a layer for the panel to render on
        private GraphicLayer CreateLayer(int layerDepth)
        {
            GraphicLayer layer = new GraphicLayer();
            GameObject panel = new GameObject(string.Format(GraphicLayer.LAYER_OBJECT_NAME_FORMAT, layerDepth));
            RectTransform rect = panel.AddComponent<RectTransform>();
            panel.AddComponent<CanvasGroup>();
            panel.transform.SetParent(rootPanel.transform, false);
    
            // Scale to size of parent panel aka size of parent UI
            rect.anchorMin = Vector2.zero;
            rect.anchorMax = Vector2.one;
            rect.offsetMin = Vector2.zero;
            rect.offsetMax = Vector2.one;
    
            layer.panel = panel.transform;
            layer.layerDepth = layerDepth;
    
            int index = layers.FindIndex(l => l.layerDepth > layerDepth);
            if (index == -1)
            {
                layers.Add(layer);
            }
            else
            {
                layers.Insert(index, layer);
            }
    
            for (int i = 0; i < layers.Count; i++)
            {
                layers[i].panel.SetSiblingIndex(layers[i].layerDepth);
            }
    
            return layer;
        }

        public void Clear(float speed = 1f, Texture blendTexture = null, bool immedate = false)
        {
            foreach (var layer in layers)
                layer.Clear(speed, blendTexture);
        }
    }
    
}