using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.iOS.Xcode;
using UnityEditor.XR.VisionOS;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace Plugins.RichFramework.Editor
{
    using System;
using System.IO;
using UnityEditor;

    [CreateAssetMenu(fileName = "BuildScheme", menuName = "Build/BuildScheme", order = 1)]
    public class BuildScheme : ScriptableObject
    {
        private const string XCODE_FILE_NAME = "Unity-VisionOS.xcodeproj";

        [SerializeField] private string productName;
        [SerializeField] private string _bundleId;
        [SerializeField] private Vector3 _version = new Vector3(0,1,1);
        // [SerializeField] private int _majorVersion = 0;
        // [SerializeField] private int _minorVersion = 1;
        // [SerializeField] private int _patchVersion = 1;
        [SerializeField] private SceneReference[] scenes;
        [SerializeField] private bool _launchXCode = true;
        [SerializeField] private VisionOSSettings.AppMode _appMode;
        [SerializeField] private Texture2D[] _iconLayers;
        [SerializeField] private bool _skipBuildStep;
        [SerializeField] private AppStoreInformation _information;
       
        private string projectName =>  productName.Replace(" ","").Replace(":","_");
        private string folder => "Builds/" + projectName;
        private string locationPathName => folder + "/" + projectName;

        private string xcodeProjectPath => locationPathName + "/" + XCODE_FILE_NAME;


        public void PerformBuild()
        {
            _version.z++;
            VisionOSSettings.currentSettings.appMode = _appMode;

            PlayerSettings.applicationIdentifier = _bundleId;
            // Application.  = $"{_majorVersion}.{_minorVersion}.{_patchVersion}";
            PlayerSettings.bundleVersion = $"{_version.x}.{_version.y}.{_version.z}";

            BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();

            List<string> sceneList = new List<string>();
            foreach (SceneReference scene in scenes)
            {
                if (scene != null)
                {
#if UNITY_EDITOR
                    Debug.Log(scene.EditorAssetPath);
                    sceneList.Add(scene.EditorAssetPath);
#endif

                }
            }

            PlayerSettings.productName = productName;

            buildPlayerOptions.scenes = sceneList.ToArray();
            Debug.Log("Building Scenes:");
            foreach (string scene in buildPlayerOptions.scenes)
            {
                Debug.Log(scene);
            }

            buildPlayerOptions.locationPathName = locationPathName;
            buildPlayerOptions.target = BuildTarget.VisionOS; // Set your target platform
            SetIcons();

            if (_skipBuildStep == false)
            {
                BuildPipeline.BuildPlayer(buildPlayerOptions);
            }

            // PlayerSettings.SetIconsForTargetGroup(BuildTargetGroup.iOS, _iconLayers);

        }


        [PostProcessBuild]
        public static void PostBuild(BuildTarget target, string pathToBuiltProject)
        {
            if (target == BuildTarget.VisionOS)
            {
                // UpdateXcodeProjectBuildNumber(pathToBuiltProject, 3);
                OpenXcodeStatic(pathToBuiltProject + "/" + XCODE_FILE_NAME);
            }
        }

        private void SetIcons()
        {
            Debug.Log("Set Icons");
            // const string iconDir = "Assets/Icons/visionOS";

            var iconGuidFront = AssetDatabase.AssetPathToGUID( AssetDatabase.GetAssetPath(_iconLayers[2]));
            var iconGuidMiddle =  AssetDatabase.AssetPathToGUID(AssetDatabase.GetAssetPath(_iconLayers[1]));
            var iconGuidBack = AssetDatabase.AssetPathToGUID( AssetDatabase.GetAssetPath(_iconLayers[0]));

            XcodePostProcessUtils.SetupVisionOSIcon(
                buildPath: xcodeProjectPath,
                version: 1,
                author: "Xcode", 
                Tuple.Create("Front", iconGuidFront),
                Tuple.Create("Middle", iconGuidMiddle),
                Tuple.Create("Back", iconGuidBack)
            );
        }
        
        private static void UpdateXcodeProjectBuildNumber(string path, int number)
        {
            Debug.Log($"UpdateXcodeProjectBuildNumber{path} {number}");
            string plistPath = path + "/Info.plist";
            var plist = new PlistDocument();
            plist.ReadFromString(File.ReadAllText(plistPath));

            var rootDict = plist.root;
            rootDict.SetString("CFBundleVersion", "" + number);

            File.WriteAllText(plistPath, plist.WriteToString());
        }

        private static void OpenXcodeStatic(string path)
        {
            OpenFile(path);
        }

        public void OpenXcode()
        {
            OpenFile(xcodeProjectPath);
        }

        private static void OpenFile(string path)
        {
            Process.Start("open", path);
        }

        public void OpenBuilderFolder()
        {
            OpenFile(locationPathName);
        }

        public void DeleteBuild()
        {
            try
            {
                // Delete the folder and all its contents (second parameter is true)
                Directory.Delete(locationPathName, true);
                Debug.Log($"The folder {locationPathName} has been deleted successfully.");
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }
    }
}