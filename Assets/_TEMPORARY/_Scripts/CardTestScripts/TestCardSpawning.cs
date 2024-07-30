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
            const string CARD_PAIR_5 = "flkjajdasfweqwedfflka";
            const string CARD_PAIR_6 = "flkjajdwweqwasdfdflka";
       

            CardMinigameLevelLoader.AddCard("fake warrant 1", CardType.warrant);
            CardMinigameLevelLoader.AddCard("WARRANT ONE", CARD_PAIR_1, CardType.warrant, CLAIM_KEY_1);
            CardMinigameLevelLoader.AddCard("WARRANT TWO", CARD_PAIR_2, CardType.warrant, CLAIM_KEY_1);
            CardMinigameLevelLoader.AddCard("WARRANT THREE", CARD_PAIR_3, CardType.warrant, CLAIM_KEY_1);

            CardMinigameLevelLoader.AddCard("GROUND ONE", CARD_PAIR_1, CardType.ground, CLAIM_KEY_1);
            CardMinigameLevelLoader.AddCard("GROUND TWO", CARD_PAIR_2, CardType.ground, CLAIM_KEY_1);
            CardMinigameLevelLoader.AddCard("GROUND THREE", CARD_PAIR_3, CardType.ground, CLAIM_KEY_1);


            CardMinigameLevelLoader.AddCard("WARRANT FOUR", CARD_PAIR_4, CardType.warrant, CLAIM_KEY_2);
            CardMinigameLevelLoader.AddCard("WARRANT FIVE", CARD_PAIR_5, CardType.warrant, CLAIM_KEY_2);

            CardMinigameLevelLoader.AddCard("GROUND FOUR", CARD_PAIR_4, CardType.ground, CLAIM_KEY_2);
            CardMinigameLevelLoader.AddCard("GROUND FIVE", CARD_PAIR_5, CardType.ground, CLAIM_KEY_2);

            CardMinigameLevelLoader.AddCard("WARRANT SIX", CARD_PAIR_6, CardType.warrant, CLAIM_KEY_3);

            CardMinigameLevelLoader.AddCard("GROUND SIX", CARD_PAIR_6, CardType.ground, CLAIM_KEY_3);

            CardMinigameLevelLoader.AddClaim("Claim One", CLAIM_KEY_1, ClaimType.AGAINST, 3);
            CardMinigameLevelLoader.AddClaim("Claim Two", CLAIM_KEY_2, ClaimType.FOR, 2);
            CardMinigameLevelLoader.AddClaim("Claim Three", CLAIM_KEY_3, ClaimType.IRRELEVANT, 1);

            CardMinigameLevelLoader.subject = "Test Subject";

            GameSystem.instance.LoadCardGame(false);
        }
    }
}