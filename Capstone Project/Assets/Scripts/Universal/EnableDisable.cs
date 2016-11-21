using UnityEngine;
using System.Collections;

public class EnableDisable : MonoBehaviour {

    [SerializeField]
    private GameObject _object;

    Vector3 _GOpos;

    private void OnEnable() {
        StartCoroutine(CheckIfOffScreen());
    }

    private IEnumerator CheckIfOnScreen() {

        _GOpos = Camera.main.WorldToViewportPoint(_object.transform.position);

        while (_GOpos.x < -0.15f || _GOpos.x > 1.15f || _GOpos.y < -0.15f || _GOpos.y > 1.0f) {
            yield return 0;
        }

        _object.SetActive(true);
        StartCoroutine(CheckIfOffScreen());
    }

    private IEnumerator CheckIfOffScreen() {

        _GOpos = Camera.main.WorldToViewportPoint(_object.transform.position);

        while (_GOpos.x > -0.15f && _GOpos.x < 1.15f && _GOpos.y > -0.15f && _GOpos.y < 1.0f) {
            yield return 0;
        }

        _object.SetActive(false);
        StartCoroutine(CheckIfOnScreen());
    }
}
