using UnityEngine;
using System.Collections;

public class SetCameraPosition : MonoBehaviour {

    [SerializeField]
    private Vector3 _newCameraPos;

    private void OnTriggerEnter2D(Collider2D otherGO) {

        if (otherGO.tag == "Player") {
            GameObject.FindGameObjectWithTag("SmartCamera").transform.position = new Vector3(_newCameraPos.x, _newCameraPos.y, _newCameraPos.z);
        }
    }
}
