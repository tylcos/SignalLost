using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;



[CustomEditor(typeof(RoomManager))]
[CanEditMultipleObjects]
[ExecuteInEditMode]
class RoomManagerEditor : Editor
{
    public List<int>[] connectors = new List<int>[4];
    public Bounds bounds;



    private RoomManager castedTarget;
    private readonly GUILayoutOption width = GUILayout.Width(25);



    void OnEnable()
    {
        castedTarget = (target as RoomManager);

        bounds = serializedObject.FindProperty("bounds").boundsValue;

        // Necessary because unity cannot serialize an array of lists
        connectors[0] = castedTarget.side1;
        connectors[1] = castedTarget.side2;
        connectors[2] = castedTarget.side3;
        connectors[3] = castedTarget.side4;
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
            var currentRow = connectors[r];

            EditorGUILayout.BeginHorizontal();
            
            if (GUILayout.Button("+", width))
            {
                Undo.RecordObject(castedTarget, "Change pathways");
                currentRow.Add(0);  
                UpdateValues();
                PrefabUtility.RecordPrefabInstancePropertyModifications(castedTarget);
            }
                

            if (GUILayout.Button("-", width) && currentRow.Count > 0)
            {
                Undo.RecordObject(castedTarget, "Change pathways");
                currentRow.RemoveAt(currentRow.Count - 1);
                UpdateValues();
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
                    UpdateValues();
                    PrefabUtility.RecordPrefabInstancePropertyModifications(castedTarget);
                }
            }


            EditorGUILayout.EndHorizontal();
        }

        EditorGUILayout.Separator();
        if (GUILayout.Button("Update room bounds", GUILayout.Width(200)))
            castedTarget.UpdateBoundSize();
    }

    private void UpdateValues()
    {
        castedTarget.UpdateConnectors();
        SceneView.RepaintAll();
    }
}
