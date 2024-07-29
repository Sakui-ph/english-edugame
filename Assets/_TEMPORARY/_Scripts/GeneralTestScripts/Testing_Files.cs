using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TESTING 
{
    public class Testing_Files : MonoBehaviour
    {
        public TextAsset textAsset;
        // Start is called before the first frame update
        void Start()
        {
            StartCoroutine(Run());
        }

        IEnumerator Run()
        {
            // checking for asset right now
            List<string> lines = FileManager.ReadTextAsset(textAsset, false);
            yield return null;
        }
    }
}

