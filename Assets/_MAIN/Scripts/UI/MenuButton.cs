using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using AUDIO_SYSTEM;

[System.Serializable]
public class MenuButton : Button
{
    public AudioClip clickAudio => GameSystemSL.services.gameSystem.menuConfigSO.buttonClickSound;
    [SerializeField] public AudioClip clickSound;
    public override void OnPointerClick(PointerEventData eventData)
    {
        base.OnPointerClick(eventData);
        if (clickSound == null)
            GameSystemSL.services.audioManager.PlaySoundEffect(clickAudio);
        else
            GameSystemSL.services.audioManager.PlaySoundEffect(clickSound);
    }

}
