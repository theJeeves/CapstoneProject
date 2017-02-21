using UnityEngine;
using System.Collections;

public class EnableDisable : MonoBehaviour {

    [SerializeField]
    private GameObject _object;

    private Vector3 _GOpos;
    private bool _enable = true;

    private void OnEnable() {
        StartCoroutine(CheckIfOffScreen());
    }

    private IEnumerator CheckIfOnScreen() {

        _GOpos = Camera.main.WorldToViewportPoint(transform.position);
        _object.SetActive(false);

        while (_GOpos.x < -0.25f || _GOpos.x > 1.25f || _GOpos.y < -0.25f || _GOpos.y > 1.25f) {

            _GOpos = Camera.main.WorldToViewportPoint(transform.position);
            yield return 0;
        }

        _object.SetActive(true);
        StartCoroutine(CheckIfOffScreen());
    }

    private IEnumerator CheckIfOffScreen() {

        _GOpos = Camera.main.WorldToViewportPoint(transform.position);
        _object.SetActive(true);

        while (_GOpos.x > -0.25f && _GOpos.x < 1.25f && _GOpos.y > -0.25f && _GOpos.y < 1.25f) {

            _GOpos = Camera.main.WorldToViewportPoint(transform.position);
            yield return 0;
        }

        _object.SetActive(false);
        StartCoroutine(CheckIfOnScreen());
    }
}
