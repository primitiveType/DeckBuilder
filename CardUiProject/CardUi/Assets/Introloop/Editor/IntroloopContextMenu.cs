/* 
/// Copyright (c) 2015 Sirawat Pitaksarit, Exceed7 Experiments LP 
/// http://www.exceed7.com/introloop
*/

using UnityEditor;
using UnityEngine;
using System.IO;

namespace E7.Introloop.Editor
{
    internal class IntroloopContextMenu : MonoBehaviour
    {
        const string createPlayingScript = "Assets/Create/Introloop/IntroloopAudio";
        [MenuItem(createPlayingScript)]
        static void CreateIntroloopAudio()
        {
            string path = AssetDatabase.GetAssetPath(Selection.activeObject);
            ChangeAudioImportSettings(path);
            if (path == "")
            {
                path = "Assets";
            }
            else if (Path.GetExtension(path) != "")
            {
                path = Path.GetFileNameWithoutExtension(AssetDatabase.GetAssetPath(Selection.activeObject));
            }
            CreateIntroloopAudioAsset(path, (AudioClip)Selection.activeObject);
        }

        [MenuItem(createPlayingScript, isValidateFunction: true)]
        static bool CreateIntroloopAudioValidation()
        {
            if (Selection.activeObject != null)
            {
                return Selection.activeObject.GetType() == typeof(AudioClip);
            }
            return false;
        }

        static void ChangeAudioImportSettings(string path)
        {
            AudioImporter imp = AssetImporter.GetAtPath(path) as AudioImporter;
            imp.loadInBackground = true;
            imp.preloadAudioData = false;
            AudioImporterSampleSettings settings = imp.defaultSampleSettings;
            settings.compressionFormat = AudioCompressionFormat.Vorbis;
            imp.defaultSampleSettings = settings;
            imp.SaveAndReimport();
        }

        public static T CreateAsset<T>() where T : ScriptableObject
        {
            return CreateAsset<T>("New " + typeof(T).ToString());
        }

        public static T CreateAsset<T>(string fileName) where T : ScriptableObject
        {
            T asset = ScriptableObject.CreateInstance<T>();

            string path = AssetDatabase.GetAssetPath(Selection.activeObject);
            if (path == "")
            {
                path = "Assets";
            }
            else if (Path.GetExtension(path) != "")
            {
                path = path.Replace(Path.GetFileName(AssetDatabase.GetAssetPath(Selection.activeObject)), "");
            }

            string assetPathAndName = AssetDatabase.GenerateUniqueAssetPath(path + "/" + fileName + ".asset");

            AssetDatabase.CreateAsset(asset, assetPathAndName);

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            EditorUtility.FocusProjectWindow();
            Selection.activeObject = asset;

            return asset;
        }

        public static IntroloopAudio CreateIntroloopAudioAsset(string fileName, AudioClip audioClip)
        {
            IntroloopAudio asset = ScriptableObject.CreateInstance<IntroloopAudio>();
            asset.audioClip = audioClip;
            asset.Volume = 1;

            string path = AssetDatabase.GetAssetPath(Selection.activeObject);
            if (path == "")
            {
                path = "Assets";
            }
            else if (Path.GetExtension(path) != "")
            {
                path = path.Replace(Path.GetFileName(AssetDatabase.GetAssetPath(Selection.activeObject)), "");
            }

            string assetPathAndName = AssetDatabase.GenerateUniqueAssetPath(path + "/" + fileName + ".asset");

            AssetDatabase.CreateAsset(asset, assetPathAndName);

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            EditorUtility.FocusProjectWindow();
            Selection.activeObject = asset;

            return asset;
        }
    }
}