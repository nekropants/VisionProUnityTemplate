
#if UNITY_EDITOR
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using UnityEditor;
using UnityEditor.Build.Content;
using UnityEditor.Callbacks;
using UnityEditor.XR.VisionOS;
using UnityEngine;
using UnityEngine.Serialization;
using Debug = UnityEngine.Debug;

[CreateAssetMenu(fileName = "BuildScheme", menuName = "Build/BuildScheme", order = 1)]
   public class BuildScheme : ScriptableObject
   {
       private const string XCODE_FILE_NAME ="Unity-VisionOS.xcodeproj";

       [SerializeField] private string productName;
       [SerializeField] private SceneReference[] scenes;
       [SerializeField] private bool _launchXCode = true;
       [SerializeField] private  VisionOSSettings.AppMode _appMode;
       [SerializeField] private Texture2D[] _iconLayers;
       [SerializeField] private bool _skipBuildStep;

       
       private  string folder => "Builds/" + productName;
       private string locationPathName => folder + "/" + productName;
       
       private string xcodeProjectPath => locationPathName + "/" +XCODE_FILE_NAME;
       

           public void PerformBuild()
           {

               VisionOSSettings.currentSettings.appMode = _appMode;

               PlayerSettings.applicationIdentifier = "com.seafoamx."+productName.ToLower();

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

               PlayerSettings.productName = productName;

               buildPlayerOptions.scenes = sceneList.ToArray();
               
               Debug.Log("Building Scenes:");
               foreach (string scene in buildPlayerOptions.scenes)
               {
                   Debug.Log(scene);
               }
               buildPlayerOptions.locationPathName = locationPathName;
               buildPlayerOptions.target = BuildTarget.VisionOS; // Set your target platform

               if (_skipBuildStep == false)
               {
                   BuildPipeline.BuildPlayer(buildPlayerOptions);
               }
               
               PlayerSettings.SetIconsForTargetGroup(BuildTargetGroup.iOS, _iconLayers);
               // PlayerSettings.SetIcons(BuildTargetGroup.iOS, _iconLayers, );

               if (_launchXCode)
               {
                   // OpenXCode( buildPlayerOptions.locationPathName );
               }
           }


           [PostProcessBuild]
           public static void PostBuild(BuildTarget target, string pathToBuiltProject)
           {
               Debug.Log("OpenXcodeProject " );

               if (target == BuildTarget.VisionOS  )
               {
                   OpenXCode(pathToBuiltProject+"/"+XCODE_FILE_NAME);
               }
           }

           public void OpenXCodeProject()
           {
               OpenFile(xcodeProjectPath);
           }
           private static void OpenXCode(string folderPath)
           {
               // Path to the Xcode project
               // string path = folderPath + "/Unity-VisionOS.xcodeproj";

               // Open the Xcode project
               
               
               // Assuming the build_and_deploy.sh is in the root of your Unity project
               string scriptPath = Path.Combine(Directory.GetParent(Application.dataPath).FullName, "BuildAndDeploy.sh");

               // Process.Start("/bin/bash", $"{scriptPath} {pathToBuiltProject}");
           }
           
           private static void OpenFile(string path)
           {
               
               Debug.Log("OpenFile " + path);

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
#endif
