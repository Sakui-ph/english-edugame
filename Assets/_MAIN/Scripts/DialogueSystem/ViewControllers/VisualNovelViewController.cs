using System;
using UnityEngine;
using UnityEngine.UI;


public class VisualNovelViewController : MonoBehaviour
{
    public GameObject dialogue;
    public GameObject controls;
    public Image weekday;
    CanvasGroup cg => weekday.gameObject.GetComponent<CanvasGroup>();
    private DaysOfTheWeek currentDay = DaysOfTheWeek.Monday;
    private bool isFocusMode = false;

    void Awake()
    {
        cg.alpha = 0;
        weekday.gameObject.SetActive(false);
    }

    void Start()
    {
        if (GameObject.Find("EventSystem") == null)
        {
            var eventSystem = Instantiate(Resources.Load("Prefabs/EventSystem"));
            eventSystem.name = "EventSystem";
        }
            
    }

    public void SetWeekday(DaysOfTheWeek day)
    {
        weekday.gameObject.SetActive(true);
        currentDay = day;
        

        weekday.sprite = Resources.Load<Sprite>($"WeekdayGraphics/{currentDay.ToString()}");
        cg.alpha = 0;
        LeanTween.alphaCanvas(cg, 1, 1);
    }

    public void Hide()
    {
        controls.SetActive(false);
        dialogue.SetActive(false);
    }

    public void Show()
    {
        controls.SetActive(true);
        dialogue.SetActive(true);
    }

    public void SetFocusMode(bool setting)
    {
        isFocusMode = setting;
        controls.SetActive(!isFocusMode);
    }

}

public enum DaysOfTheWeek
{
    Monday, Tuesday, Wednesday, Thursday, Friday, Saturday, Sunday
}
