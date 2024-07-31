using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using AUDIO_SYSTEM;
using TMPro;
using UnityEngine;

namespace CARD_GAME
{
    public class CardMinigameSystem : MonoBehaviour
    {
        public static CardMinigameSystem instance;
        public CardGameConfigSO config;
        public CardGamePlayer cardGamePlayer = new();
        public ChainManager chainManager;
        public ClaimManager claimManager;
        public CardGameHealthDisplay cardGameHealthDisplay;
        private CardManager cardManager => CardManager.instance;
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

        public void StartGame() // TODO: Make card spawning happen here as well, since all cards belong to the level
        {
            this.level = CardMinigameLevelLoader.LoadLevel();

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

            process = StartCoroutine(RunMinigame());
        }   

        private IEnumerator RunMinigame()
        {
            LoadLevelObjects(level.claims, level.cardDataSet, level.subject);
            InitializePlayer();
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
                chainManager.SpawnChains(claims[i].chainDataList, $"Chain Root {i + 1}");
            }
            claimManager.MapChainToTabGroup(chainManager.chainGroup);

            this.subjectText.text = subjectText;

            SpawnCards(cards);
        }

        public void TryEndMinigame()
        {
            if (!claimManager.CheckFinished())
                return;

            GameSystem.instance.currentLevel.EndLevel();
        }

        private void InitializePlayer()
        {
            cardGamePlayer.HealthChanged += cardGameHealthDisplay.OnHealthChange; 
            cardGamePlayer.ChangeHealth(config.startingHP);
        }


        void OnDestroy()
        {
            AudioManager.instance.StopTrack(0, true);
        }
    }

}
