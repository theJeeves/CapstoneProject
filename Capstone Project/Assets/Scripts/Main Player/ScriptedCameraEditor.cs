using UnityEngine;
using UnityEditor;

//[CustomEditor(typeof(ScriptedCamera))]
public class ScriptedCameraEditor : Editor {

    private SerializedProperty adjustFOV;
    private SerializedProperty toFOV;
    private SerializedProperty FOVAdjustSpeed;

    private SerializedProperty adjustPosition;
    private SerializedProperty targetPosition;
    private SerializedProperty positionAdjustSpeed;

    private void OnEnable() {
        adjustFOV = serializedObject.FindProperty("_adjustFOV");
        toFOV = serializedObject.FindProperty("_toFOV");
        FOVAdjustSpeed = serializedObject.FindProperty("_FOVAdjustSpeed");

        adjustPosition = serializedObject.FindProperty("_adjustPosition");
        targetPosition = serializedObject.FindProperty("_target");
        positionAdjustSpeed = serializedObject.FindProperty("_positionAdjustSpeed");
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

        EditorGUILayout.PropertyField(adjustPosition, new GUIContent("Adjust Position"));
        if (adjustPosition.boolValue == true) {
            EditorGUILayout.PropertyField(targetPosition, new GUIContent("Target Position"));
            EditorGUILayout.PropertyField(positionAdjustSpeed, new GUIContent("Position Adjust Speed"));
            EditorGUILayout.Space(); EditorGUILayout.Space();
        }
        else {
            EditorGUILayout.Space();
        }

        serializedObject.ApplyModifiedProperties();
    }
}
