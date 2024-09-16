using AUDIO_SYSTEM;
using UnityEngine;

namespace MAIN_MENU
{
    [System.Serializable]
    public class MainMenuAudio
    {
        private AudioManager audioManager => GameSystemSL.services.audioManager; 
        public AudioClip BGM;
        public void Play() => audioManager.PlayTrack(BGM, startingVolume:0, loop:true, volumeCap: 0.7f, pitch: 1.5f);
        
        public void Stop() => audioManager.StopTrack(0, true);
    }
}