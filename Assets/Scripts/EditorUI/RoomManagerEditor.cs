using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;


/*
// https://www.youtube.com/watch?v=RInUu1_8aGw
[CustomEditor(typeof(RoomManager))]
[CanEditMultipleObjects]
class RoomManagerEditor : Editor
{
    int[][] connectors;

    bool showSides = true;
    bool[] showSide = { true, true, true, true };



    void OnEnable() 
    {
        connectors = ((RoomManager)target).connectors;
    }

    public override void OnInspectorGUI()
    {
        EditorGUILayout.Space();

        showSides = EditorGUILayout.Foldout(showSides, "Sides");
        if (showSides)
        {
            EditorGUI.indentLevel++;

            for (ushort s = 0; s < 4; s++)
            {
                var side = connectors[s];

                showSide[s] = EditorGUILayout.Foldout(showSide[s], "Side" + s);
                if (showSide[s])
                {
                    side[0] = EditorGUILayout.IntField(side[0], new GUIStyle());
                }

            }
        }
    }
}*/
