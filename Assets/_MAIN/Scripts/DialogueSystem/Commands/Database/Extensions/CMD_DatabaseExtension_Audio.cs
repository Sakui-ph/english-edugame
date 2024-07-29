using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using COMMANDS;
using AUDIO_SYSTEM;
using UnityEngine.Audio;
using System;

public class CMD_DatabaseExtension_Audio : CMD_DatabaseExtension
{
    private static string[] PARAM_TRACK = new string[] { "-t", "-track" };
    private static string[] PARAM_VOLUME = new string[] { "-v", "-volume" };
    private static string[] PARAM_STARTING_VOLUME = new string[] { "-sv", "-startingvolume" };
    private static string[] PARAM_PITCH = new string[] { "-p", "-pitch" };
    private static string[] PARAM_LOOP = new string[] { "-l", "-loop" }; 
    private static string[] PARAM_CHANNEL = new string[] { "-c", "-channel" };
    private static string[] PARAM_IMMEDIATE = new string[] { "-i", "-immediate" };
    private const string HOME_DIRECTORY_SYMBOL = "~/";
    new public static void Extend(CommandDatabase database)
    {
        // USAGE: -t trackname -c channel -v volume -p pitch -sv startingvolume -l loop
        database.AddCommand("playtrack", new Action<string[]>(PlayMusic));
        // USAGE: -t trackname -v volume -p pitch -l loop
        database.AddCommand("playsoundeffect", new Action<string[]>(PlaySFX));
        // USAGE: -t trackname -v volume -p pitch -l loop
        database.AddCommand("playvoice", new Action<string[]>(PlayVoice));
        // Usage -t trackname -c channel 
        database.AddCommand("stoptrack", new Func<string[], IEnumerator>(StopMusic));
        // Usage -t trackname
        database.AddCommand("stopsoundeffect", new Action<string>(StopSFX));
    }
    
    private static void StopSFX(string data)
    {
        string trackName = data;
        AudioManager.instance.StopSoundEffect(trackName);
    }

    private static IEnumerator StopMusic(string[] data)
    {
        var parameters = ConvertDataToParameters(data);
        int channel = 0;
        bool immediate = false;

        parameters.TryGetValue(PARAM_CHANNEL, out channel, defaultValue: -1);
        parameters.TryGetValue(PARAM_IMMEDIATE, out immediate, defaultValue: false);

        if (channel == -1)
        {
            Debug.LogError("Invalid channel number");
            yield return null;
        }
        
        yield return AudioManager.instance.StopTrack(channel);
    }

    private static void PlaySFX(string[] data)
    {
        string filePath = "";
        AudioClip clip = null;
        var parameters = ConvertDataToParameters(data);

        parameters.TryGetValue(PARAM_TRACK, out filePath);
        clip = Resources.Load<AudioClip>(GetPathToAudio(FilePaths.resources_sfx, filePath));
        if (clip == null)
        {
            Debug.LogError($"Audio clip not found at path: {filePath}");
            return;
        }
        AudioMixerGroup audioMixerGroup = AudioManager.instance.sfxMixer;
        PlaySound(parameters, clip, audioMixerGroup);
    }

    private static void PlayVoice(string[] data)
    {
        string filePath = "";
        AudioClip clip = null;
        var parameters = ConvertDataToParameters(data);

        parameters.TryGetValue(PARAM_TRACK, out filePath);
        clip = Resources.Load<AudioClip>(GetPathToAudio(FilePaths.resources_voices, filePath));
        if (clip == null)
        {
            Debug.LogError($"Audio clip not found at path: {filePath}");
            return;
        }
        AudioMixerGroup audioMixerGroup = AudioManager.instance.voiceMixer;
        PlaySound(parameters, clip, audioMixerGroup);
    }
    
    private static void PlaySound(CommandParameters parameters, AudioClip clip, AudioMixerGroup mixer)
    {   
        float volume = 1f;
        float pitch = 1f;
        bool loop = false;

        parameters.TryGetValue(PARAM_VOLUME, out volume, defaultValue: 1f);
        parameters.TryGetValue(PARAM_PITCH, out pitch, defaultValue: 1f);
        parameters.TryGetValue(PARAM_LOOP, out loop, defaultValue: false);

        if (mixer == AudioManager.instance.voiceMixer)
        {
            AudioManager.instance.PlayVoice(clip, volume, pitch, loop);
            return;
        }
        AudioManager.instance.PlaySoundEffect(clip, mixer, volume, pitch, loop);
        return;
    }

    // Usage: -t trackname -c channel -v volume -p pitch -sv startingvolume -l loop
    private static void PlayMusic(string[] data)
    {
        string filePath = "";
        int channel = 0;
        float volume = 1f;
        float startingvolume = 0f;
        float pitch = 1f;
        bool loop = false;
        
        AudioClip clip = null;

        var parameters = ConvertDataToParameters(data);

        parameters.TryGetValue(PARAM_TRACK, out filePath);
        clip = Resources.Load<AudioClip>(GetPathToAudio(FilePaths.resources_music, filePath));
        if (clip == null)
        {
            Debug.LogError($"Audio clip not found at path: {filePath}");
            return;
        }
        parameters.TryGetValue(PARAM_CHANNEL, out channel, defaultValue: 0);
        parameters.TryGetValue(PARAM_VOLUME, out volume, defaultValue: 1f);
        parameters.TryGetValue(PARAM_PITCH, out pitch, defaultValue: 1f);
        parameters.TryGetValue(PARAM_STARTING_VOLUME, out startingvolume, defaultValue: 0f);
        parameters.TryGetValue(PARAM_LOOP, out loop, defaultValue: false);

        AudioManager.instance.PlayTrack(clip, channel, loop, startingvolume, volume, pitch, filePath);
    }

    private static string GetPathToAudio(string defaultPath, string graphicName)
    {
        if (graphicName.StartsWith(HOME_DIRECTORY_SYMBOL))
        {
            return graphicName.Substring(HOME_DIRECTORY_SYMBOL.Length);
        }
        return defaultPath + graphicName;
    }
}
