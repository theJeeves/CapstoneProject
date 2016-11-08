using UnityEngine;
using System.Collections;

public class LaserSights : MonoBehaviour {

    private Transform _player;
    private LineRenderer _renderer;

    private void OnEnable() {

        _player = GameObject.FindGameObjectWithTag("Player").transform;
        _renderer = GetComponent<LineRenderer>();

        StartCoroutine(Laser());
    }

    private IEnumerator Laser() {
        while (true) {
            _renderer.SetPosition(0, transform.position);
            _renderer.SetPosition(1, _player.position);
            _renderer.SetWidth(1.0f, 1.0f);

            yield return 0;
        }
    }
}
