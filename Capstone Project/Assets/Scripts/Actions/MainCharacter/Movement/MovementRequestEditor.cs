using UnityEngine;
using UnityEditor;

[CustomEditor (typeof(MovementRequest))]
public class MovementRequestEditor : Editor {

    private SerializedProperty type;
    private SerializedProperty walkSpeed;
    private SerializedProperty recoil;
    private SerializedProperty setVel;
    private SerializedProperty addVel;
    private SerializedProperty xMultiplier;
    private SerializedProperty xImpulse;
    private SerializedProperty yImpulse;

    private void OnEnable() {

        type = serializedObject.FindProperty("_type");
        walkSpeed = serializedObject.FindProperty("_walkSpeed");
        recoil = serializedObject.FindProperty("_recoil");
        setVel = serializedObject.FindProperty("_setVel");
        addVel = serializedObject.FindProperty("_addVel");
        xMultiplier = serializedObject.FindProperty("_xMultiplier");
        xImpulse = serializedObject.FindProperty("_xImpulse");
        yImpulse = serializedObject.FindProperty("_yImpulse");
    }

    public override void OnInspectorGUI() {
        serializedObject.Update();

        EditorGUILayout.PropertyField(type, new GUIContent("Movement Type") );

        //NONE
        if (type.intValue == 0) {
            EditorGUILayout.LabelField("                        Please select a type of movement", EditorStyles.boldLabel);
        }
        //WALKING
        else if (type.intValue == 1) {
            EditorGUILayout.PropertyField(walkSpeed, new GUIContent("Walk Speed"));
        }
        //SHOTGUN
        else if (type.intValue == 2) {
            EditorGUILayout.PropertyField(recoil, new GUIContent("Recoil"));
            EditorGUILayout.PropertyField(setVel, new GUIContent("Set Velocity"));
            EditorGUILayout.PropertyField(addVel, new GUIContent("Add Velocity"));
        }
        //MACHINE GUN
        else if (type.intValue == 3) {
            EditorGUILayout.PropertyField(recoil, new GUIContent("Recoil"));
            EditorGUILayout.PropertyField(setVel, new GUIContent("Set Velocity"));
            EditorGUILayout.PropertyField(addVel, new GUIContent("Add Velocity"));
            EditorGUILayout.PropertyField(xMultiplier, new GUIContent("X Multiplier"));
        }
        //MACHINE GUN INITIAL LIFT
        else if (type.intValue == 4) {
            EditorGUILayout.PropertyField(xImpulse, new GUIContent("X Impulse Force"));
            EditorGUILayout.PropertyField(yImpulse, new GUIContent("Y Impulse Force"));
        }

        serializedObject.ApplyModifiedProperties();
    }
}
