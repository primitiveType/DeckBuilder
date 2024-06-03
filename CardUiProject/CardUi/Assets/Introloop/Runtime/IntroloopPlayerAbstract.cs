/* 
/// Copyright (c) 2015 Sirawat Pitaksarit, Exceed7 Experiments LP 
/// http://www.exceed7.com/introloop
*/

namespace E7.Introloop
{
    /// <summary>
    /// You can subclass to make your own named singleton <see cref="IntroloopPlayer"/>.
    /// Then to access your singleton you need to use `.Get` instead of `.Instance` on your
    /// class name.
    /// </summary>
    /// <remarks>
    /// Define your class like this :
    /// 
    /// ```
    /// public class FieldBGMPlayer : IntroloopPlayer&lt;FieldBGMPlayer&gt;
    /// ```
    /// 
    /// (Put your class **itself** into the generic variable.)
    /// </remarks>
    /// <typeparam name="T">
    /// The new singleton access point <see cref="Get"/> uses a template prefab of the same name
    /// as this class name. Be sure to prepare it in `Resources/Introloop`.
    /// </typeparam>
    public abstract class IntroloopPlayer<T> : IntroloopPlayer where T : IntroloopPlayer
    {
        private static T instance;

        /// <summary>
        /// With <see cref="IntroloopPlayer.Instance"/>, it refers to the same 
        /// "default singleton instance" throughout your game.
        /// Meaning that you cannot have 2 concurrent singleton Introloop players playing+looping at the same time.
        /// 
        /// With `MySubClassOfIntroloopPlayer.Get`, it will spawns a different singleton player.
        /// 
        /// This means you can now have many Introloop playing at the same time, all of them are singletons
        /// but this time it's you who defines them.
        /// </summary>
        /// <remarks>
        /// It is useful for dividing the players into several parts. Like `BGMPlayer`, `AmbientPlayer`, etc.
        /// 
        /// Moreover, you can then define your own methods on your subclass to be more suitable for your game.
        /// Like `FieldBGMPlayer.Get.PlayDesertTheme()` instead of `IntroloopPlayer.Instance.Play(desertTheme);`
        /// 
        /// The template's name was hardcoded as the same as your class name.
        /// 
        /// If your class name is `FieldBGMPlayer` then you must have FieldBGMPlayer.prefab in 
        /// the same location as `IntroloopPlayer.prefab` in `Resources` folder.
        /// 
        /// (Defined in <see cref="IntroloopSettings.defaultTemplatePathWithoutFileName"/> as a `const string`.)
        /// </remarks>
        public static T Get
        {
            get
            {
                if (instance == null)
                {
                    instance = InstantiatePlayer<T>();
                    instance.name = IntroloopSettings.singletonObjectPrefix + instance.name;
                }
                return instance;
            }
        }

    }

}