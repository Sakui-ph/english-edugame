using System;
using System.Collections.Generic;
using System.Security.Claims;
using UnityEngine;

public static class HigherOrderErrorHandler
{
    // This is more of an error counter
    public static int claim_error = 0;
    public static int ground_error = 0;
    public static int warrant_error = 0;
    public static List<HighOrderError> highOrderErrors = new();
    public static void AddError(
        HOErrorType errorType, 
        string subject, 
        string claim = "", 
        string claim_sort = "", 
        string warrant_card_text = "", 
        string ground_card_text = "")
    {
        switch((int)errorType)
        {
            case 0:
                claim_error++;
                break;
            case 1:
                ground_error++;
                break;
            case 2:
                warrant_error++;
                break;
        }
        highOrderErrors.Add(new HighOrderError(errorType, subject, claim, claim_sort, warrant_card_text, ground_card_text));

        // TODO make it so that this save only happens at the end of a level
        Save(subject);
    }

    public static void Reset()
    {
        claim_error = 0;
        ground_error = 0;
        warrant_error = 0;
        highOrderErrors = new();
    }

    public static void Save(string subject)
    {
        SaveSystem.SaveHOErrors(subject);
    }
}

public class HighOrderError
{
    public HOErrorType errorType;
    public string subject;
    public string claim;
    public string claim_sort;
    public string warrant_card_text;
    public string ground_card_text;
    public HighOrderError(
        HOErrorType errorType, 
        string subject, 
        string claim = "", 
        string claim_sort = "", 
        string warrant_card_text= "", 
        string ground_card_text = "")
    {
        this.errorType = errorType;
        this.subject = subject;
        this.claim = claim;
        this.claim_sort = claim_sort;
        this.warrant_card_text = warrant_card_text;
        this.ground_card_text = ground_card_text;
    }

    public override string ToString()
    {
        return $"Mistake made:\n\tType: {errorType.ToString()}\n\tSubject: {subject}\n\tClaim: {claim}\n\t\tClaimSort: {claim_sort}\n\tCards:\n\t\tWarrant: {warrant_card_text}\n\t\tGround: {ground_card_text}";
    }
}

public enum HOErrorType
{
    CLAIM_ERROR, // failed to identify if a claim is FOR, AGAINST, or IRRELEVANT
    GROUND_ERROR, // failed to put grounds under the right claim
    WARRANT_ERROR, // failed to put the right warrant with the right grounds
    TOTAL_ERROR
}