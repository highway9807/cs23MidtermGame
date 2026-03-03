using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

/// <summary>
/// Adds IceRespawnManager and sample IceSpawnPoints to the current scene so ice respawns
/// at random positions (away from entrance/exit). Run with an ice mine scene open, then
/// move the IceSpawnPoint objects to valid spots.
/// </summary>
public static class IceMineSetup
{
    const string MenuPath = "Tools/Ice Mine/Add Respawn Manager and Spawn Points";
    const string IcePrefabPath = "Assets/Prefabs/Ice.prefab";

    [MenuItem(MenuPath)]
    static void SetupIceMine()
    {
        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().path == null || !UnityEngine.SceneManagement.SceneManager.GetActiveScene().path.Contains("Ice_Mine"))
            Debug.Log("Tip: Open an Ice_Mine scene first, then run this again so spawn points are in the right scene.");

        // 1. IceRespawnManager
        GameObject managerGo = GameObject.Find("IceRespawnManager");
        if (managerGo == null)
        {
            managerGo = new GameObject("IceRespawnManager");
            managerGo.AddComponent<IceRespawnManager>();
        }

        IceRespawnManager manager = managerGo.GetComponent<IceRespawnManager>();
        if (manager != null)
        {
            SerializedObject so = new SerializedObject(manager);
            SerializedProperty prop = so.FindProperty("icePrefab");
            if (prop != null && prop.objectReferenceValue == null)
            {
                GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(IcePrefabPath);
                if (prefab != null)
                {
                    prop.objectReferenceValue = prefab;
                    so.ApplyModifiedPropertiesWithoutUndo();
                    Debug.Log("Assigned Ice prefab to IceRespawnManager.");
                }
                else
                    Debug.LogWarning("Could not find Ice prefab at " + IcePrefabPath + ". Assign it manually in the Inspector.");
            }
        }

        // 2. IceSpawnPoints (only add if none exist)
        IceSpawnPoint[] existing = Object.FindObjectsByType<IceSpawnPoint>(FindObjectsSortMode.None);
        if (existing.Length == 0)
        {
            for (int i = 1; i <= 5; i++)
            {
                GameObject sp = new GameObject("IceSpawnPoint_" + i);
                sp.AddComponent<IceSpawnPoint>();
                sp.transform.position = new Vector3((i - 3) * 2f, 0, 0); // spread horizontally; move in scene
            }
            Debug.Log("Added 5 IceSpawnPoint objects. Move them in the scene away from entrance/exit.");
        }
        else
            Debug.Log("IceSpawnPoints already exist (" + existing.Length + "). Move them as needed.");

        EditorSceneManager.MarkSceneDirty(UnityEngine.SceneManagement.SceneManager.GetActiveScene());
    }
}
