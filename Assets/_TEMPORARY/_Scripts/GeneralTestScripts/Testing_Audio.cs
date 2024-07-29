using System.Collections;
using System.Collections.Generic;
using CHARACTERS;
using AUDIO_SYSTEM;
using UnityEngine;

namespace TESTING {
    public class AudioTesting : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            StartCoroutine(Running());
        }

        Character CreateCharacter(string name) => CharacterManager.instance.CreateCharacter(name);

        IEnumerator Running()
        {
            Character_Sprite Nanami = CreateCharacter("Nanami") as Character_Sprite;
            Character Me = CreateCharacter("Me");
            Nanami.Show();

            AudioManager.instance.PlayTrack("misty_memory", startingVolume: 0f, volumeCap: 1f);

            yield return new WaitForSeconds(3);

            yield return Me.Say("Please stop the music");

            Nanami.TransitionSprite(Nanami.GetSprite("pout"));

            yield return new WaitForSeconds(5);
            

            yield return null;
            Nanami.Say("much better");
        }
    }
}
