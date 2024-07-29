using UnityEngine;

public class NotebookViewController : MonoBehaviour
{
    public GameObject view;

    void Start()
    {
        if (GameObject.Find("EventSystem") == null)
        {
            var eventSystem = Instantiate(Resources.Load("Prefabs/EventSystem"));
            eventSystem.name = "EventSystem";
        }
    }

    public void Hide()
    {
        view.SetActive(false);
    }

    public void Show()
    {
        view.SetActive(true);
    }
}
