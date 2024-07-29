using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using AUDIO_SYSTEM;

[System.Serializable]
public class MenuButton : Button
{
    public AudioClip clickAudio => GameSystem.instance.menuConfigSO.buttonClickSound;
    [SerializeField] public AudioClip clickSound;
    public override void OnPointerClick(PointerEventData eventData)
    {
        base.OnPointerClick(eventData);
        if (clickSound == null)
            AudioManager.instance.PlaySoundEffect(clickAudio);
        else
            AudioManager.instance.PlaySoundEffect(clickSound);
    }

}
