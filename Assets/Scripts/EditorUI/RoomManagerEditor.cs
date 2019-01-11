using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;



// https://www.youtube.com/watch?v=RInUu1_8aGw
[CustomEditor(typeof(RoomManager))]
[CanEditMultipleObjects]
class RoomManagerEditor : Editor
{   
    SerializedProperty connectors;

    void OnEnable() 
    {
        connectors = serializedObject.FindProperty("connectors");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        for (int i = 0; i < 4; i++)
        {
            EditorGUILayout.PropertyField(connectors.GetArrayElementAtIndex(i), new GUIContent("Lamo"));
            serializedObject.ApplyModifiedProperties();
        }
    }
}   