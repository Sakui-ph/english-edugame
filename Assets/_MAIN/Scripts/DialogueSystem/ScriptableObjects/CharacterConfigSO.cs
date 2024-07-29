using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CHARACTERS
{
    [CreateAssetMenu(fileName = "Character Configuration Asset", menuName = "Dialogue System/Character Configuration Asset")]
    public class CharacterConfigSO : ScriptableObject
    {
        public CharacterConfigData[] characters;

        public CharacterConfigData GetConfig(string characterName)
        {
            for (int i = 0; i < characters.Length; i++)
            {
                CharacterConfigData data = characters[i];
                if (string.Equals(data.name.ToLower(), characterName.ToLower()) || string.Equals(data.alias.ToLower(), characterName.ToLower()))
                {
                    return data.Clone();
                }
            }
            return CharacterConfigData.Default;
        }
    }
}