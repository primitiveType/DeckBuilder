/* 
/// Copyright (c) 2015 Sirawat Pitaksarit, Exceed7 Experiments LP 
/// http://www.exceed7.com/introloop
*/

using UnityEngine;
using UnityEngine.Audio;
using System.Collections;
using System.Collections.Generic;
using System;

namespace E7.Introloop
{
    /// <summary>
    /// A component that coordinates 4 <see cref="AudioSource"/> together with scheduling methods 
    /// to achieve gapless looping music with intro section.
    /// </summary>
    /// <remarks>
    /// 2 <see cref="AudioSource"/> uses scheduling methods to stitch up audio precisely, while the other 2 sources
    /// are there to support cross fading to a new Introloop audio. Potentially a moment when all 4 sources
    /// are playing at the same time is possible. (e.g. one introlooping audio at the seam, while being tasked to 
    /// cross fade into an another introloop audio that starts near the seam.)
    /// </remarks>
    public class IntroloopPlayer : MonoBehaviour
    {
        private IntroloopTrack[] twoTracks = new IntroloopTrack[2];
        private float[] towardsVolume = new float[2];
        private bool[] willStop = new bool[2];
        private bool[] willPause = new bool[2];
        private float[] fadeLength = new float[2];
        private IntroloopAudio previousPlay;

        private AudioSource templateSource;

        /// <summary>
        /// An <see cref="AudioSource"/> connected to the same object as the player is a template source,
        /// which will not actually be used at runtime but its settings will be copied to all underlying
        /// 4 <see cref="AudioSource"/> on `Start()` of the player component.
        /// </summary>
        /// <remarks>
        /// If you want to batch change all 4 underlying <see cref="AudioSource"/> again (maybe you missed the `Start()`),
        /// you can set things up on the template source then use <see cref="ApplyAudioSourceCharacteristics(AudioSource)"/>
        /// with input argument as this property.
        /// </remarks>
        public AudioSource TemplateSource
        {
            get
            {
                if (templateSource == null)
                {
                    templateSource = GetComponent<AudioSource>();
                    if (templateSource == null)
                    {
                        //If no template source, make it a 2D source by default.
                        templateSource = gameObject.AddComponent<AudioSource>();
                        SetupDefaultAudioSourceForIntroloop(templateSource);
                    }
                }

                return templateSource;
            }
        }

        /// <summary>
        /// If you wish to do something that affects all 4 <see cref="AudioSource"/> that Introloop utilize at once, 
        /// do a `foreach` on this property.
        /// </summary>
        /// <remarks>
        /// You should not use this in `Awake`, as Introloop might still not yet spawn the <see cref="AudioSource"/>.
        /// </remarks>
        public IEnumerable<AudioSource> InternalAudioSources
        {
            get
            {
                if (twoTracks == null)
                {
                    throw new IntroloopException(
                        "Child game objects of Introloop player is not yet initialized. Please avoid accessing internal AudioSource on Awake.");
                }

                foreach (AudioSource aSource in twoTracks[0].AllAudioSources)
                {
                    yield return aSource;
                }

                foreach (AudioSource aSource in twoTracks[1].AllAudioSources)
                {
                    yield return aSource;
                }
            }
        }

        /// <summary>
        /// It will change to 0 on first <see cref="Play"/>. 0 is the first track.
        /// </summary>
        private int currentTrack = 1;

        /// <summary>
        /// This fade is inaudible, it helps removing loud pop/click when you stop song suddenly.
        /// This is used automatically when you <see cref="Stop"/>.
        /// If you really don't want this, use <see cref="StopFade(float)"/> with 0 second fade length.
        /// </summary>
        private const float popRemovalFadeTime = 0.055f;

#pragma warning disable 0649
        [SerializeField] private IntroloopSettings introloopSettings;
#pragma warning restore 0649

        private static IntroloopPlayer instance;

        /// <summary>
        /// Access a convenient singleton player.
        /// </summary>
        /// <remarks>
        /// This is instantiated from a prefab named `IntroloopPlayer.prefab` in your `Resources/Introloop` folder
        /// (by default).
        /// 
        /// It should be setup as a fully 2D player so you can use it to play music.
        /// In most cases this is all you need. However Introloop also supports multiple players and positional players.
        /// </remarks>
        public static IntroloopPlayer Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = InstantiatePlayer<IntroloopPlayer>();
                    instance.name = IntroloopSettings.singletonObjectPrefix + instance.name;
                }

                return instance;
            }
        }

        /// <summary>
        /// Newly instantate all required `GameObject` chain with components wired up completely
        /// and return a reference to the player.
        /// 
        /// This player will have <see cref="UnityEngine.Object.DontDestroyOnLoad(UnityEngine.Object)"/> applied.
        /// </summary>
        /// <remarks>
        /// It uses a prefab named the same as <typeparamref name="T"/> located in `Resources/Introloop`
        /// as a blueprint. All 4 underlying <see cref="AudioSource"/> will have the same setup.
        /// 
        /// The <see cref="Instance"/> `static` access point uses this internally.
        /// With this, you can use the template prefab really as a template to create
        /// any amount of players as you like. They will not be associated with the
        /// singleton instance.
        /// </remarks>
        /// <returns>Reference to the player component on the newly instantiated player.</returns>
        public static T InstantiatePlayer<T>() where T : IntroloopPlayer
        {
            System.Type type = typeof(T);
            string typeString = type.Name;

            //Try loading a template prefab by matching name
            string path = IntroloopSettings.defaultTemplatePathWithoutFileName + typeString;

            GameObject templatePrefab = Resources.Load<GameObject>(path);

            //With or without template prefab, we new the game object.

            GameObject newIntroloopPlayerObject;
            T playerComponent;
            AudioSource templateAudioSource;
            if (templatePrefab != null)
            {
                newIntroloopPlayerObject = Instantiate(templatePrefab);
                playerComponent = newIntroloopPlayerObject.GetComponent<T>();
                templateAudioSource = newIntroloopPlayerObject.GetComponent<AudioSource>();

                //Can add a player in case that it is missing on the template prefab,
                //the template could contain only AudioSource for characteristic copy
                //if you want.
                if (playerComponent == null)
                {
                    playerComponent = newIntroloopPlayerObject.AddComponent<T>();
                }

                if (templateAudioSource == null)
                {
                    templateAudioSource = newIntroloopPlayerObject.AddComponent<AudioSource>();
                    SetupDefaultAudioSourceForIntroloop(templateAudioSource);
                }
            }
            else
            {
                newIntroloopPlayerObject = new GameObject(typeString);
                playerComponent = newIntroloopPlayerObject.AddComponent<T>();
                templateAudioSource = newIntroloopPlayerObject.AddComponent<AudioSource>();
                SetupDefaultAudioSourceForIntroloop(templateAudioSource);
            }

            DontDestroyOnLoad(newIntroloopPlayerObject);

            playerComponent.CreateImportantChildren();

            //The template audio source's character will propagate on this new object's Start.
            return playerComponent;
        }

        void Awake()
        {
            if (introloopSettings == null)
            {
                introloopSettings = new IntroloopSettings();
            }

            CreateImportantChildren();
        }

        void Start()
        {
            //RequireComponent is not available at Awake.
            TemplateSource.enabled = false;
            this.ApplyAudioSourceCharacteristics(TemplateSource);
        }

        private bool importantChildrenCreated = false;

        private void CreateImportantChildren()
        {
            if (!importantChildrenCreated)
            {
                // These are all the components that make this plugin works. Basically 4 AudioSources with special control script
                // to juggle music file carefully, stop/pause/resume gracefully while Introloop-ing.

                Transform musicPlayerTransform = transform;
                GameObject musicTrack1 = new GameObject();
                musicTrack1.AddComponent<IntroloopTrack>();
                musicTrack1.name = "IntroloopTrack 1";
                musicTrack1.transform.parent = musicPlayerTransform;
                musicTrack1.transform.localPosition = Vector3.zero;
                twoTracks[0] = musicTrack1.GetComponent<IntroloopTrack>();
                twoTracks[0].introloopSettings = this.introloopSettings;

                GameObject musicTrack2 = new GameObject();
                musicTrack2.AddComponent<IntroloopTrack>();
                musicTrack2.name = "IntroloopTrack 2";
                musicTrack2.transform.parent = musicPlayerTransform;
                musicTrack2.transform.localPosition = Vector3.zero;
                twoTracks[1] = musicTrack2.GetComponent<IntroloopTrack>();
                twoTracks[1].introloopSettings = this.introloopSettings;

                importantChildrenCreated = true;
            }
        }

        private static void SetupDefaultAudioSourceForIntroloop(AudioSource audioSource)
        {
            audioSource.spatialBlend = 0;
            audioSource.priority = 0;
        }

        private void Update()
        {
            FadeUpdate();
            twoTracks[0].SchedulingUpdate();
            twoTracks[1].SchedulingUpdate();
        }

        private void FadeUpdate()
        {
            //For two main tracks
            for (int i = 0; i < 2; i++)
            {
                float towardsVolumeBgmVolumeApplied = towardsVolume[i];
                if (twoTracks[i].FadeVolume != towardsVolumeBgmVolumeApplied)
                {
                    //Handles the fade in/out
                    if (fadeLength[i] == 0)
                    {
                        twoTracks[i].FadeVolume = towardsVolumeBgmVolumeApplied;
                    }
                    else
                    {
                        if (twoTracks[i].FadeVolume > towardsVolumeBgmVolumeApplied)
                        {
                            twoTracks[i].FadeVolume -= Time.unscaledDeltaTime / fadeLength[i];
                            if (twoTracks[i].FadeVolume <= towardsVolumeBgmVolumeApplied)
                            {
                                //Stop the fade
                                twoTracks[i].FadeVolume = towardsVolumeBgmVolumeApplied;
                            }
                        }
                        else
                        {
                            twoTracks[i].FadeVolume += Time.unscaledDeltaTime / fadeLength[i];
                            if (twoTracks[i].FadeVolume >= towardsVolumeBgmVolumeApplied)
                            {
                                //Stop the fade
                                twoTracks[i].FadeVolume = towardsVolumeBgmVolumeApplied;
                            }
                        }
                    }

                    //Stop check
                    if (willStop[i] && twoTracks[i].FadeVolume == 0)
                    {
                        willStop[i] = false;
                        willPause[i] = false;
                        twoTracks[i].Stop();
                        UnloadTrack(i);
                    }

                    //Pause check
                    if (willPause[i] && twoTracks[i].FadeVolume == 0)
                    {
                        willStop[i] = false;
                        willPause[i] = false;
                        twoTracks[i].Pause();
                        //don't unload!
                    }
                }
            }
        }

        private void UnloadTrack(int trackNumber)
        {
            //Have to check if other track is using the music or not?

            //If playing the same song again,
            //the loading of the next song might come earlier, then got immediately unloaded by this.

            //Also check for when using different IntroloopAudio with the same source file.
            //In this case .Music will be not equal, but actually the audioClip inside is the same song.

            //Note that load/unloading has no effect on "Streaming" audio type.

            bool musicEqualCurrent = (twoTracks[trackNumber].Music == twoTracks[(trackNumber + 1) % 2].Music);
            bool clipEqualCurrent = (twoTracks[trackNumber].Music != null && twoTracks[(trackNumber + 1) % 2].Music != null) &&
                                    (twoTracks[trackNumber].Music.AudioClip == twoTracks[(trackNumber + 1) % 2].Music.AudioClip);

            //As = AudioSource
            bool isSameSongAsCurrent = musicEqualCurrent || clipEqualCurrent;

            bool musicEqualNext = (twoTracks[trackNumber].Music == twoTracks[(trackNumber + 1) % 2].MusicAboutToPlay);
            bool clipEqualNext = (twoTracks[trackNumber].Music != null && twoTracks[(trackNumber + 1) % 2].MusicAboutToPlay != null) &&
                                 (twoTracks[trackNumber].Music.AudioClip == twoTracks[(trackNumber + 1) % 2].MusicAboutToPlay.AudioClip);

            bool isSameSongAsAboutToPlay = musicEqualNext || clipEqualNext;

            bool usingAndPlaying = twoTracks[(trackNumber + 1) % 2].IsPlaying && isSameSongAsCurrent;

            if (!usingAndPlaying && !isSameSongAsAboutToPlay)
            {
                //If not, it is now safe to unload it
                //Debug.Log("Unloading");
                twoTracks[trackNumber].Unload();
            }
        }

        internal void ApplyVolumeSettingToAllTracks()
        {
            twoTracks[0].ApplyVolume();
            twoTracks[1].ApplyVolume();
        }

        /// <summary>
        /// Play an <see cref="IntroloopAudio"/> asset. 
        /// </summary>
        /// <remarks>
        /// It applies <see cref="IntroloopAudio.Volume"/> and <see cref="IntroloopAudio.Pitch"/> 
        /// to the underlying <see cref="AudioSource"/>.
        /// 
        /// If an another <see cref="IntroloopAudio"/> is playing on this player, 
        /// it could cross-fade between the two if <paramref name="fadeLengthSeconds"/> is provided.
        /// 
        /// The faded out audio will be unloaded automatically once the fade is finished.
        /// </remarks>
        /// <param name="audio"> An <see cref="IntroloopAudio"/> asset file to play.</param>
        /// <param name="fadeLengthSeconds">
        /// Fade in/out length to use in seconds.
        /// 
        /// - If 0, it uses a small pop removal fade time.
        /// - If negative, it is immediate.
        /// 
        /// The audio will be unloaded only after it had fade out completely.
        /// </param>
        /// <param name="startTime">
        /// Specify starting point in time instead of starting from the beginning. 
        /// 
        /// The time you specify here will be converted to "playhead time", Introloop will make the playhead 
        /// at the point in time as if you had played for this amount of time before starting. 
        /// Since <see cref="IntroloopAudio"/> conceptually has infinite length, any number that is over looping boundary 
        /// will be wrapped over to the intro boundary in the calculation. (Except that if the audio is non-looping) 
        /// 
        /// The time specified here is **not** taking <see cref="IntroloopAudio.Pitch"/> into account. 
        /// It's an elapsed time as if <see cref="IntroloopAudio.Pitch"/> is 1.
        /// </param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="audio"/> is `null`.</exception>
        public void Play(IntroloopAudio audio, float fadeLengthSeconds = 0, float startTime = 0)
        {
            if (audio == null)
            {
                throw new ArgumentNullException("audio", "Played IntroloopAudio is null!");
            }

            //Auto-crossfade old ones. If no fade length specified, a very very small fade will be used to avoid pops/clicks.
            Stop(fadeLengthSeconds);

            int next = (currentTrack + 1) % 2;
            twoTracks[next].Play(audio, fadeLengthSeconds == 0 ? false : true, startTime);
            towardsVolume[next] = 1;
            fadeLength[next] = TranslateFadeLength(fadeLengthSeconds);

            currentTrack = next;
            this.previousPlay = audio;
        }


        /// <summary>
        /// Play an <see cref="IntroloopAudio"/> asset. 
        /// Works like <see cref="Play(IntroloopAudio, float, float)"/> but this allows it to be use as a delegate target in editor.
        /// 
        /// The requirement for that is there must be at most 1 argument. Therefore this way you cannot
        /// specify fade length or start time.
        /// </summary>
        /// <remarks>
        /// For example you can connect any <see cref="UnityEngine.Events.UnityEvent"/> and select `Play(IntroloopAudio)`
        /// from the drop down, then you will see a slot of `IntroloopAudio` to connect the asset.
        /// 
        /// It applies <see cref="IntroloopAudio.Volume"/> and <see cref="IntroloopAudio.Pitch"/> 
        /// to the underlying <see cref="AudioSource"/>.
        /// 
        /// If an another <see cref="IntroloopAudio"/> is playing on this player, 
        /// it could cross-fade between the two if <paramref name="fadeLengthSeconds"/> is provided.
        /// </remarks>
        /// <param name="audio"> An <see cref="IntroloopAudio"/> asset file to play.</param>
        public void Play(IntroloopAudio audio)
        {
            Play(audio, fadeLengthSeconds: 0, startTime: 0);
        }

        /// <summary>
        /// Move the playhead of the currently playing audio to anywhere in terms of elapsed time.
        /// 
        /// - If it is currently playing, you can instantly move the playhead position to anywhere else.
        /// - If it is not playing, no effect. (This includes while in paused state, you cannot seek in paused state.)
        /// </summary>
        /// <remarks>
        /// An internal implementation is not actually a seek, but a completely new <see cref="Play(IntroloopAudio, float, float)"/>
        /// with the previous <see cref="IntroloopAudio"/>. This is why you cannot seek while in pause, as it actually
        /// do a new play for you.
        /// 
        /// It is handy because it doesn't require you to remember and specify that audio again.
        /// </remarks>
        /// <param name="elapsedTime">
        /// Introloop will make the playhead at the point in time as if you had played for this amount 
        /// of time before starting. 
        /// 
        /// The time you specify here will be converted to "playhead time", Introloop will make the playhead 
        /// at the point in time as if you had played for this amount of time before starting. 
        /// Since <see cref="IntroloopAudio"/> conceptually has infinite length, any number that is over looping boundary 
        /// will be wrapped over to the intro boundary in the calculation. (Except that if the audio is non-looping) 
        /// 
        /// The time specified here is **not** taking <see cref="IntroloopAudio.Pitch"/> into account. 
        /// It's an elapsed time as if <see cref="IntroloopAudio.Pitch"/> is 1.
        /// </param>
        public void Seek(float elapsedTime)
        {
            if (twoTracks[currentTrack].IsPlaying)
            {
                twoTracks[currentTrack].Play(previousPlay, isFade: false, startTime: elapsedTime);
                towardsVolume[currentTrack] = 1;
                fadeLength[currentTrack] = 0;
            }
        }

        /// <summary>
        /// Stop the currently playing <see cref="IntroloopAudio"/> immediately and unload it from memory.
        /// </summary>
        public void Stop()
        {
            willStop[currentTrack] = false;
            willPause[currentTrack] = false;
            fadeLength[currentTrack] = 0;
            twoTracks[currentTrack].FadeVolume = 0;
            twoTracks[currentTrack].Stop();
            UnloadTrack(currentTrack);
        }

        /// <summary>
        /// Fading out to stop the currently playing <see cref="IntroloopAudio"/>, and unload it from memory 
        /// once it is completely faded out.
        /// </summary>
        /// <param name="fadeLengthSeconds">
        /// Fade out length to use in seconds.
        /// - 0 is a special value that will still apply small pop removal fade time.
        /// - If negative, this method works like <see cref="Stop"/> overload.
        /// </param>
        public void Stop(float fadeLengthSeconds)
        {
            if (fadeLengthSeconds < 0)
            {
                Stop();
            }
            else
            {
                willStop[currentTrack] = true;
                willPause[currentTrack] = false;
                fadeLength[currentTrack] = TranslateFadeLength(fadeLengthSeconds);
                towardsVolume[currentTrack] = 0;
            }
        }

        /// <summary>
        /// Pause the currently playing <see cref="IntroloopAudio"/> immediately without unloading.
        /// Call <see cref="Resume(float)"/> to continue playing.
        /// </summary>
        public void Pause()
        {
            if (twoTracks[currentTrack].IsPausable())
            {
                willStop[currentTrack] = false;
                willPause[currentTrack] = false;
                fadeLength[currentTrack] = 0;
                twoTracks[currentTrack].FadeVolume = 0;
                twoTracks[currentTrack].Pause();
            }
        }

        /// <summary>
        /// Fading out to pause the currently playing <see cref="IntroloopAudio"/> without unloading.
        /// Call <see cref="Resume(float)"/> to continue playing.
        /// </summary>
        /// <param name="fadeLengthSeconds">Fade out length to use in seconds.
        /// - 0 is a special value that will still apply small pop removal fade time. 
        /// - If negative, this method works like <see cref="Pause"/> overload.
        /// </param>
        public void Pause(float fadeLengthSeconds)
        {
            if (twoTracks[currentTrack].IsPausable())
            {
                if (fadeLengthSeconds < 0)
                {
                    Pause();
                }
                else
                {
                    willPause[currentTrack] = true;
                    willStop[currentTrack] = false;
                    fadeLength[currentTrack] = TranslateFadeLength(fadeLengthSeconds);
                    ;
                    towardsVolume[currentTrack] = 0;
                }
            }
        }

        /// <summary>
        /// Resume playing of previously paused (<see cref="Pause(float)"/>) <see cref="IntroloopAudio"/>.
        /// If currently not pausing, it does nothing.
        /// </summary>
        /// <remarks>
        /// Note that if it is currently "fading to pause", the state is not considered paused 
        /// yet so you can't resume in that time.
        /// </remarks>
        /// <param name="fadeLengthSeconds">Fade out length to use in seconds. 
        /// - If 0, it uses a small pop removal fade time. 
        /// - If negative, it resumes immediately.
        /// </param>
        public void Resume(float fadeLengthSeconds = 0)
        {
            if (twoTracks[currentTrack].Resume())
            {
                //Resume success
                willStop[currentTrack] = false;
                willPause[currentTrack] = false;
                towardsVolume[currentTrack] = 1;
                fadeLength[currentTrack] = TranslateFadeLength(fadeLengthSeconds);
            }
        }

        /// <summary>
        /// Zero length is a special value that equals pop removal small fade time.
        /// Negative length is a special value that equals (real) 0.
        /// </summary>
        private float TranslateFadeLength(float fadeLength)
        {
            return fadeLength > 0 ? fadeLength : fadeLength < 0 ? 0 : popRemovalFadeTime;
        }

        /// <summary>
        /// An experimental feature in the case that you really want the audio to start 
        /// in an instant you call <see cref="Play(IntroloopAudio, float, float)"/>. You must use the same
        /// <see cref="IntroloopAudio"/> that you preload in the next play.
        /// </summary>
        /// <remarks>
        /// By normally using <see cref="Play(IntroloopAudio, float, float)"/> and <see cref="Stop(float)"/> 
        /// it loads the audio the moment you called <see cref="Play(IntroloopAudio, float, float)"/>. 
        /// 
        /// Introloop waits for an audio to load before playing with a coroutine. 
        /// (If you have <see cref="AudioClip.loadInBackground"/> in the import settings, 
        /// otherwise <see cref="Play(IntroloopAudio, float, float)"/> will be a blocking call)
        /// 
        /// Introloop can't guarantee that the playback will be instant but your game can continue while it is loading.
        /// 
        /// By using this method before actually calling <see cref="Play(IntroloopAudio, float, float)"/> 
        /// it will instead be instant. This function is special that even songs with <see cref="AudioClip.loadInBackground"/> 
        /// can be loaded in a blocking fashion. (You can put <see cref="Play(IntroloopAudio, float, float)"/> immediately 
        /// in the next line expecting a fully loaded audio)
        /// 
        /// However be aware that memory is managed less efficiently in the following case : 
        /// 
        /// Normally Introloop immediately unloads the previous track to minimize memory, 
        /// but if you use <see cref="Preload(IntroloopAudio)"/> then  did not call 
        /// <see cref="Play(IntroloopAudio, float, float)"/> with the same <see cref="IntroloopAudio"/> afterwards, 
        /// the loaded memory will be unmanaged. 
        /// 
        /// (Just like if you tick <see cref="AudioClip.preloadAudioData"/> on your clip and have them 
        /// in a hierarchy somewhere, then did not use it.)
        /// 
        /// Does not work with <see cref="AudioClipLoadType.Streaming"/> audio loading type.
        /// </remarks>
        public void Preload(IntroloopAudio audio)
        {
            audio.Preload();
        }

        /// <summary>
        /// This interpretation of a play time could decrease when it goes over looping boundary back to intro boundary.
        /// Conceptually Introloop audio has infinite length, so this time is a bit different from normal sense.
        /// </summary>
        /// <remarks>
        /// Think as it as not "elapsed time" but rather the position of the actual playhead, 
        /// expressed in time as if the pitch is 1.
        /// 
        /// For example with pitch enabled, the playhead will move slowly, 
        /// and so the time returned from this method respect that slower playhead.
        /// 
        /// It is usable with <see cref="Play(IntroloopAudio, float, float)"/> as a start time 
        /// to "restore" the play from remembered time. With only 1 <see cref="IntroloopPlayer"/> you can stop and
        /// unload previous song then continue later after reloading it.
        /// 
        /// Common use case includes battle music which resumes the field music afterwards. 
        /// If the battle is memory consuming unloading the field music could help.
        /// </remarks>
        public float GetPlayheadTime()
        {
            return twoTracks[currentTrack].PlayheadPositionSeconds;
        }

        /// <summary>
        /// Assign a different audio mixer group to all underlying <see cref="AudioSource"/>.
        /// The mixer group starts with what you assigned on the template prefab in `Resources/Introloop`.
        /// </summary>
        public void SetMixerGroup(AudioMixerGroup audioMixerGroup)
        {
            foreach (AudioSource aSource in InternalAudioSources)
            {
                aSource.outputAudioMixerGroup = audioMixerGroup;
            }
        }

        /// <summary>
        /// All 4 underlying <see cref="AudioSource"/> will have their settings to be like <paramref name="applyFrom"/>.
        /// </summary>
        /// <remarks>
        /// This includes the <see cref="AudioMixerGroup"/> since it is one of a property on the source.
        /// </remarks>
        public void ApplyAudioSourceCharacteristics(AudioSource applyFrom)
        {
            foreach (AudioSource aSource in InternalAudioSources)
            {
                ApplyAudioSourceCharacteristicsInternal(applyTarget: aSource, applyFrom: applyFrom);
            }
        }

        private static void ApplyAudioSourceCharacteristicsInternal(AudioSource applyTarget, AudioSource applyFrom)
        {
            applyTarget.outputAudioMixerGroup = applyFrom.outputAudioMixerGroup;

            applyTarget.SetCustomCurve(AudioSourceCurveType.CustomRolloff, applyFrom.GetCustomCurve(AudioSourceCurveType.CustomRolloff));
            applyTarget.SetCustomCurve(AudioSourceCurveType.ReverbZoneMix, applyFrom.GetCustomCurve(AudioSourceCurveType.ReverbZoneMix));
            applyTarget.SetCustomCurve(AudioSourceCurveType.SpatialBlend, applyFrom.GetCustomCurve(AudioSourceCurveType.SpatialBlend));
            applyTarget.SetCustomCurve(AudioSourceCurveType.Spread, applyFrom.GetCustomCurve(AudioSourceCurveType.Spread));

            applyTarget.ignoreListenerVolume = applyFrom.ignoreListenerVolume;
            applyTarget.ignoreListenerPause = applyFrom.ignoreListenerPause;
            applyTarget.velocityUpdateMode = applyFrom.velocityUpdateMode;
            applyTarget.panStereo = applyFrom.panStereo;
            applyTarget.spatialBlend = applyFrom.spatialBlend;
            applyTarget.spatialize = applyFrom.spatialize;
            applyTarget.spatializePostEffects = applyFrom.spatializePostEffects;
            applyTarget.reverbZoneMix = applyFrom.reverbZoneMix;
            applyTarget.bypassEffects = applyFrom.bypassEffects;
            applyTarget.bypassListenerEffects = applyFrom.bypassListenerEffects;
            applyTarget.bypassReverbZones = applyFrom.bypassReverbZones;
            applyTarget.dopplerLevel = applyFrom.dopplerLevel;
            applyTarget.spread = applyFrom.spread;
            applyTarget.priority = applyFrom.priority;
            applyTarget.mute = applyFrom.mute;
            applyTarget.minDistance = applyFrom.minDistance;
            applyTarget.maxDistance = applyFrom.maxDistance;
        }

        /// <summary>
        /// Each player contains 4 <see cref="AudioSource"/>, this method
        /// returns the current information of the first pair for debugging purpose.
        /// </summary>
        public string[] GetDebugStringsTrack1()
        {
            return twoTracks[0].DebugInformation;
        }

        /// <summary>
        /// Each player contains 4 <see cref="AudioSource"/>, this method
        /// returns the current information of the second pair for debugging purpose.
        /// </summary>
        public string[] GetDebugStringsTrack2()
        {
            return twoTracks[1].DebugInformation;
        }

#if UNITY_2019_1_OR_NEWER
        private float timeBeforePause;
        /// <summary>
        /// This is a dirty workaround for the bug in 2019.1+ where on game minimize or <see cref="AudioListener"/> pause,
        /// All <see cref="AudioSource.SetScheduledEndTime(double)"/> will be lost. I confirmed it is not a problem in 2018.4 LTS.
        /// 
        /// The ideal fix is to call Pause just before the game goes to minimize then Resume after we comeback to reschedule.
        /// However at this callback Pause does not work, as all audio are already on its way to pausing.
        /// 
        /// So an another approach is that we will remember the time just before the pause, and the play again
        /// after coming back using that time. The Seek method can be used instead of Play here so you don't have to specify the
        /// previous audio.
        /// 
        /// Please see : https://forum.unity.com/threads/introloop-easily-play-looping-music-with-intro-section-v4-0-0-2019.378370/#post-4793741
        /// Track the case here : https://fogbugz.unity3d.com/default.asp?1151637_4i53coq9v07qctp1
        /// </summary>
        public void OnApplicationPause(bool paused)
        {
            if (paused)
            {
                timeBeforePause = this.GetPlayheadTime();
            }
            else
            {
                this.Seek(timeBeforePause);
            }
        }
#endif
    }
}
