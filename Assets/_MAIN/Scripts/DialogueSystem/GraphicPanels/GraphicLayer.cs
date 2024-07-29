using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Video;

namespace GRAPHIC_PANELS
{
    public class GraphicLayer
    {
        public const string LAYER_OBJECT_NAME_FORMAT = "Layer: {0}";
        public int layerDepth = 0;
        public Transform panel;


        public GraphicObject currentGraphic = null;
        private List<GraphicObject> oldGraphics = new List<GraphicObject>();

        public Coroutine SetTexture(string filePath, float transitionSpeed = 1f, Texture blendingTexture = null, bool immediate = false)
        {
            // Load all the images we want to create for the BGs.
            Texture tex = Resources.Load<Texture>(filePath);

            if (tex == null) 
            {
                Debug.LogError("Texture not found at: " + filePath);
                return null;
            }

            return SetTexture(tex, transitionSpeed, blendingTexture, filePath, immediate: immediate);
        }

        public Coroutine SetTexture(Texture tex, float transitionSpeed = 1f, Texture blendingTexture = null, string filePath = "", bool immediate = false)
        {
            if (tex == null)
            {
                Debug.LogError("Texture is null");
                return null;
            }

            return CreateGraphic(tex, transitionSpeed, filePath, blendingTexture: blendingTexture, immediate: immediate);
        }

        public Coroutine SetVideo(string filePath, float transitionSpeed = 1f, bool useAudioForVideo = true, Texture blendingTexture = null, bool immediate = false)
        {
            VideoClip clip = Resources.Load<VideoClip>(filePath);

            if (clip == null) 
            {
                Debug.LogError("Could not load video at: " + filePath);
                return null;
            }

            return SetVideo(clip, transitionSpeed, useAudioForVideo, blendingTexture, filePath, immediate: immediate);
        }

        public Coroutine SetVideo(VideoClip video, float transitionSpeed = 1f, bool useAudioForVideo = true, Texture blendingTexture = null, string filePath = "", bool immediate = false)
        {
            if (video == null)
            {
                Debug.LogError("Texture is null");
                return null;
            }

            return CreateGraphic(video, transitionSpeed, filePath, useAudioForVideo, blendingTexture: blendingTexture, immediate: immediate);
        }



        // Makes textures or video so we pass T for type
        private Coroutine CreateGraphic<T>(T graphicData, float transitionSpeed, string filePath,  bool useAudioForVideo = false, Texture blendingTexture = null, bool immediate = false)
        {
            GraphicObject newGraphic = null;

            if (graphicData is Texture)
            {
                newGraphic = new GraphicObject(this, filePath, graphicData as Texture, immediate);
            }
            else if (graphicData is VideoClip)
            {
                newGraphic = new GraphicObject(this, filePath, graphicData as VideoClip, useAudioForVideo, immediate);
            }
            else
            {
                Debug.LogError("Graphic type not supported");
                return null;
            }

            if (currentGraphic != null && !oldGraphics.Contains(currentGraphic))
                oldGraphics.Add(currentGraphic);
            
            currentGraphic = newGraphic;
            if (!immediate)
                return currentGraphic.FadeIn(transitionSpeed, blendingTexture);
            
            DestroyOldGraphics();
            return null;
        }

        public void DestroyOldGraphics()
        {
            foreach (var graphic in oldGraphics)
            {
                Object.Destroy(graphic.renderer.gameObject);
            }
            oldGraphics.Clear();
        }

        public void Clear(float speed = 1f, Texture blend = null, bool immediate = false)
        {
            if (currentGraphic != null)
                if (!immediate)
                    currentGraphic.FadeOut(speed, blend);
                else
                    currentGraphic.Destroy();
            
            foreach (var graphic in oldGraphics)
            {
                if (!immediate)
                    graphic.FadeOut(speed, blend);
                else
                    graphic.Destroy();
            }
        }
    }
}