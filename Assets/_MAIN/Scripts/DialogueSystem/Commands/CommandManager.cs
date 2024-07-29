// NOTE: MOST OF THE CODE FROM THE DIALOGUE SYSTEM IS FROM STELLAR STUDIO'S YOUTUBE CHANNEL TUTORIAL
// CHECK THEM OUT HERE: https://www.youtube.com/@stellarstudio5495

using UnityEngine;
using System.Reflection; // lets us use assembly functions
using System.Linq;
using System;
using System.Collections; // for querying

namespace COMMANDS{
    public class CommandManager : MonoBehaviour
{
    public static CommandManager instance { get; private set; }
    private static Coroutine process = null;
    public static bool isRunningProcess => process != null; 
    private CommandDatabase commandDatabase;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            commandDatabase = new CommandDatabase();
            Assembly assembly = Assembly.GetExecutingAssembly();

            // find every type of database extension and extend the command database with it
            Type[] extensionTypes = assembly.GetTypes().Where(t => t.IsSubclassOf(typeof(CMD_DatabaseExtension))).ToArray();

            foreach(Type extension in extensionTypes)
            {
                MethodInfo extendMethod = extension.GetMethod("Extend");
                extendMethod.Invoke(null, new object[] { commandDatabase });
            }
        }   
        else
            DestroyImmediate(gameObject);
    }
    
    public Coroutine Execute(string commandName, params string[] args)
    {
        Delegate command = commandDatabase.GetCommand(commandName);
        if (command == null)
            return null;

        return StartProcess(commandName, command, args);
    }

    private Coroutine StartProcess(string commandName, Delegate command, string[] args)
    {
        StopCurrentProcess();

        process = StartCoroutine(RunningProcess(command, args));
        return process;
    }

    private void StopCurrentProcess()
    {
        if (process != null)
            StopCoroutine(process);
        process = null;
    }

    private IEnumerator RunningProcess(Delegate command, string[] args)
    {
        yield return WaitingForProcessCompletion(command, args);
        process = null;
    }

    private IEnumerator WaitingForProcessCompletion(Delegate command, string[] args)
    {
        if (command is Action)
            command.DynamicInvoke();

        else if (command is Action<string>)
            command.DynamicInvoke(args[0]);

        else if (command is Action<string[]>)
            command.DynamicInvoke((object)args);

        else if (command is Func<IEnumerator>)
            yield return ((Func<IEnumerator>)command)(); // parenthesis at the end to call the delegate
        
        else if (command is Func<string, IEnumerator>)
            yield return ((Func<string, IEnumerator>)command)(args[0]);

        else if (command is Func<string[], IEnumerator>)
            yield return ((Func<string[], IEnumerator>)command)(args); // doesn't need to convert to object

        else
            Debug.LogError($"Command {command.Method.Name} is not a valid command type");
    }
}
}