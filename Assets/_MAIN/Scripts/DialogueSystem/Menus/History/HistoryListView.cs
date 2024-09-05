using System.Collections.Generic;
using System.Linq;
using DIALOGUE;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HistoryListView : MonoBehaviour
{
    public GameObject historyRoot;
    public GameObject historyLayout;
    public GameObject historyEntryPrefab;
    public Button menuButton;
    public Button backButton;
    public ScrollRect scrollRect;
    private string lastSpeaker = "";

    void Start()
    {
        menuButton.onClick.AddListener(OpenHistory);
        backButton.onClick.AddListener(CloseHistory);
        HistoryData.onAddLine += AddEntry;
    }

    private void AddEntry(DIALOGUE_LINE line)
    {
        GameObject historyEntry = Instantiate(historyEntryPrefab, historyLayout.transform);
        TextMeshProUGUI tmp = historyEntry.GetComponentInChildren<TextMeshProUGUI>();
        List<string> segmentDialogue = new();

        foreach (var segment in line.dialogueData.segments)
        {
            segmentDialogue.Add(segment.dialogue);
        }

        if (line.hasSpeaker)
        {
            lastSpeaker = line.speakerData.castName == "" ? line.speakerData.name : line.speakerData.castName;
        }

        tmp.text = $"{lastSpeaker}: {string.Join("", segmentDialogue)}";
    }

    public void OpenHistory()
    {
        CanvasGroupControl.ShowCanvasGroup(historyRoot);

        scrollRect.verticalNormalizedPosition = 0;
        Canvas.ForceUpdateCanvases();
    }

    public void CloseHistory()
    {
        CanvasGroupControl.HideCanvasGroup(historyRoot);
    }
}