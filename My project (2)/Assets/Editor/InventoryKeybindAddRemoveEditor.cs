using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

[CustomEditor(typeof(InventoryKeybindAddRemove))]
public class InventoryKeybindAddRemoveEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        if (GUILayout.Button("Refresh item list from project (all ItemDefinitions)"))
            RefreshItemList((InventoryKeybindAddRemove)target);
    }

    static void RefreshItemList(InventoryKeybindAddRemove script)
    {
        string[] guids = AssetDatabase.FindAssets("t:ItemDefinition");
        if (guids == null || guids.Length == 0)
        {
            Debug.Log("No ItemDefinition assets found in the project.");
            return;
        }
        List<ItemDefinition> list = new List<ItemDefinition>();
        for (int i = 0; i < guids.Length; i++)
        {
            string path = AssetDatabase.GUIDToAssetPath(guids[i]);
            ItemDefinition item = AssetDatabase.LoadAssetAtPath<ItemDefinition>(path);
            if (item != null) list.Add(item);
        }
        SerializedObject so = new SerializedObject(script);
        SerializedProperty prop = so.FindProperty("items");
        if (prop == null) { Debug.LogError("'items' property not found."); return; }
        prop.ClearArray();
        for (int i = 0; i < list.Count; i++)
        {
            prop.InsertArrayElementAtIndex(i);
            prop.GetArrayElementAtIndex(i).objectReferenceValue = list[i];
        }
        so.ApplyModifiedPropertiesWithoutUndo();
        Debug.Log("Item list refreshed with " + list.Count + " ItemDefinitions.");
    }
}
