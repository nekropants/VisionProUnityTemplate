
#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.Build.Content;
using UnityEditor.XR.VisionOS;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "BuildScheme", menuName = "Build/BuildScheme", order = 1)]
   public class BuildScheme : ScriptableObject
   {
        public string productName;
       // public int versionCode;
       // public string versionName;
       public SceneReference[] scenes;
       public  VisionOSSettings.AppMode _appMode;

           public void PerformBuild()
           {

               VisionOSSettings.currentSettings.appMode = _appMode;

               BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();

               List<string> sceneList = new List<string>();
               foreach (SceneReference scene in scenes)
               {
                   if (scene != null)
                   {
                       Debug.Log(scene.EditorAssetPath);

                       sceneList.Add(scene.EditorAssetPath);   
                   }
               }

               string folder = "Builds/" + productName +"/";
               PlayerSettings.productName = productName;

               
               // buildPlayerOptions.scenes = sceneList.ToArray();
               // string fullpath = Application.dataPath.Replace("Assets", "") + folder;
               
               // Debug.Log(Application.dataPath.Replace("Assets", "") + folder );
               // Directory.CreateDirectory(fullpath);

               string[] array = sceneList.ToArray();
               // buildPlayerOptions.scenes = new[] { "Assets/Dogs/DoggyVisualization.unity" };
               buildPlayerOptions.scenes = sceneList.ToArray();
               
               foreach (string VARIABLE in buildPlayerOptions.scenes)
               {
                   Debug.Log(VARIABLE);
               }
               buildPlayerOptions.locationPathName = folder + "/" + productName;
               buildPlayerOptions.target = BuildTarget.VisionOS; // Set your target platform
               
               BuildPipeline.BuildPlayer(buildPlayerOptions);
           }
           
   }
#endif
