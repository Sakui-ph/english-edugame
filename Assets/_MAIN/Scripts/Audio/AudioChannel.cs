using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AUDIO_SYSTEM {
    public class AudioChannel {
        private const string TRACK_CONTAINER_NAME_FORMAT = "Channel - [{0}]";

        public int channelIndex { get; private set; }
        public Transform trackContainer { get; private set; } = null;
        private List<AudioTrack> tracks = new List<AudioTrack>();
        public AudioTrack activeTrack { get; private set;} = null;
        Coroutine thread_volumeLeveling = null;
        bool isVolumeLeveling => thread_volumeLeveling != null;

        public AudioChannel(int channel) {
            channelIndex = channel;
            trackContainer = new GameObject(string.Format(TRACK_CONTAINER_NAME_FORMAT, channelIndex)).transform;
            trackContainer.SetParent(AudioManager.instance.transform);
        }

        public AudioTrack PlayTrack(AudioClip clip, bool loop, float startingVolume, float volumeCap, float pitch, string filePath) {
            if (TryGetExistingTrack(clip.name, out AudioTrack existingTrack)) {
                if (volumeCap != existingTrack.volumeCap)
                    existingTrack.SetVolumeCap(volumeCap);

                if (!existingTrack.isPlaying)
                    existingTrack.Play();

                SetAsActiveTrack(existingTrack);
                return existingTrack;
            }

            AudioTrack track = new AudioTrack(clip, loop, startingVolume, volumeCap, pitch, this, AudioManager.instance.musicMixer);
            track.Play();

            SetAsActiveTrack(track);
            return track;
        }

        private void SetAsActiveTrack(AudioTrack track)
        {
            if (!tracks.Contains(track))
                tracks.Add(track);

            activeTrack = track;

            TryStartVolumeLeveling();
        }

        public bool TryGetExistingTrack(string trackName, out AudioTrack value) {
            trackName = trackName.ToLower();
            foreach(var track in tracks) {
                if (trackName == track.name.ToLower()) {
                    value = track;
                    return true;
                }
            }
            value = null;
            return false;
        }

        private Coroutine TryStartVolumeLeveling()
        {
            if (!isVolumeLeveling) {
                thread_volumeLeveling = AudioManager.instance.StartCoroutine(VolumeLeveling());
                return thread_volumeLeveling;
            }
            return null;   
        }

        private IEnumerator VolumeLeveling()
        {
            while ((activeTrack != null && (tracks.Count > 1 || activeTrack.volume != activeTrack.volumeCap)) || 
            (activeTrack == null && tracks.Count > 0))
            {
                for (int i = tracks.Count - 1; i >= 0; i--)
                {
                    AudioTrack track = tracks[i];

                    float targetVol = activeTrack == track ? track.volumeCap : 0f;
                    if (track.volume == targetVol && track == activeTrack)
                    {
                        continue;
                    }
                    
                    track.volume = Mathf.MoveTowards(track.volume, targetVol, AudioManager.TRACK_TRANSITION_SPEED * Time.deltaTime);

                    if (track != activeTrack && track.volume == 0f)
                    {
                        DestroyTrack(track);
                    }
                }
                yield return null;
            }

            thread_volumeLeveling = null;
        }

        private void DestroyTrack(AudioTrack track)
        {
            if (tracks.Contains(track))
                tracks.Remove(track);
            Object.Destroy(track.root);
        }

        public Coroutine StopTrack(bool immediate = false)
        {
            if (activeTrack == null)
                return null;

            if (immediate)
            {
                activeTrack.volume = 0f;
            }
            activeTrack = null;
            if(!immediate)
                return TryStartVolumeLeveling();

            return null;
        }
    }
}   

