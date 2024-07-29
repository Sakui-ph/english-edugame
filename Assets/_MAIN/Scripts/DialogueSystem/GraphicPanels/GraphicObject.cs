using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using AUDIO_SYSTEM;

namespace GRAPHIC_PANELS
{
    public class GraphicObject
    {
        private const string NAME_FORMAT = "Graphic: [{0}]";
        private const string MATERIAL_PATH = "Materials/layerTransitionMaterial";
         // Reference to variable fields of the material
        private const string MATERIAL_FIELD_COLOR = "_Color";
        private const string MATERIAL_FIELD_MAIN_TEX = "_MainTex";
        private const string MATERIAL_FIELD_BLEND_TEX = "_BlendTex";
        private const string MATERIAL_FIELD_BLEND = "_Blend";
        private const string MATERIAL_FIELD_ALPHA = "_Alpha";
        public RawImage renderer;

        private GraphicLayer layer;
        public bool isVideo {get { return video != null; } }
        private VideoPlayer video = null;
        public AudioSource audio = null;
        public string graphicPath = "";
        public string graphicName { get; private set; }
        private Coroutine thread_fadingIn = null;
        private Coroutine thread_fadingOut = null;

    
        public GraphicObject(GraphicLayer layer, string graphicPath, Texture tex, bool immediate = false) 
        {
            this.graphicPath = graphicPath;
            this.layer = layer;
            
            GameObject ob = new GameObject();
            ob.transform.SetParent(layer.panel);
            renderer = ob.AddComponent<RawImage>();

            graphicName = tex.name;
            renderer.name = string.Format(NAME_FORMAT, graphicName);
            InitGraphic(immediate);

            
            renderer.material.SetTexture(MATERIAL_FIELD_MAIN_TEX, tex);
        }

        public GraphicObject(GraphicLayer layer, string graphicPath, VideoClip clip, bool useAudio, bool immediate = false)
        {
            this.graphicPath = graphicPath;
            this.layer = layer;
            
            GameObject ob = new GameObject();
            ob.transform.SetParent(layer.panel);
            renderer = ob.AddComponent<RawImage>();

            graphicName = clip.name;
            renderer.name = string.Format(NAME_FORMAT, graphicName);
            InitGraphic(immediate);

            RenderTexture tex = new RenderTexture(Mathf.RoundToInt(clip.width), Mathf.RoundToInt(clip.height), 0);
            
            renderer.material.SetTexture(MATERIAL_FIELD_MAIN_TEX, tex);
            video = renderer.gameObject.AddComponent<VideoPlayer>();
            video.playOnAwake = true;
            video.source = VideoSource.VideoClip;
            video.clip = clip;
            video.renderMode = VideoRenderMode.RenderTexture;
            video.targetTexture = tex;
            video.isLooping = true;

            video.audioOutputMode = VideoAudioOutputMode.AudioSource;
            audio = video.gameObject.AddComponent<AudioSource>();
            audio.outputAudioMixerGroup = AudioManager.instance.sfxMixer;

            // Start with vol 0 to setup for fade in
            audio.volume = immediate? 1:  0;

            if(!useAudio)
            {
                audio.mute = true;
            }

            video.SetTargetAudioSource(0, audio);
            video.frame = 0;
            video.Prepare();
            video.Play();

            video.enabled = false;
            video.enabled = true;
        }

        private void InitGraphic(bool immediate = false)
        {
            int startingOpacity = immediate ? 1 : 0;

            // So it starts off on the panel position
            renderer.transform.localPosition = Vector3.zero;
            // So its the size of its parent
            renderer.transform.localScale = Vector3.one;
            // So the size of rect transform and anchors are also same  
            RectTransform rect = renderer.GetComponent<RectTransform>();
            rect.anchorMin = Vector2.zero;
            rect.anchorMax = Vector2.one;
            rect.offsetMin = Vector2.zero;
            rect.offsetMax = Vector2.one;

            renderer.material = GetTransitionMaterial();

            renderer.material.SetFloat(MATERIAL_FIELD_BLEND, startingOpacity);
            renderer.material.SetFloat(MATERIAL_FIELD_ALPHA, startingOpacity);
        }

        private Material GetTransitionMaterial()
        {
            Material mat = Resources.Load<Material>(MATERIAL_PATH);
            if (mat == null)
            {
                Debug.LogError("Material not found at: " + MATERIAL_PATH);
                return null;
            }

            return new Material(mat);
        }

        GraphicPanelManager panelManager => GraphicPanelManager.instance;

        public Coroutine FadeIn(float speed = 1f, Texture blend = null)
        {
            if (thread_fadingOut != null) {
                panelManager.StopCoroutine(thread_fadingOut);
            }
            if (thread_fadingIn != null) {
                return thread_fadingIn;
            }

            thread_fadingIn = panelManager.StartCoroutine(Fading(1f, speed, blend));
            return thread_fadingIn;
        }

        public Coroutine FadeOut(float speed = 1f, Texture blend = null)
        {
            if (thread_fadingIn != null) {
                panelManager.StopCoroutine(thread_fadingIn);
            }
            if (thread_fadingOut != null) {
                return thread_fadingOut;
            }

            thread_fadingOut = panelManager.StartCoroutine(Fading(0f, speed, blend));
            return thread_fadingOut;    
        }

        private IEnumerator Fading(float target, float speed, Texture blend)
        {
            bool isBlending = blend != null;
            bool fadingIn = target > 0;

            renderer.material.SetTexture(MATERIAL_FIELD_BLEND_TEX, blend);
            renderer.material.SetFloat(MATERIAL_FIELD_ALPHA, isBlending ? 1 : fadingIn ? 0 : 1);
            renderer.material.SetFloat(MATERIAL_FIELD_BLEND, isBlending ? fadingIn ? 0 : 1 : 1);

            string opacityParam = isBlending? MATERIAL_FIELD_BLEND : MATERIAL_FIELD_ALPHA;

            while (renderer.material.GetFloat(opacityParam) != target)
            {
                float opacity = Mathf.MoveTowards(renderer.material.GetFloat(opacityParam), target, speed * Time.deltaTime * GraphicPanelManager.DEFAULT_TRANSITION_SPEED);
                renderer.material.SetFloat(opacityParam, opacity);

                if(isVideo)
                {
                    audio.volume = opacity;
                }

                yield return null;
            }

            thread_fadingIn = null;
            thread_fadingOut = null;

            if (target == 0)
                Destroy();
            else 
                DestroyBackgroundGraphicsOnLayer();
        }

        public void Destroy()
        {
            if (layer.currentGraphic != null && layer.currentGraphic.renderer == renderer)
            {
                layer.currentGraphic = null;
            }
            Object.Destroy(renderer.gameObject);
        }

        private void DestroyBackgroundGraphicsOnLayer()
        {
            layer.DestroyOldGraphics();
        }

    }
}
