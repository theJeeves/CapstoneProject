using UnityEngine;
using System.Collections;

public class EnableDisable : MonoBehaviour {

    [SerializeField]
    private GameObject _object;

    private void OnEnable() {
        StartCoroutine(CheckIfOffScreen());
    }

    private IEnumerator CheckIfOnScreen() {

        while (Camera.main.WorldToViewportPoint(_object.transform.position).x < 0 ||
               Camera.main.WorldToViewportPoint(_object.transform.position).x > 1 ||
               Camera.main.WorldToViewportPoint(_object.transform.position).y < 0 ||
               Camera.main.WorldToViewportPoint(_object.transform.position).y > 1) {

            yield return 0;
        }

        _object.SetActive(true);
        StartCoroutine(CheckIfOffScreen());
    }

    private IEnumerator CheckIfOffScreen() {
        while (Camera.main.WorldToViewportPoint(_object.transform.position).x > 0 &&
                Camera.main.WorldToViewportPoint(_object.transform.position).x < 1 &&
                Camera.main.WorldToViewportPoint(_object.transform.position).y > 0 &&
                Camera.main.WorldToViewportPoint(_object.transform.position).y < 1) {

            yield return 0;
        }

        _object.SetActive(false);
        StartCoroutine(CheckIfOnScreen());
    }
}
