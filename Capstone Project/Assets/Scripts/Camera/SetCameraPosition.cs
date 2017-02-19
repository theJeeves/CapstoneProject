using UnityEngine;
using System.Collections;

public class SetCameraPosition : MonoBehaviour {

    [SerializeField]
    private float _FOV;
    [SerializeField]
    private Vector2 _newCameraPos;

    private void OnTriggerEnter2D(Collider2D otherGO) {

        if (otherGO.tag == "Player") {
            Camera.main.orthographicSize = _FOV;
            GameObject.FindGameObjectWithTag("SmartCamera").transform.position = new Vector3(_newCameraPos.x, _newCameraPos.y, -10.0f);
        }
    }
}
