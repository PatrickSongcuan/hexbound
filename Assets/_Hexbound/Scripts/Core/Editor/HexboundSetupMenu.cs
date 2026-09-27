using System.IO;
using System.Linq;
using Hexbound.Core;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace Hexbound.EditorTools
{
    /// <summary>
    /// One-click creation of the Boot/Hub/Run scenes and their Build Settings order.
    /// Safe to re-run: existing scenes are kept, only the build list is refreshed.
    /// </summary>
    public static class HexboundSetupMenu
    {
        private const string SceneFolder = "Assets/_Hexbound/Scenes";
        private static readonly string[] SceneNames = { "Boot", GameManager.HubScene, GameManager.RunScene };

        [MenuItem("Hexbound/Create Foundation Scenes")]
        public static void CreateFoundationScenes()
        {
            if (!EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
                return;

            Directory.CreateDirectory(SceneFolder);

            foreach (string name in SceneNames)
            {
                string path = $"{SceneFolder}/{name}.unity";
                if (File.Exists(path))
                {
                    Debug.Log($"[Hexbound] {path} already exists, skipped.");
                    continue;
                }

                var scene = EditorSceneManager.NewScene(NewSceneSetup.DefaultGameObjects, NewSceneMode.Single);
                if (name == "Boot")
                    new GameObject("GameManager").AddComponent<GameManager>();

                EditorSceneManager.SaveScene(scene, path);
                Debug.Log($"[Hexbound] Created {path}");
            }

            // Build index order matters: index 0 is what a build launches first.
            EditorBuildSettings.scenes = SceneNames
                .Select(n => new EditorBuildSettingsScene($"{SceneFolder}/{n}.unity", true))
                .ToArray();

            EditorSceneManager.OpenScene($"{SceneFolder}/Boot.unity");
            AssetDatabase.Refresh();
            Debug.Log("[Hexbound] Build Settings: Boot(0), Hub(1), Run(2).");
        }
    }
}
