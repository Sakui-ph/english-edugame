using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace AUDIO_SYSTEM {
    public class AudioManager : MonoBehaviour
    {
        private const string SFX_PARENT_NAME = "SFX";
        private const string SFX_NAME_FORMAT = "SFX - [{0}]";
        public const float TRACK_TRANSITION_SPEED = 0.1f;
        
        public Dictionary<int, AudioChannel> channels = new Dictionary<int, AudioChannel>();

        // Sub part of a main mixer, a master can have multiple submixers
        public AudioMixerGroup musicMixer;
        public AudioMixerGroup sfxMixer;
        public AudioMixerGroup voiceMixer;

        public AudioSource voiceSource;
        
        Coroutine thread_voicebank = null;
        bool isVoicing => thread_voicebank != null;
        bool playingVoiceBank = false;
        private Transform sfxRoot;
        
        void Awake() 
        {
            transform.SetParent(null);
            sfxRoot  = new GameObject(SFX_PARENT_NAME).transform;
            sfxRoot.SetParent(transform);
        }


        // anything that is loaded from dialogue needs to be loaded from resources, so we need a path to the audio file
        public AudioSource PlaySoundEffect(string filePath, AudioMixerGroup mixer = null, float volume = 1f, float pitch = 1f, bool loop = false, bool additive = false) {
            AudioClip clip = Resources.Load<AudioClip>(FilePaths.resources_sfx + filePath);
            if (clip == null) {
                Debug.LogError($"Audio clip not found at path: {filePath}");
                return null;
            }
            

            return PlaySoundEffect(clip, mixer, volume, pitch, loop, additive);
        }

        public AudioSource PlaySoundEffect(AudioClip clip, AudioMixerGroup mixer = null, float volume = 1f, float pitch = 1f, bool loop = false, bool additive = false) {
            bool exists = false;
            if (additive) {
                AudioSource[] sources = sfxRoot.GetComponentsInChildren<AudioSource>();
                foreach (AudioSource source in sources) {
                    if (source.clip.name.ToLower() == clip.name.ToLower() && source.isPlaying) {
                        exists = true;
                    }
                }
            }

            if (exists) {
                return null;
            }

            AudioSource effectSource = new GameObject(string.Format(SFX_NAME_FORMAT, clip.name)).AddComponent<AudioSource>();
            effectSource.transform.SetParent(sfxRoot);
            effectSource.transform.position = sfxRoot.position;

            effectSource.clip = clip;

            if (mixer == null)
                mixer = sfxMixer;

            effectSource.outputAudioMixerGroup = mixer;
            effectSource.volume = volume;
            effectSource.spatialBlend = 0; // its not a 3d sound
            effectSource.pitch = pitch;
            effectSource.loop = loop;

            effectSource.Play();

            if (!loop) 
                Destroy(effectSource.gameObject, (clip.length / pitch) + 1f);

            return effectSource;
        }

        public AudioSource PlayVoice(string filePath, float volume = 1f, float pitch = 1f, bool loop = false) {
            AudioClip clip = Resources.Load<AudioClip>(FilePaths.resources_voices + filePath);
            return PlaySoundEffect(clip, voiceMixer, volume, pitch, loop);
        }

        public AudioSource PlayVoice(AudioClip clip, float volume = 1f, float pitch = 1f, bool loop = false) {
            return PlaySoundEffect(clip, voiceMixer, volume, pitch, loop);
        }

        public void RunVoiceBank(List<AudioClip> voiceBank)
        {
            if (isVoicing)
            {   
                StopVoiceBank();
                StopCoroutine(thread_voicebank);
            }
            thread_voicebank = StartCoroutine(PlayVoiceBank(voiceBank));
        }

        private IEnumerator PlayVoiceBank(List<AudioClip> voiceBank)
        {
            voiceSource.Stop();
            playingVoiceBank = true;
            while (playingVoiceBank)
            {
                int lastRandom = -1;
                int random = Random.Range(0, voiceBank.Count - 1);
                while(random == lastRandom)
                    random = Random.Range(0, voiceBank.Count - 1);

                lastRandom = random;
                AudioClip clip = voiceBank[random];
                voiceSource.clip = clip;
                voiceSource.Play();
                while(voiceSource.isPlaying)
                {
                    yield return null;
                }
            }

            if (voiceSource != null)
                voiceSource.Stop();
        }

        public void StopVoiceBank()
        {
            playingVoiceBank = false;
        }

        public void StopSoundEffect(AudioClip clip) => StopSoundEffect(clip.name);

        public void StopSoundEffect(string soundName) {
            soundName = soundName.ToLower();
            AudioSource[] sources = sfxRoot.GetComponentsInChildren<AudioSource>();
            foreach (AudioSource source in sources) {
                if (source.clip.name.ToLower() == soundName) {
                    Destroy(source.gameObject);
                    return;
                }
            }
        }

        public void StopAllSoundEffects() {
            AudioSource[] sources = sfxRoot.GetComponentsInChildren<AudioSource>();
            foreach (AudioSource source in sources) {
                Destroy(source.gameObject);
            }
        }

        public void StopAllTracks()
        {
            foreach ((int channel, AudioChannel ac) in channels)
            {
                StopTrack(channel, true);
            }
        }

        // for music / songs
        public AudioTrack PlayTrack(string filePath, int channel = 0, bool loop = true, float pitch = 1f, float startingVolume = 0f,  float volumeCap = 1f) {
            AudioClip clip = Resources.Load<AudioClip>(FilePaths.resources_music + filePath);
            if (clip == null) {
                Debug.LogError($"Audio clip not found at path: {filePath}");
                return null;
            }

            return PlayTrack(clip, channel, loop, startingVolume, volumeCap, pitch, filePath);
        }

        public AudioTrack PlayTrack(AudioClip clip, int channel = 0, bool loop = true, float startingVolume = 0f, float volumeCap = 1f, float pitch = 1f, string filepath = "") {
            AudioChannel audioChannel = TryGetChannel(channel, createIfDoesNotExist: true);
            AudioTrack track = audioChannel.PlayTrack(clip, loop, startingVolume, volumeCap, pitch, filepath);
            return track;
        }

        public Coroutine StopTrack(int channel, bool immediate = false)
        {
            AudioChannel c = TryGetChannel(channel);
            if (c == null)
                return null;

            return c.StopTrack(immediate);
        }


        public AudioChannel TryGetChannel(int channelNumber, bool createIfDoesNotExist = false) {
            AudioChannel channel = null;

            if (channels.TryGetValue(channelNumber, out channel)) {
                return channel;
            }
            else if (createIfDoesNotExist) {
                channel = new AudioChannel(channelNumber);
                channels.Add(channelNumber, channel);
                return channel;
            }

            return null;
        }

        void OnApplicationQuit()
        {
            StopAllSoundEffects();
            StopAllTracks();
        }

    }

}
