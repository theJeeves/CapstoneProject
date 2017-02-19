using UnityEngine;
using System.Collections;

public class TransitionCamera : MonoBehaviour {

    [SerializeField]
    private ScriptableObject _SOCameraHandler;
    [SerializeField]
    private float _FOV;
    [SerializeField]
    private Vector2 _newCameraPosition;

    private void OnTriggerEnter2D(Collider2D otherGO) {

        if (otherGO.tag == "Player") {

        }
    }
}
