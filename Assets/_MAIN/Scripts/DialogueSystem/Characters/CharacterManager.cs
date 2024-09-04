using System.Collections.Generic;
using DIALOGUE;
using UnityEngine;

namespace CHARACTERS
{
    public class CharacterManager : MonoBehaviour
    {
        private static CharacterManager m_Instance = null;
        public static CharacterManager instance
        {
            get
            {
                if (m_Instance == null)
                {
                    m_Instance = FindObjectOfType<CharacterManager>();
                }
                return m_Instance;
            }
        }
        private static Dictionary<string, Character> characters = new Dictionary<string, Character>();
        private CharacterConfigSO config => VisualNovelSL.services.dialogueSystem.config.characterConfigurationAsset;

        // Casting characters so that you can have multiple characters that use the same configuration
        private const string CHAR_CASTING_ID = " as ";

        // Need fields for the root path of the character, and prefabs
        // Uses injection / string formatting to get the correct path
        private const string CHAR_NAME_PLACEHOLDER = "<charname>";
        private string characterRootPath => $"Characters/{CHAR_NAME_PLACEHOLDER}";
        public string characterPrefabName => $"Character - [{CHAR_NAME_PLACEHOLDER}]";
        private string characterPrefabPath => $"{characterRootPath}/{characterPrefabName}";

        [SerializeField] private RectTransform characterPanel = null;
        public RectTransform CharacterPanel => characterPanel;
        
        public CharacterConfigData GetCharacterConfig(string characterName)
        {
            return config.GetConfig(characterName);
        }

        public Character GetCharacter(string characterName, bool createIfDoesNotExist = false)
        {
            if (CharacterExists(characterName))
                return characters[characterName.ToLower()];
            else if (createIfDoesNotExist) {
                CreateCharacter(characterName);
                return null;
            } 
                
            Debug.LogWarning($"Character {characterName} does not exist. Failed to get character.");
            return null;
        }

        public Character CreateCharacter(string characterName, bool revealAfterCreation = false)
        {
            if (CharacterExists(characterName))
            {
                Debug.LogWarning($"A character with name {characterName} already exists. Character creation failed.");
                return null;
            }

            // find out the character type
            CHARACTER_INFO info = GetCharacterInfo(characterName);
            Character character = CreateCharacterFromInfo(info);


            characters.Add(info.name.ToLower(), character);
            if (revealAfterCreation)
                character.Show();

            return character;
        }

        private Character CreateCharacterFromInfo(CHARACTER_INFO info) 
        {
            CharacterConfigData config = info.config;

            switch (config.characterType)
            {
                case Character.CharacterType.Text:
                    return new Character_Text(info.name, config, null);
                case Character.CharacterType.Sprite:
                case Character.CharacterType.SpriteSheet:
                    return new Character_Sprite(info.name, config, info.prefab, info.rootCharacterFolder);
                case Character.CharacterType.Live2D:
                    return new Character_Live2D(info.name, config, info.prefab, info.rootCharacterFolder);
                case Character.CharacterType.Model3D:
                    return new Character_Model3D(info.name, config, info.prefab, info.rootCharacterFolder);
                default:
                    return null;
            }

        }

        public bool CharacterExists(string characterName) => characters.ContainsKey(characterName.ToLower());

        private CHARACTER_INFO GetCharacterInfo(string characterName)
        {
            CHARACTER_INFO result = new CHARACTER_INFO();

            string[] nameData = characterName.Split(CHAR_CASTING_ID, System.StringSplitOptions.RemoveEmptyEntries);

            result.name = nameData[0];

            result.castingName = nameData.Length > 1 ? nameData[1] : result.name;

            result.config = GetCharacterConfig(result.castingName);

            result.prefab = GetPrefabForCharacter(result.castingName);

            result.rootCharacterFolder = FormatCharacterPath(characterRootPath, result.castingName);

            return result;
        }

        private GameObject GetPrefabForCharacter(string characterName)
        {
            string path = FormatCharacterPath(characterPrefabPath, characterName);
            GameObject prefab = Resources.Load<GameObject>(path);
            if (prefab == null)
                Debug.LogWarning($"No prefab found for character {characterName}.");
            return prefab;
        }

        // Inject character name
        public string FormatCharacterPath(string path, string characterName) => path.Replace(CHAR_NAME_PLACEHOLDER, characterName);

        private class CHARACTER_INFO
        {
            public string name = "";
            public string castingName = "";

            public string rootCharacterFolder = "";

            public CharacterConfigData config = null;
            public GameObject prefab = null;
        }

        public void ClearCharacters()
        {
            // empty character panel children
            foreach (Transform child in characterPanel)
            {
                Destroy(child.gameObject);
            }
            characters.Clear();
        }

        public void DimCharacters(Color dimColor)
        {
            foreach((string name, Character character) in characters)
            {
                if (character is Character_Sprite && !character.overriddenColor)
                {
                    character.SetColor(dimColor);
                }
            }
        }

        void OnDestroy()
        {
            if (m_Instance == this)
                m_Instance = null;

            characters.Clear();
            // Debug.Log("Character Manager destroyed");
        }
    }
}
