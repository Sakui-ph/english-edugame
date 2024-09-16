
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AccountSelectHandler : MonoBehaviour
{
    public Transform accountListRoot;
    public GameObject accountButtonPrefab;
    public Button createNewAccount;

    void Start()
    {
        createNewAccount.onClick.AddListener(GameSystemSL.services.gameSystem.CreateNewCharacter);
    }
    public void SetupButtons(string[] accountNames)
    {
        foreach (string name in accountNames)
        {
            GameObject newButton = Instantiate(accountButtonPrefab, accountListRoot);
            Button button = newButton.GetComponent<Button>();
            TextMeshProUGUI buttonText = newButton.GetComponentInChildren<TextMeshProUGUI>();
            buttonText.text = name;

            button.onClick.AddListener(() => 
            {
                GameSystemSL.services.gameSystem.LoadPlayer(name);
                GameSystemSL.services.gameSystem.LoadMainMenu();
            });
        }
    }
}
