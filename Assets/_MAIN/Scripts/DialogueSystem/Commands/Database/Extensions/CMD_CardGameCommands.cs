using CARD_GAME;
using COMMANDS;
using System;
using UnityEngine;
public class CMD_CardGameCommands : CMD_DatabaseExtension
{   
    private static string[] TEXT = new string[] {"-t", "-text"};
    private static string[] KEY = new string[] {"-k", "-key"};
    private static string[] CONNECTION_KEY = new string[] {"-ck", "-connectkey"};
    private static string[] CHAINS = new string[] {"-ch", "chains"};
    private static GameSystem gameSystem => GameSystemSL.services.gameSystem;

    new public static void Extend(CommandDatabase database)
    {
        database.AddCommand("playcardgame", new Action<string[]>(PlayCardGame));
        // usage: AddGroundCard(cardText, claimKey, connectionKey)
        database.AddCommand("addgroundcard", new Action<string[]>(AddGroundCard));
        database.AddCommand("addwarrantcard", new Action<string[]>(AddWarrantCard));

        // usage: AdFordClaim(claimText, claimKey, numChains)
        database.AddCommand("addforclaim", new Action<string[]>(AddForClaim));
        database.AddCommand("addagainstclaim",  new Action<string[]>(AddAgainstClaim));
        database.AddCommand("addirrelevantclaim", new Action<string[]>(AddIrrelevantClaim));
    }

    private static void PlayCardGame(string[] data)
    {
        var parameters = ConvertDataToParameters(data);
        string subjectName = "";
        bool tutorialMode = false;     
        parameters.TryGetValue("-s", out subjectName, "");
        parameters.TryGetValue("-t", out tutorialMode, false);   
        CardMinigameLevelLoader.subject = subjectName;
        gameSystem.LoadCardGame(tutorialMode);
    }

    private static void AddWarrantCard(string[] data)
    {
        string cardText = "";
        string claimKey = "";
        string connectionKey = "";
        (cardText, claimKey, connectionKey) = CreateCard(data);
        
        if (cardText == "")
        {
            Debug.LogError("No card");
            return;
        }
            
        if (claimKey == "" || connectionKey == "")
        {
            CardMinigameLevelLoader.AddCard(cardText, CardType.warrant);
        }
        else
        {
            CardMinigameLevelLoader.AddCard(cardText, claimKey, CardType.warrant, connectionKey);
        }
    }

    private static void AddGroundCard(string[] data)
    {
        string cardText = "";
        string claimKey = "";
        string connectionKey = "";
        (cardText, claimKey, connectionKey) = CreateCard(data);
        
        if (cardText == "")
        {
            Debug.LogError("No card");
            return;
        }
            
        if (claimKey == "" || connectionKey == "")
        {
            CardMinigameLevelLoader.AddCard(cardText, CardType.ground);
        }
        else
        {
            CardMinigameLevelLoader.AddCard(cardText, claimKey, CardType.ground, connectionKey);
        }
    }

    private static (string, string, string) CreateCard(string[] data)
    {
        var parameters = ConvertDataToParameters(data);
        string cardText = "";
        string claimKey = "";
        string connectionKey = "";

        parameters.TryGetValue(TEXT, out cardText, "");
        if (cardText == "")
        {   
            Debug.LogError("No cardText supplied");
            return ("", "", "");
        }

        parameters.TryGetValue(KEY, out claimKey, "");

        parameters.TryGetValue(CONNECTION_KEY, out connectionKey, "");
        return (cardText, claimKey, connectionKey);
    }

    private static void AddForClaim(string[] data)
    {
        string claimText = "";
        string claimKey = "";
        int numChains = 0;
        (claimText, claimKey, numChains) = CreateClaim(data);

        if (claimText == "")
        {
            Debug.LogError("No subject provided");
            return;
        }

        if (claimKey == "")
        {
            Debug.LogError("No claim key provided");
            return;
        }

        if (numChains == 0)
        {
            Debug.LogError("No chains");
            return;
        }

        CardMinigameLevelLoader.AddClaim(claimText, claimKey, ClaimType.FOR, numChains);
    }

    private static void AddAgainstClaim(string[] data)
    {
        string claimText = "";
        string claimKey = "";
        int numChains = 0;
        (claimText, claimKey, numChains) = CreateClaim(data);

        if (claimText == "")
        {
            Debug.LogError("No subject provided");
            return;
        }

        if (claimKey == "")
        {
            Debug.LogError("No claim key provided");
            return;
        }

        if (numChains == 0)
        {
            Debug.LogError("No chains");
            return;
        }

        CardMinigameLevelLoader.AddClaim(claimText, claimKey, ClaimType.AGAINST, numChains);
    }

    private static void AddIrrelevantClaim(string[] data)
    {
        string claimText = "";
        string claimKey = "";
        int numChains = 0;
        (claimText, claimKey, numChains) = CreateClaim(data);

        if (claimText == "")
        {
            Debug.LogError("No subject provided");
            return;
        }

        if (claimKey == "")
        {
            Debug.LogError("No claim key provided");
            return;
        }

        if (numChains == 0)
        {
            Debug.LogError("No chains");
            return;
        }

        CardMinigameLevelLoader.AddClaim(claimText, claimKey, ClaimType.IRRELEVANT, numChains);
    }

    private static (string, string, int) CreateClaim(string[] data)
    {
        var parameters = ConvertDataToParameters(data);
        string claimText = "";
        string claimKey = "";
        int numChains = 0;

        parameters.TryGetValue(TEXT, out claimText, "");
        
        parameters.TryGetValue(KEY, out claimKey, "");
        
        parameters.TryGetValue(CHAINS, out numChains, 0);
        return (claimText, claimKey, numChains);
    }
}


