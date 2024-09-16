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
            CardMinigameLevelLoader.AddCard("WARRANT ONE", CLAIM_KEY_1, CardType.warrant, CARD_PAIR_1);
            CardMinigameLevelLoader.AddCard("WARRANT TWO", CLAIM_KEY_1, CardType.warrant, CARD_PAIR_2);
            CardMinigameLevelLoader.AddCard("WARRANT THREE", CLAIM_KEY_1, CardType.warrant, CARD_PAIR_3);

            CardMinigameLevelLoader.AddCard("GROUND ONE", CLAIM_KEY_1, CardType.ground, CARD_PAIR_1);
            CardMinigameLevelLoader.AddCard("GROUND TWO", CLAIM_KEY_1, CardType.ground, CARD_PAIR_2);
            CardMinigameLevelLoader.AddCard("GROUND THREE", CLAIM_KEY_1, CardType.ground, CARD_PAIR_3);


            CardMinigameLevelLoader.AddCard("WARRANT FOUR", CLAIM_KEY_2, CardType.warrant, CARD_PAIR_4);
            CardMinigameLevelLoader.AddCard("WARRANT FIVE", CLAIM_KEY_2, CardType.warrant, CARD_PAIR_5);

            CardMinigameLevelLoader.AddCard("GROUND FOUR", CLAIM_KEY_2, CardType.ground, CARD_PAIR_4);
            CardMinigameLevelLoader.AddCard("GROUND FIVE", CLAIM_KEY_2, CardType.ground, CARD_PAIR_5);

            CardMinigameLevelLoader.AddCard("WARRANT SIX", CLAIM_KEY_3, CardType.warrant, CARD_PAIR_6);

            CardMinigameLevelLoader.AddCard("GROUND SIX", CLAIM_KEY_3, CardType.ground, CARD_PAIR_6);

            CardMinigameLevelLoader.AddClaim("Claim One", CLAIM_KEY_1, ClaimType.AGAINST, 3);
            CardMinigameLevelLoader.AddClaim("Claim Two", CLAIM_KEY_2, ClaimType.FOR, 2);
            CardMinigameLevelLoader.AddClaim("Claim Three", CLAIM_KEY_3, ClaimType.IRRELEVANT, 1);

            CardMinigameLevelLoader.subject = "Test Subject";

            GameSystemSL.services.gameSystem.LoadCardGame(false);
        }
    }
}