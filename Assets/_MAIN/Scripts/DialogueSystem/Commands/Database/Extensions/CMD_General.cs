using System;
using System.Collections;
using UnityEngine;

namespace COMMANDS
{


    public class CMD_General : CMD_DatabaseExtension
    {
        new public static void Extend(CommandDatabase database)
        {
            database.AddCommand("wait", new Func<string, IEnumerator>(Wait));
        }

    private static IEnumerator Wait(string data)
    {
            if (float.TryParse(data, out float time))
            {
                yield return new WaitForSeconds(time);
            }
    }
    }
}