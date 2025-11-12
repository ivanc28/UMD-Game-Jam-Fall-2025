using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MovingPlatformActor))]
public class MovingPlatformHandle : Editor
{
    //Vector3 handlePos;
    void OnSceneGUI()
    {
        Handles.color = Color.white;
        MovingPlatformActor actor = (MovingPlatformActor)target;
        Handles.DrawLine(actor.transform.position, actor.finalPosition);
        actor.finalPosition = Handles.PositionHandle(actor.finalPosition, Quaternion.identity);
    }
}
