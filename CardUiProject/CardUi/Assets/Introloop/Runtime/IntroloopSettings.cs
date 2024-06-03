/* 
/// Copyright (c) 2015 Sirawat Pitaksarit, Exceed7 Experiments LP 
/// http://www.exceed7.com/introloop
*/

using System;
using UnityEngine;
using UnityEngine.Audio;

namespace E7.Introloop
{
    /// <summary>
    /// Allows you to set up something based on assets independent of the scene, such as the target audio mixer.
    /// This class could serialize them together with the template prefab in your `Resources` folder.
    /// </summary>
    [Serializable]
    internal class IntroloopSettings 
    {
        ///<summary>
        ///This is the path of <see cref="IntroloopPlayer"/> or <see cref="IntroloopPlayer{T}"/> 
        /// template relative to `Resources` folder.
        ///</summary>
        internal const string defaultTemplatePathWithoutFileName = "Introloop/";

        /// <summary>
        /// When using <see cref="IntroloopPlayer.Instance"/> or <see cref="IntroloopPlayer{T}.Get"/> for the first time,
        /// a new game object in `DontDestroyOnLoad` scene will have its name prefixed with this.
        /// </summary>
        internal const string singletonObjectPrefix = "Singleton-";

#pragma warning disable 0649
        ///<summary>
        /// Check this in your <see cref="IntroloopPlayer"/> template to log various debug data.
        ///</summary>
        public bool logInformation;
#pragma warning restore 0649
    }

}