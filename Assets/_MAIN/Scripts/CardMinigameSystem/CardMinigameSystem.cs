using System;
using System.Collections;
using System.Collections.Generic;
using AUDIO_SYSTEM;
using TMPro;
using UnityEngine;

namespace CARD_GAME
{
    public class CardMinigameSystem : MonoBehaviour
    {
        private const int DEFAULT_STARTING_HP = 3;
        public ChainManager chainManager;
        public ClaimManager claimManager;
        private CardManager cardManager => CardManager.instance;
        private CardGamePlayerDataManager playerDataManager => CardGamePlayerDataManager.instance;
        public static CardMinigameSystem instance;
        public CardGameConfigSO config;
        private CardMinigameLevel level;
        private int claimCount => level.claims.Count;
        private Coroutine process = null;
        public TextMeshProUGUI subjectText;
        [Header("Audio")]
        public AudioClip BGM;
        private bool isRunning => process != null;

        void Awake()
        {
            if (instance == null)
                instance = this;
            else
            {
                DestroyImmediate(gameObject);
            }
        }

        public void StartGame(CardMinigameLevel level) // TODO: Make card spawning happen here as well, since all cards belong to the level
        {
            CardMinigameLevelLoader.Reset();
            if (isRunning) 
            {
                Debug.LogWarning("Process \"Card Game Minigame System\" was running after starting a new one. Destroying old process");
                StopCoroutine(process);
            }
            
            if (BGM != null)
            {
                AudioManager.instance.PlayTrack(BGM, startingVolume:0, loop:true, volumeCap: 0.3f, pitch: 0.8f);
            }

            process = StartCoroutine(RunMinigame(level));
        }   

        private IEnumerator RunMinigame(CardMinigameLevel level)
        {
            this.level = level;
            InitializePlayerData();
            LoadLevelObjects(level.claims, level.cardDataSet, level.subject);
            yield return null;
        }

        private void SpawnCards(HashSet<CardData> cards)
        {
            cardManager.SpawnCard(cards);
            cardManager.ShuffleDecks();
        }
    
        private void LoadLevelObjects(List<Claim> claims, HashSet<CardData> cards, string subjectText)
        {
            claimManager.SetClaimData(claims);
            claimManager.SetupClaimTabs(claimCount);
            for (int i = 0; i < claimCount; i++)
            {
                chainManager.SpawnChains(claims[i].chains, $"Chain Root {i + 1}");
            }
            claimManager.MapChainToTabGroup(chainManager.chainGroup);

            this.subjectText.text = subjectText;

            SpawnCards(cards);
        }

        private void InitializePlayerData()
        {
            if (level == null)
            {
                Debug.LogWarning("Tried to initialize the player without a loaded level!");
                return;
            }
                
            playerDataManager.Initialize(config.startingHP);
        }

        public void CheckFinished()
        {
            if (claimManager.CheckFinished())
            {
                CardMinigameLevelLoader.EndGame();
            }
        }

        void OnDestroy()
        {
            AudioManager.instance.StopTrack(0, true);
        }
    }

}
