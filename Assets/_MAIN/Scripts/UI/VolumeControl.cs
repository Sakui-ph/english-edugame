using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class VolumeControl : MonoBehaviour
{
    public Slider volumeSlider => GetComponent<Slider>();
    public AudioMixer mainMixer;

    void Awake()
    {
        volumeSlider.onValueChanged.AddListener(delegate {UpdateVolumeOnChange ();});
        RetrieveSavedVolume();
    }

    // TODO FIX VOLUME SAVING 
    public void RetrieveSavedVolume()
    {
        float savedValue = GameSystem.instance.GetLoadedPlayer().volumePreference;
        volumeSlider.value = savedValue;
        mainMixer.SetFloat("Volume", Mathf.Log10(savedValue) * 20);
    }

    public void UpdateVolumeOnChange()
    {
        mainMixer.SetFloat("Volume", Mathf.Log10(volumeSlider.value) * 20);
        GameSystem.instance.GetLoadedPlayer().volumePreference = volumeSlider.value;
        GameSystem.instance.SaveLoadedPlayer();
    }

}