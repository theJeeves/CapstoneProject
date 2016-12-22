using UnityEngine;
using System.Collections;

public class cameratest : MonoBehaviour {

    [SerializeField]
    private BasicScriptedCamera _scriptedCam;

    private void OnTriggerEnter2D(Collider2D player) {
        if (player.tag == "Player") {
            StartCoroutine(AdjustCamera(player.gameObject.transform.position));
            Debug.Log("entered trigger");
        }
    }

    private IEnumerator AdjustCamera(Vector2 playerPos) {
        while (!_scriptedCam.MoveCamera(playerPos)) {
            yield return 0;
        }
        Debug.Log("done adjusting");
    }
}
