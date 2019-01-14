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
    int[][] connectors;

    float size = 5f;

    protected virtual void OnSceneGUI()
    {
        if (Event.current.type == EventType.Repaint)
        {
            Transform transform = ((RoomManager)target).transform;

            Handles.color = Handles.xAxisColor;
            Handles.ArrowHandleCap(
                0,
                transform.position,
                transform.rotation * Quaternion.LookRotation(Vector3.right),
                size,
                EventType.Repaint
            );

            Handles.color = Handles.yAxisColor;
            Handles.ArrowHandleCap(
                0,
                transform.position,
                transform.rotation * Quaternion.LookRotation(Vector3.up),
                size,
                EventType.Repaint
            );
        }
    }
}
