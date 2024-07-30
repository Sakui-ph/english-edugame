using UnityEngine;

public class CardData
{
    public string cardText {get; private set;} // the text displayed in the card
    public CardType cardType {get; private set;} // the type of card we have
    
    public string claimKey {get; private set;} = null;
    public string connectionKey {get; private set;} = null;
    public bool hasConnectionKey => connectionKey != null;  

    public CardData(string cardText, CardType cardType, string claimKey, string connectionKey)
    {
        this.cardText = cardText;
        this.cardType = cardType;
        this.claimKey = claimKey;
        this.connectionKey = connectionKey;
    }

    public CardData(string cardText, CardType cardType)
    {
        this.cardText = cardText;
        this.cardType = cardType;
    }
}
public enum CardType { ground, warrant } 
