using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DIALOGUE;

namespace TESTING 
{
    public class Testing_Parsing : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            string line = "Speaker \"Dialogue \\\"Goes\\\" In Here!\" PlaySong(\"Minecraft Gamer\" -v 10 -p 0.3)";
            DIALOGUE_LINE parsedLine = DialogueParser.Parse(line);

        }

        // Update is called once per frame
        void Update()
        {
            
        }
    }
}
