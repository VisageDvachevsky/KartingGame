using UnityEditor;
using UnityEditor.SceneManagement;

namespace Project.Editor
{
    [InitializeOnLoad]
    public static class MainSceneAutoLoader
    {
        static MainSceneAutoLoader()
        {
            if (EditorBuildSettings.scenes.Length == 0) return;
            EditorSceneManager.playModeStartScene = AssetDatabase.LoadAssetAtPath<SceneAsset>(EditorBuildSettings.scenes[0].path);
        }
    }
}
