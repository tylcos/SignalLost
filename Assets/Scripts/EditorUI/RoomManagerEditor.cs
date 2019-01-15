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
    Vector3 offset = new Vector3(10, 0, 100);

    protected virtual void OnSceneGUI()
    {
        Transform roomCenter = ((RoomManager)target).transform;
        

        Handles.RectangleHandleCap(0, offset)
        Handles.Label(offset, offset.ToString());
        offset = Handles.PositionHandle(offset, Quaternion.identity);
        
    }
}
