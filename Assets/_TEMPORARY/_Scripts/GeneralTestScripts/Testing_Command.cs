using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using COMMANDS;

namespace TESTING 
{
    public class Testing_Command : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            StartCoroutine(Running());
        }


        IEnumerator Running()
        {
            yield return CommandManager.instance.Execute("printdefault");
            yield return CommandManager.instance.Execute("print_1p", "Hello World!");
            yield return CommandManager.instance.Execute("print_mp", "Hello", "World!", "How", "Are", "You?");

            yield return CommandManager.instance.Execute("lambda");
            yield return CommandManager.instance.Execute("lambda_1p", "Hello World!");
            yield return CommandManager.instance.Execute("lambda_mp", "Hello", "World!", "How", "Are", "You?");

            yield return CommandManager.instance.Execute("process");
            yield return CommandManager.instance.Execute("process_1p", "5");
            yield return CommandManager.instance.Execute("process_mp", "Hello", "World!", "How", "Are", "You?");
        }

        // Update is called once per frame
        void Update()
        {
            
        }
    }
}

