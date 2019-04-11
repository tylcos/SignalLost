using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;



[CustomEditor(typeof(RoomManager))]
[CanEditMultipleObjects]
[ExecuteInEditMode]
class RoomManagerEditor : Editor
{
    public List<int>[] connectors;
    public Bounds bounds;



    private RoomManager castedTarget;
    private readonly GUILayoutOption width = GUILayout.Width(25);



    void OnEnable()
    {
        castedTarget = (target as RoomManager);

        bounds = serializedObject.FindProperty("bounds").boundsValue;

        // Necessary because unity cannot serialize an array of lists
        string[] sides = castedTarget.connectorsString.Split('|');

        connectors = new List<int>[4];
        for (int i = 0; i < 4; i++)
            connectors[i] = sides[i].Length == 0 ? new List<int>() : sides[i].Split(',').Select(int.Parse).ToList();
    }



    public override void OnInspectorGUI()
    {
        if (Application.isPlaying)
            return;



        GUI.skin.button.alignment = TextAnchor.MiddleCenter;
        GUI.skin.button.margin = new RectOffset(5, 5, 0, 0);
        


        EditorGUILayout.LabelField("Used to place pathways on each side of the current room.");

        for (int r = 0; r < 4; r++)
        {
            List<int> currentRow = connectors[r];

            EditorGUILayout.BeginHorizontal();
            
            if (GUILayout.Button("+", width))
            {
                Undo.RecordObject(castedTarget, "Change pathways");
                currentRow.Add(0);  
                SaveValues();
                PrefabUtility.RecordPrefabInstancePropertyModifications(castedTarget);
            }
                

            if (GUILayout.Button("-", width) && currentRow.Count > 0)
            {
                Undo.RecordObject(castedTarget, "Change pathways");
                currentRow.RemoveAt(currentRow.Count - 1);
                SaveValues();
                PrefabUtility.RecordPrefabInstancePropertyModifications(castedTarget);
            }


            for (int c = 0; c < currentRow.Count; c++)
            {
                int size = (int)(r < 2 ? bounds.extents.x : bounds.extents.y);
                int changedValue = EditorGUILayout.IntSlider(currentRow[c], -size + 1, size - 1);

                if (changedValue != currentRow[c])
                {
                    Undo.RecordObject(castedTarget, "Change pathways");
                    currentRow[c] = changedValue;
                    SaveValues();
                    PrefabUtility.RecordPrefabInstancePropertyModifications(castedTarget);
                }
            }


            EditorGUILayout.EndHorizontal();
        }

        EditorGUILayout.Separator();
        if (GUILayout.Button("Save room changes (Hit ctrl-s also)", GUILayout.Width(250)))
            castedTarget.UpdateBoundSize();

        EditorGUILayout.Separator();
        EditorGUILayout.LabelField("The room connection string = " + castedTarget);
    }

    private void SaveValues()
    {
        string[] connectorsStrings = new string[4];
        for (int i = 0; i < 4; i++)
            connectorsStrings[i] = string.Join(",", connectors[i]);

        castedTarget.connectorsString = string.Join("|", connectorsStrings);

        castedTarget.UpdateConnectors();
        
        SceneView.RepaintAll();
    }
}
