
using UnityEngine.UI;
using UnityEngine;

public class TestResources : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Image img = GetComponent<Image>();
        Sprite test = Resources.Load<Sprite>("Test/inchikan_front") ;
        Debug.Log(test);
        img.sprite = test;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
