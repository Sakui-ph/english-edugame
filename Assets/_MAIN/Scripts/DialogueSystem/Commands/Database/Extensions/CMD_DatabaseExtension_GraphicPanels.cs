using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using COMMANDS;
using GRAPHIC_PANELS;
using System;
using UnityEngine.UI;
using Unity.Mathematics;
using UnityEngine.Video;

public class CMD_DatabaseExtension_GraphicPanels : CMD_DatabaseExtension
{
    private static string[] PARAM_PANEL = new string[] { "-p", "-panel" };
    private static string[] PARAM_LAYER = new string[] { "-l", "-layer" };
    private static string[] PARAM_MEDIA = new string[] { "-m", "-media" };
    private static string[] PARAM_SPEED = new string[] { "-spd", "-speed" };
    private static string[] PARAM_IMMEDIATE = new string[] { "-i", "-immediate" };
    private static string[] PARAM_BLENDTEX = new string[] { "-b", "-blendtex" };
    private static string[] PARAM_USEAUDIO = new string[] { "-aud", "-audio" };
    private const string HOME_DIRECTORY_SYMBOL = "~/";
    new public static void Extend(CommandDatabase database)
    {
        // usage: -p (string)panelName -l (number)layerNumber -m (string)mediaName -b (string)blendTexName -i (bool)immediate -spd (number)transitionSpeed -aud (bool)useAudio
        database.AddCommand("setlayermedia", new Func<string[], IEnumerator>(SetLayerMedia));
        // usage: -p (string)panelName -l (number)layerNumber -b (string)blendTexName -i (bool)immediate -spd (number)transitionSpeed
        database.AddCommand("clearlayermedia", new Func<string[], IEnumerator>(ClearLayerMedia));
    }

    private static IEnumerator SetLayerMedia(string[] data)
    {
        // parameters available to function
        string panelName = "";
        int layer = 0;
        string mediaName = "";
        string blendTexName = "";
        float transitionSpeed = 0;
        bool immediate = false;
        bool useAudio = false;

        string pathToGraphic = "";

        UnityEngine.Object graphic = null;
        Texture blendTex = null;

        var parameters = ConvertDataToParameters(data);

        // Get all the params
        parameters.TryGetValue(PARAM_PANEL, out panelName, "");
        GraphicPanel panel = GraphicPanelManager.instance.GetPanel(panelName);
        if (panel == null)
        {
            Debug.LogError("Panel not found: " + panelName);
            yield break;
        }
        parameters.TryGetValue(PARAM_LAYER, out layer, defaultValue: 0);
        parameters.TryGetValue(PARAM_MEDIA, out mediaName);
        parameters.TryGetValue(PARAM_BLENDTEX, out blendTexName);
        parameters.TryGetValue(PARAM_IMMEDIATE, out immediate, defaultValue: false);
        if (!immediate)
            parameters.TryGetValue(PARAM_SPEED, out transitionSpeed, defaultValue: 1);
        parameters.TryGetValue(PARAM_USEAUDIO, out useAudio, defaultValue: true);

        // Logic and checks
        pathToGraphic = GetPathToGraphic(FilePaths.resources_backgroundImages, mediaName);
        graphic = Resources.Load<Texture>(pathToGraphic);
        if (graphic == null)
        {
            pathToGraphic = GetPathToGraphic(FilePaths.resources_backgroundVideos, mediaName);
            graphic = Resources.Load<VideoClip>(pathToGraphic);
        }

        if (graphic == null) 
        {
            Debug.LogError("Graphic not found: " + mediaName);
            yield break;
        }

        if (!immediate && blendTexName != string.Empty)
        {
            blendTex = Resources.Load<Texture>(FilePaths.resources_transitionEffects + blendTexName);
        }
        
        GraphicLayer graphicLayer = panel.GetLayer(layer, createIfDoesNotExist: true);

        if (graphic is Texture)
        {
            yield return graphicLayer.SetTexture(graphic as Texture, transitionSpeed, blendTex, pathToGraphic, immediate);
        }
        else
        {
            yield return graphicLayer.SetVideo(graphic as VideoClip, transitionSpeed, useAudio, blendTex, pathToGraphic, immediate);
        }


        yield return null;
    }

    private static IEnumerator ClearLayerMedia(string[] data)
    {
        string panelName = "";
        int layer = 0;
        float transitionSpeed = 0;
        bool immediate = false;
        string blendTexName = "";

        Texture blendTex = null;

        var parameters = ConvertDataToParameters(data);

        // Get all the params
        parameters.TryGetValue(PARAM_PANEL, out panelName, "");
        GraphicPanel panel = GraphicPanelManager.instance.GetPanel(panelName);
        if (panel == null)
        {
            Debug.LogError("Panel not found: " + panelName);
            yield break;
        }
        parameters.TryGetValue(PARAM_LAYER, out layer, defaultValue: -1);
        parameters.TryGetValue(PARAM_BLENDTEX, out blendTexName);
        parameters.TryGetValue(PARAM_IMMEDIATE, out immediate, defaultValue: false);
        if (!immediate)
            parameters.TryGetValue(PARAM_SPEED, out transitionSpeed, defaultValue: 1);
        

        // Logic and checks
        if (!immediate && blendTexName != string.Empty)
        {
            blendTex = Resources.Load<Texture>(FilePaths.resources_transitionEffects + blendTexName);
        }

        if (layer == -1)
        {
            panel.Clear(transitionSpeed, blendTex, immediate);
        }
        else {
            GraphicLayer graphicLayer = panel.GetLayer(layer);
            if (graphicLayer == null)
            {
                Debug.LogError("Layer not found: " + layer);
                yield break;
            }
            graphicLayer.Clear(transitionSpeed, blendTex, immediate);
        }
    }
    
    private static string GetPathToGraphic(string defaultPath, string graphicName)
    {
        if (graphicName.StartsWith(HOME_DIRECTORY_SYMBOL))
        {
            return graphicName.Substring(HOME_DIRECTORY_SYMBOL.Length);
        }
        return defaultPath + graphicName;
    }
}

