using UnityEngine;
using System.Collections;

public class LaserSights : MonoBehaviour {

    private Transform _player;
    [SerializeField]
    private Transform _endOfBarrel;
    private LineRenderer _renderer;

    private void Awake() {

        _player = GameObject.FindGameObjectWithTag("Player").transform;
        _renderer = GetComponent<LineRenderer>();
    }

    private void OnEnable() {
        SniperPushBack.Stun += TurnOffSights;

        StartCoroutine(Laser());
    }

    private void OnDisable() {
        SniperPushBack.Stun -= TurnOffSights;
    }

    private void TurnOffSights() {
        _renderer.enabled = false;
        StartCoroutine(TurnOnSights());
    }

    private IEnumerator TurnOnSights() {
        yield return new WaitForSeconds(1.0f);
        _renderer.enabled = true;
    }

    private IEnumerator Laser() {
        while (true) {
            //Set the Start Position
            _renderer.SetPosition(0, _endOfBarrel.position);

            //Set the End Position
            _renderer.SetPosition(1, _player.position);

            //Set the width of the line renderer
            _renderer.SetWidth(1.0f, 1.0f);

            yield return 0;
        }
    }
}
