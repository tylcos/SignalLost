using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;



[CustomEditor(typeof(RoomManager))]
[CanEditMultipleObjects]
class RoomManagerEditor : Editor
{
    public List<int>[] connectors = new List<int>[4];
    public BoundsInt bounds;



    private RoomManager castedTarget;
    private readonly GUILayoutOption width = GUILayout.Width(25);



    void OnEnable()
    {
        castedTarget = (target as RoomManager);

        bounds = serializedObject.FindProperty("bounds").boundsIntValue;

        // Necessary because unity cannot serialize an array of lists
        connectors[0] = castedTarget.side1;
        connectors[1] = castedTarget.side2;
        connectors[2] = castedTarget.side3;
        connectors[3] = castedTarget.side4;
    }



    public override void OnInspectorGUI()
    {
        GUI.skin.button.alignment = TextAnchor.MiddleCenter;
        GUI.skin.button.margin = new RectOffset(5, 5, 0, 0);

        for (int r = 0; r < 4; r++)
        {
            var currentRow = connectors[r];

            EditorGUILayout.BeginHorizontal();
            
            if (GUILayout.Button("+", width))
            {
                Undo.RecordObject(castedTarget, "Change pathways");
                currentRow.Add(0);
                UpdateValues();
            }
                

            if (GUILayout.Button("-", width) && currentRow.Count > 0)
            {
                Undo.RecordObject(castedTarget, "Change pathways");
                currentRow.RemoveAt(currentRow.Count - 1);
                UpdateValues();
            }


            for (int c = 0; c < currentRow.Count; c++)
            {
                int size = r < 2 ? bounds.size.x : bounds.size.y;
                int changedValue = EditorGUILayout.IntSlider(currentRow[c], 0, size - 2);

                if (changedValue != currentRow[c])
                {
                    Undo.RecordObject(castedTarget, "Change pathways");
                    currentRow[c] = changedValue;
                    UpdateValues();
                }
            }


            EditorGUILayout.EndHorizontal();
        }
    }

    private void UpdateValues()
    {
        castedTarget.side1 = connectors[0];
        castedTarget.side2 = connectors[1];
        castedTarget.side3 = connectors[2];
        castedTarget.side4 = connectors[3];

        castedTarget.UpdateConnectors();
    }
}
