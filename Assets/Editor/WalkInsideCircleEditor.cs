using UnityEngine;
using UnityEditor;

[CustomEditor (typeof(WalkInsideCircle))]
public class WalkInsideEditor : Editor {
    public override void OnInspectorGUI() {
        WalkInsideCircle script = (WalkInsideCircle) target;

        script.circleRadius = EditorGUILayout.FloatField("Circle Radius", script.circleRadius);
        script.origin = EditorGUILayout.Vector3Field("Origin", script.origin);


        script.randomlyPause = EditorGUILayout.Toggle("Randomly Pause", script.randomlyPause);
        if (script.randomlyPause) {
            script.minPause = EditorGUILayout.FloatField("Min Pause", script.minPause);
            script.maxPause = EditorGUILayout.FloatField("Max Pause", script.maxPause);
        }

        script.randomizeSpeed = EditorGUILayout.Toggle("Randomize Speed", script.randomizeSpeed);
        if (script.randomizeSpeed) {
            script.minSpeed = EditorGUILayout.FloatField("Min Speed", script.minSpeed);
            script.maxSpeed = EditorGUILayout.FloatField("Max Speed", script.maxSpeed);
        }
    }
}


