using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(NoiseTilemap), editorForChildClasses:true)]
public class NoiseTilemapInspector : Editor
{

    private NoiseTilemap tilemap;

    private void OnEnable()
    {
        tilemap = target as NoiseTilemap;
        Undo.undoRedoPerformed += RefreshInspector;
    }

    private void OnDisable()
    {
        Undo.undoRedoPerformed -= RefreshInspector;
    }

    private void RefreshInspector()
    {
        if (Application.isPlaying)
        {
            tilemap.Refresh();
        }
    }

    public override void OnInspectorGUI()
    {
        EditorGUI.BeginChangeCheck();
        DrawDefaultInspector();
        if (EditorGUI.EndChangeCheck())
        {
            RefreshInspector();
        }

        if (GUILayout.Button("Randomize Offsets"))
        {
            Undo.RecordObject(tilemap, "Randomize Offsets");
            tilemap.RandomizeOffsets();
            EditorUtility.SetDirty(tilemap);
            RefreshInspector();
        }

        if (GUILayout.Button("Randomize Rotation"))
        {
            Undo.RecordObject(tilemap, "Randomize Rotation");
            tilemap.RandomizeRotation();
            EditorUtility.SetDirty(tilemap);
            RefreshInspector();
        }
    }
}
