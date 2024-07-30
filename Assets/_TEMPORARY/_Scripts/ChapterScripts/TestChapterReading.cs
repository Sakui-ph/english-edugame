using UnityEngine;
using DIALOGUE;

namespace TESTING
{
    public class TestChapterReading : MonoBehaviour
    {
        DialogueSystem ds => DialogueSystem.instance;
        
        void Start()
        {
            ds.LoadChapter("level10_cg");
        }
    }


}