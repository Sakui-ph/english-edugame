using System;
using System.Collections;
using UnityEngine;
using COMMANDS;
// example thing for the command database extension abstract class
public class CMD_DatabaseExtension_Examples : CMD_DatabaseExtension
{
    // the 'new' keyword hides the inherited method from the base class
    new public static void Extend(CommandDatabase database)
    {
        // add Action with no parameters
        database.AddCommand("printdefault", new Action(PrintDefaultMessage));
        database.AddCommand("print_1p", new Action<string>(PrintUserMessage));
        database.AddCommand("print_mp", new Action<string[]>(PrintLines));

        // add lambda with no parameters
        database.AddCommand("lambda", new Action(() => 
        {
            Debug.Log("This is the default message lambda");
        }));

        database.AddCommand("lambda_1p", new Action<string>((arg) => 
        {
            Debug.Log("Message Lambda: " + arg);
        }));

        database.AddCommand("lambda_mp", new Action<string[]>((args) => 
        {
            Debug.Log(string.Join(", ", args));
        }));

        // add coroutines with no parameters
        database.AddCommand("process", new Func<IEnumerator>(SimpleProcess));
        database.AddCommand("process_1p", new Func<string, IEnumerator>(LineProcess));
        database.AddCommand("process_mp", new Func<string[], IEnumerator>(ProcessWithArgs));

    }

    private static void PrintDefaultMessage()
    {
        Debug.Log("This is the default message");
    }

    private static void PrintUserMessage(string message)
    {
        Debug.Log("User Message: " + message);
    }

    private static void PrintLines(string[] lines)
    {   
        int i = 1;
        foreach (string line in lines)
            Debug.Log($"{i++}. '{line}'");
    }

    private static IEnumerator SimpleProcess()
    {
        for(int i = 1; i < 5; i++)  
        {
            Debug.Log($"Process Running... {i}");
            yield return new WaitForSeconds(1f);
        }
    }
    
    private static IEnumerator LineProcess(string data)
    {
        if (int.TryParse(data, out int lines))
        {
            for(int i = 1; i <= lines; i++)  
            {
                Debug.Log($"Process Running... {i}");
                yield return new WaitForSeconds(1f);
            }
        }
    }

    private static IEnumerator ProcessWithArgs(string[] args)
    {
        for(int i = 0; i < args.Length; i++)  
        {
            Debug.Log($"Process Running... {args[i]}");
            yield return new WaitForSeconds(1f);
        }
    }
}
