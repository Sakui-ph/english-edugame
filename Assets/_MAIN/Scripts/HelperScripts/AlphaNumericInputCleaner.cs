using System.Linq;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(TMP_InputField))]
public class AlphaNumericInputCleaner : MonoBehaviour
{
    [SerializeField] TMP_InputField inputField;

    void OnEnable()
    {
        //Register InputField Event
        inputField.onValueChanged.AddListener(inputValueChanged);
    }


    static string CleanInput(string input)
    {
        var cleanedString = input.Where(item => char.IsLetterOrDigit(item));
        return new string (cleanedString.ToArray());

    }

    //Called when Input changes
    void inputValueChanged(string attemptedVal)
    {
        inputField.text = CleanInput(attemptedVal);
    }

    void OnDisable()
    {
        //Un-Register InputField Events
        inputField.onValueChanged.RemoveAllListeners();
    }
}