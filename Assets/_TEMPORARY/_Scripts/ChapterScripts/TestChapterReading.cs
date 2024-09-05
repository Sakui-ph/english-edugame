using UnityEngine;
using DIALOGUE;

namespace TESTING
{
    public class TestChapterReading : MonoBehaviour
    {
        DialogueSystem ds => VisualNovelSL.services.dialogueSystem;
        
        void Start()
        {
            ds.LoadBranch("level10_cg");
        }
    }


}