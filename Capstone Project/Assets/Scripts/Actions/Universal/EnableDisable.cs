using UnityEngine;
using System.Collections;

public class EnableDisable : MonoBehaviour {

    private void OnEnable() {
        StartCoroutine(CheckOffScreen());
    }

    private void OnDisable() {
        if (gameObject.activeInHierarchy) {
            StartCoroutine(CheckOnScreen());
        }
    }

    private IEnumerator CheckOnScreen() {

        while (Camera.main.WorldToViewportPoint(transform.position).x < 0 ||
            Camera.main.WorldToViewportPoint(transform.position).x > 1 ||
            Camera.main.WorldToViewportPoint(transform.position).y < 0 ||
            Camera.main.WorldToViewportPoint(transform.position).y > 1) {

            Debug.Log("not on screen");

            yield return 0;
        }

        OnEnable();
        yield return 0;
    }

    private IEnumerator CheckOffScreen() {
        while (Camera.main.WorldToViewportPoint(transform.position).x > 0 &&
            Camera.main.WorldToViewportPoint(transform.position).x < 1 &&
            Camera.main.WorldToViewportPoint(transform.position).y > 0 &&
            Camera.main.WorldToViewportPoint(transform.position).y < 1) {

            Debug.Log("on screen");

            yield return 0;
        }

        OnDisable();
        yield return 0;
    }
}
