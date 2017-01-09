using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ScriptedCamera))]
public class ScriptedCameraEditor : Editor {

    private SerializedProperty adjustFOV;
    private SerializedProperty toFOV;
    private SerializedProperty FOVAdjustSpeed;

    private SerializedProperty adjustXPosition;
    private SerializedProperty toXPosition;
    private SerializedProperty XAdjustSpeed;

    private SerializedProperty adjustYPosition;
    private SerializedProperty toYPosition;
    private SerializedProperty YAdjustSpeed;

    private void OnEnable() {
        adjustFOV = serializedObject.FindProperty("_adjustFOV");
        toFOV = serializedObject.FindProperty("_toFOV");
        FOVAdjustSpeed = serializedObject.FindProperty("_FOVAdjustSpeed");

        adjustXPosition = serializedObject.FindProperty("_adjustXPosition");
        toXPosition = serializedObject.FindProperty("_toXPos");
        XAdjustSpeed = serializedObject.FindProperty("_XAdjustSpeed");

        adjustYPosition = serializedObject.FindProperty("_adjustYPosition");
        toYPosition = serializedObject.FindProperty("_toYPos");
        YAdjustSpeed = serializedObject.FindProperty("_YAdjustSpeed");
    }

    public override void OnInspectorGUI() {

        serializedObject.Update();

        EditorGUILayout.PropertyField(adjustFOV, new GUIContent("Adjust FOV"));
        if (adjustFOV.boolValue == true) {
            EditorGUILayout.PropertyField(toFOV, new GUIContent("New FOV Size"));
            EditorGUILayout.PropertyField(FOVAdjustSpeed, new GUIContent("FOV Adjust Speed"));
            EditorGUILayout.Space(); EditorGUILayout.Space();
        }
        else {
            EditorGUILayout.Space();
        }

        EditorGUILayout.PropertyField(adjustXPosition, new GUIContent("Adjust X Position"));
        if (adjustXPosition.boolValue == true) {
            EditorGUILayout.PropertyField(toXPosition, new GUIContent("New X Position"));
            EditorGUILayout.PropertyField(XAdjustSpeed, new GUIContent("X Adjust Speed"));
            EditorGUILayout.Space(); EditorGUILayout.Space();
        }
        else {
            EditorGUILayout.Space();
        }

        EditorGUILayout.PropertyField(adjustYPosition, new GUIContent("Adjust Y Position"));
        if (adjustYPosition.boolValue == true) {
            EditorGUILayout.PropertyField(toYPosition, new GUIContent("New Y Position"));
            EditorGUILayout.PropertyField(YAdjustSpeed, new GUIContent("Y Adjust Speed"));
            EditorGUILayout.Space();
        }
        else {
            EditorGUILayout.Space();
        }

        serializedObject.ApplyModifiedProperties();
    }
}
