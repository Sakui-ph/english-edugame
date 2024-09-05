using DIALOGUE;
using UnityEngine;

public class VisualNovelSL : MonoBehaviour
{
    public static VisualNovelSL services            {get; private set;}

    public DialogueSystem dialogueSystem            {get; private set;}
    public PlayerInputManager playerInputManager    {get; private set;}
    public TitleCardController titleCardController  {get; private set;}
    public TutorialManager tutorialManager          {get; private set;}
    public VisualNovelViewController viewController {get; private set;}
    public HistoryManager historyManager            {get; private set;}



    void Awake()
    {
        if (services != null)
        {
            Destroy(this.gameObject);
            return;
        }
        services = this;
        Initialize();
    }

    private void Initialize()
    {
        dialogueSystem =        GetComponentInChildren<DialogueSystem>();
        playerInputManager =    GetComponentInChildren<PlayerInputManager>();
        titleCardController =   GetComponentInChildren<TitleCardController>();
        tutorialManager =       GetComponentInChildren<TutorialManager>();
        viewController =        GetComponentInChildren<VisualNovelViewController>();
        historyManager =        GetComponentInChildren<HistoryManager>();
    }
}
