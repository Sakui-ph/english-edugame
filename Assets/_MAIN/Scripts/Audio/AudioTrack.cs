using AUDIO_SYSTEM;
using UnityEngine;
using UnityEngine.Audio;

public class AudioTrack
{
    private const string TRACK_NAME_FORMAT = "Track - [{0}]";
    public string name { get; private set; }
    private AudioChannel channel;
    private AudioSource source;
    
    public GameObject root => source.gameObject;
    public bool loop => source.loop;
    public float volume {get {return source.volume;} set {source.volume = value;} }
    public float volumeCap { get; private set; }

    public bool isPlaying => source.isPlaying;

    public AudioTrack(AudioClip clip, bool loop, float startingVolume, float volumeCap, float pitch, AudioChannel channel, AudioMixerGroup mixer) {
        name = clip.name;
        this.channel = channel;
        this.volumeCap = volumeCap;

        source = CreateSource();
        source.clip = clip;
        source.loop = loop;
        source.volume = startingVolume;
        source.outputAudioMixerGroup = mixer;
        source.pitch = pitch;
    }
    
    private AudioSource CreateSource() {
        GameObject go = new GameObject(string.Format(TRACK_NAME_FORMAT, name));
        go.transform.SetParent(channel.trackContainer);
        AudioSource source = go.AddComponent<AudioSource>();

        return source;
    }

    public void SetVolumeCap(float value) {
        volumeCap = value;
    }

    public void Play() {
        source.Play();
    }

    public void Stop() {
        source.Stop();
    }
}   
