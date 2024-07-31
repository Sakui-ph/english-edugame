
using System.Collections.Generic;
using UnityEngine;

namespace CARD_GAME
{
    public class ChainManager : MonoBehaviour
    {
        public const string WARRANT_SLOT_KEY = "WarrantSlot";
        public const string GROUND_SLOT_KEY = "GroundSlot";
        [SerializeField] private GameObject chainPrefab;
        public GameObject chainGroup;
        [SerializeField] private GameObject chainRootPrefab;

        public void SpawnChains(List<ChainData> chainDataList, string gameObjectName = "Chain Root")
        {
            DestroyChains();
            GameObject chainRoot = Instantiate(chainRootPrefab, chainGroup.transform);
            foreach(var chainData in chainDataList)
            {
                GameObject chainObject = Instantiate(chainPrefab, chainRoot.transform);
                chainRoot.name = gameObjectName;
                Chain chain = chainObject.GetComponent<Chain>();
                chain.chainData = chainData;
                
            }
        }

        public void DestroyChains()
        {
            foreach(Transform child in chainRootPrefab.transform)
            {
                Destroy(child.gameObject);
            }
        }
    }
}