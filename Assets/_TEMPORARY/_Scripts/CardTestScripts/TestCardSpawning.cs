using System.Collections.Generic;
using UnityEngine;
using CARD_GAME;
using System.Runtime.CompilerServices;

namespace TESTING 
{
    
    public class TestCardSpawning : MonoBehaviour 
    {
        void Start()
        {
            const string CLAIM_KEY_1 = "CLksdhffkjFkj";
            const string CLAIM_KEY_2 = "fhaousd fhodj";
            const string CLAIM_KEY_3 = "hdfkjd hfk";

            const string CARD_PAIR_1 = "flkjajdflka";
            const string CARD_PAIR_2 = "flkjajdflfsdfka";
            const string CARD_PAIR_3 = "flkjajsadfadflka";
            const string CARD_PAIR_4 = "flkjajdasdfflka";


            HashSet<Card> cards = new()
            {
                new Card("Card One", CardType.ground, CLAIM_KEY_1, CARD_PAIR_1),
                new Card("Card Two", CardType.warrant, CLAIM_KEY_1, CARD_PAIR_1),

                new Card("Card Three", CardType.ground, CLAIM_KEY_2, CARD_PAIR_2),
                new Card("Card Four", CardType.warrant, CLAIM_KEY_2, CARD_PAIR_2),

                new Card("Card Five", CardType.ground, CLAIM_KEY_3, CARD_PAIR_3),
                new Card("Card Six", CardType.warrant, CLAIM_KEY_3, CARD_PAIR_3),
                new Card("Card Seven", CardType.ground, CLAIM_KEY_3, CARD_PAIR_4),
                new Card("Card Eight", CardType.warrant,CLAIM_KEY_3, CARD_PAIR_4),

                new Card("Card Nine", CardType.ground),
                new Card("Card Ten",  CardType.warrant),
                new Card("Card Eleven", CardType.ground),
                new Card("Card Twelve", CardType.warrant),
                new Card("Card Thirteen", CardType.ground),
            };

            List<Claim> claims = new() {
                new(CLAIM_KEY_1, 1, "This is claim 1", ClaimType.FOR),
                new(CLAIM_KEY_2, 3, "This is claim 2", ClaimType.IRRELEVANT),
                new(CLAIM_KEY_3, 2, "This is claim 3", ClaimType.AGAINST),
            };
        

        }
    }
}