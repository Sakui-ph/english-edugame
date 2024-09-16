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
        public ChainManager chainManager;
        public ClaimManager claimManager;
        public CardGamePlayer cardGamePlayer = new();
        public CardGameHealthDisplay cardGameHealthDisplay;
        private CardManager cardManager => CardManager.instance;
        private CardMinigameLevel level;
        private int claimCount => level.claims.Count;
        private Coroutine process = null;
        public TextMeshProUGUI subjectText;
        [Header("Audio")]
        public AudioClip BGM;
        public AudioClip correctSound;
        public AudioClip incorrectSound;    
        private bool isRunning => process != null;
        
        [Header("UI Elements")]
        [SerializeField] private OverlayUIElements overlayUIElements;

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
                GameSystemSL.services.audioManager.PlayTrack(BGM, startingVolume:0, loop:true, volumeCap: 0.3f, pitch: 0.8f);
            }

            process = StartCoroutine(RunMinigame());
        }  

        public void RestartLevel()
        {
            StopCoroutine(process);
            overlayUIElements.FadeToBlack().setOnComplete(() =>
            {
                process = StartCoroutine(RunMinigame());
                overlayUIElements.FadeToWhite();
            });
        }

        private IEnumerator RunMinigame()
        {
            LoadLevelObjects(level.claims, level.cardDataSet, level.subject);
            InitializePlayer();
            yield return null;
        }

        private void LoadLevelObjects(List<Claim> claims, HashSet<CardData> cards, string subjectText)
        {
            cardManager.DestroyAllCards();
            chainManager.DestroyChains();


            claimManager.SetClaimData(claims);
            claimManager.SetupClaimTabs(claimCount);
            for (int i = 0; i < claimCount; i++)
            {
                chainManager.SpawnChains(claims[i].chainDataList, $"Chain Root {i + 1}");
            }
            claimManager.MapChainToTabGroup(chainManager.chainGroup);

            this.subjectText.text = subjectText;

            cardManager.SpawnCard(cards);
        }

        public void TryEndMinigame()
        {
            if (!claimManager.CheckFinished())
                return;

            GameSystemSL.services.gameSystem.currentLevel.EndLevel();
        }

        private void InitializePlayer()
        {
            cardGamePlayer.HealthChanged += cardGameHealthDisplay.OnHealthChange; 
            cardGamePlayer.SetHealth(config.startingHP);
        }


        void OnDestroy()
        {
            GameSystemSL.services.audioManager.StopTrack(0, true);
        }
    }

}
