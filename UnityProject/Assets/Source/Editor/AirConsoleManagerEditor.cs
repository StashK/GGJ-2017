using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

[CustomEditor(typeof(AirConsoleManager))]
public class AirConsoleManagerEditor : Editor
{
    AirConsoleManager myTarget;

    public override void OnInspectorGUI()
    {
        myTarget = (AirConsoleManager)target;

        foreach (AirConsoleManager.Device d in myTarget.GetDevices().Values)
        {
            DrawDevice(d);
        }


        EditorUtility.SetDirty(myTarget);
    }

    public void DrawDevice (AirConsoleManager.Device device)
    {
        EditorGUILayout.BeginVertical("Box");
        EditorGUILayout.LabelField("" + device.NickName, EditorStyles.boldLabel);
        EditorGUILayout.LabelField("deviceId: ", "" + device.DeviceId);
        EditorGUILayout.LabelField("playerId: ", "" + device.playerId);
        EditorGUILayout.LabelField("view: ", "" + device.view);
        EditorGUILayout.LabelField("color: ", "" + device.color);
        if (device.playerId != -1) DrawPlayer(myTarget.GetPlayer(device.playerId));

        EditorGUILayout.EndVertical();
    }

    public void DrawPlayer (AirConsoleManager.Player player)
    {
        EditorGUILayout.BeginVertical("Box");
        EditorGUILayout.LabelField("Player " + player.PlayerId, EditorStyles.boldLabel);
        EditorGUILayout.LabelField("state: ", "" + player.state);

        EditorGUILayout.EndVertical();
    }
}