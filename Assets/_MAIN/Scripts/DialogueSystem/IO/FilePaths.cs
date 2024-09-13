using UnityEngine;

public class FilePaths
{
    public static readonly string HOME_DIRECTORY_SYMBOL = "~/";
    public static readonly string root = $"{Application.dataPath}/gameData/";
    public static readonly string stream_assets_branch = $"{Application.streamingAssetsPath}/levels/";
    public static readonly string better_stream_assets_levels = "levels/";
    // Resources Paths
    public static readonly string resources_graphics = "Graphics/";
    public static readonly string resources_backgroundImages = $"{resources_graphics}BG Images/";
    public static readonly string resources_backgroundVideos = $"{resources_graphics}BG Videos/";
    public static readonly string resources_transitionEffects = $"{resources_graphics}Transition Effects/";
    public static readonly string resources_scene_transitions = $"{resources_graphics}Scene Transitions/";

    public static readonly string resources_audio = "Audio/";
    public static readonly string resources_music = $"{resources_audio}Music/";
    public static readonly string resources_sfx = $"{resources_audio}SFX/";
    public static readonly string resources_voices = $"{resources_audio}Voices/";

    public static readonly string interrogation_files = "InterrogationFiles/";
    public static readonly string dialogue_files = "DialogueFiles/";

    public static readonly string branch_files = "Levels/";
    
}
