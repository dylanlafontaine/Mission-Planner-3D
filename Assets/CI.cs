using UnityEditor;
class CIScript
{
     static void PerformBuild ()
     {
		 BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();
         buildPlayerOptions.scenes = new[] { "Assets/maptest.unity" };
         buildPlayerOptions.locationPathName = "winBuild";
         buildPlayerOptions.target = BuildTarget.StandaloneWindows64;
         buildPlayerOptions.options = BuildOptions.None;
         BuildPipeline.BuildPlayer(buildPlayerOptions);
     }
}
