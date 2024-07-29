using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public GameObject tutorialRoot;
    private List<GameObject> popUps = new List<GameObject>();
    private int popUpIndex = 0;
    private bool enabledPopUps = false;

    void Awake()
    {
        var transform = tutorialRoot.transform;
        for (int i = 0; i < transform.childCount; i++)
        {
            popUps.Add(transform.GetChild(i).gameObject);
        }
    }

    void DisableAllPopUps()
    {
        enabledPopUps = false;
        // Loop through all pop-ups to set their active state to false
        foreach (GameObject popUp in popUps)
        {
            popUp.SetActive(false);
        }
    }

    public void StartPopUps()
    {
        // Reset the popUpIndex to start from the first pop-up
        popUpIndex = 0;
        enabledPopUps = true;

        // Enable the first pop-up
        if (popUps.Count > 0)
        {
            popUps[0].SetActive(true);
        }
    }

    void Update()
    {
        // Check if the left mouse button is pressed
        if (Input.GetMouseButtonDown(0) && enabledPopUps)
        {
            // Increment the popUpIndex
            popUpIndex++;

            // If the popUpIndex exceeds the length of the popUps array, disable all pop-ups
            if (popUpIndex >= popUps.Count)
            {
                // Disable all pop-ups
                DisableAllPopUps();
                if (Time.timeScale != 1)
                    Time.timeScale = 1;
            }
            else
            {
                // If the popUpIndex is within the bounds of the popUps array, set the active state based on the popUpIndex
                for (int i = 0; i < popUps.Count; i++)
                {
                    popUps[i].SetActive(i == popUpIndex);
                }
            }
        }
    }

    public void NextPopup() 
    {
        popUpIndex++;
    }
}
