using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;
# if UNITY_EDITOR
using UnityEditor;
#endif

namespace PLAYER_INFORMATION
{
    [ExecuteInEditMode()]
    public class RadialBar : MonoBehaviour
    {
#if UNITY_EDITOR
        [MenuItem("GameObject/UI/Radial Progress Bar")]
        public static void AddRadialProgressBar()
        {
            GameObject obj = Instantiate(Resources.Load<GameObject>("Radial Progress Bar"));
            obj.transform.SetParent(Selection.activeGameObject.transform, false);
        }

#endif
        public float min;
        public float max;
        public float current;
        public Image mask;
        public Image fill;
        public Color color;

        public void AssignValues(float min, float max, float current) {
            this.min = min;
            this.max = max;
            this.current = current;
        }

        public void SetCurrent(float current) {
            this.current = current;
        }

        void Update()
        {
            GetCurrentFill();
        }

        void GetCurrentFill()
        {
            float currentOffset = current - min;
            float maxOffset = max - min;
            float fillAmount = (float)current / (float)max;
            mask.fillAmount = fillAmount;
            fill.color = color;
        }
    }
}