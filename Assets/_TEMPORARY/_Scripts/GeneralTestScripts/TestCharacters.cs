using System.Collections;
using UnityEngine;
using CHARACTERS;
using static CHARACTERS.CharacterManager;

namespace TESTING
{
    public class TestCharacters : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            StartCoroutine(Test());   
        }

        Character CreateCharacter(string name)
        {
            return instance.CreateCharacter(name);
        }

        IEnumerator Test()
        {
            Character Doctor = CreateCharacter("Dokutah");
            Character Exia = CreateCharacter("Exusiai");
            Character_Sprite Nagito = CreateCharacter("Nagito") as Character_Sprite;
            Character_Sprite Nanami = CreateCharacter("Nanami") as Character_Sprite;


            Nagito.SetPosition(Vector2.zero);
            Nanami.SetPosition(Vector2.one);
            yield return Nagito.Show();
            yield return Nanami.Show();

            Sprite nagitoSprite0 = Nagito.GetSprite("normal");
            Sprite nagitoSprite1 = Nagito.GetSprite("smile");
            Sprite nagitoSprite2 = Nagito.GetSprite("think0");
            Sprite nagitoSprite3 = Nagito.GetSprite("think1");

            Sprite nanamiSprite0 = Nanami.GetSprite("angry");
            Sprite nanamiSprite1 = Nanami.GetSprite("inform");
            Sprite nanamiSprite2 = Nanami.GetSprite("normal");
            Sprite nanamiSprite3 = Nanami.GetSprite("sleepy");

            Nagito.TransitionSprite(nagitoSprite0, speed: 3);
            yield return new WaitForSeconds(1);
            Nagito.TransitionSprite(nagitoSprite1, speed: 3);
            yield return new WaitForSeconds(1);
            Nagito.TransitionSprite(nagitoSprite2, speed: 3);
            yield return new WaitForSeconds(1);
            Nagito.TransitionSprite(nagitoSprite3, speed: 3);
            yield return new WaitForSeconds(1);

            Nagito.Flip();

            Nanami.TransitionSprite(nanamiSprite0, speed: 3);
            yield return new WaitForSeconds(1);
            Nanami.TransitionSprite(nanamiSprite1, speed: 3);
            yield return new WaitForSeconds(1);
            Nanami.TransitionSprite(nanamiSprite2, speed: 3);
            yield return new WaitForSeconds(1);
            Nanami.TransitionSprite(nanamiSprite3, speed: 3);
            yield return new WaitForSeconds(1);
            
            Doctor.SetPosition(new Vector2(0.5f, 0.5f));
            yield return Doctor.Hide();
        }

        // Update is called once per frame
        void Update()
        {
            
        }
    }
}

