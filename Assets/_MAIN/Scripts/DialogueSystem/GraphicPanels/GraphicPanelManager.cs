using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GRAPHIC_PANELS
{
    public class GraphicPanelManager : MonoBehaviour
    {
        public const float DEFAULT_TRANSITION_SPEED = 1f;
        public static GraphicPanelManager instance {get; private set;}
        [SerializeField] private GraphicPanel[] allPanels;
    
    
        private void Awake() {
            if(instance == null){
                instance = this;
            }else{
                Destroy(gameObject);
            }
        }
    
        public GraphicPanel GetPanel(string panelName)
        {
            panelName = panelName.ToLower();
            foreach (var panel in allPanels)
            {
                if(panel.panelName.ToLower() == panelName){
                    return panel;
                }
            }
            Debug.LogWarning("No panel with name: " + panelName);
            return null;
        }
    }
}
