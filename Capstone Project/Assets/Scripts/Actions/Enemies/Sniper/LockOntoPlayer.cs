using UnityEngine;
using System.Collections;

public class LockOntoPlayer : MonoBehaviour {

    public delegate void LockOntoPlayerEvent();
    public static event LockOntoPlayerEvent Fire;

    [SerializeField]
    private float _timer;

    private Transform _player;
    private LineRenderer _renderer;

	private void OnEnable() {

        _player = GameObject.FindGameObjectWithTag("Player").transform;
        _renderer = GetComponent<LineRenderer>();

        StartCoroutine(LockOn());
        StartCoroutine(Laser());
    }

    private IEnumerator LockOn() {

        while (true) {
            int startTime = 0;
            while (startTime < _timer) {
                yield return new WaitForSeconds(1.0f);
                ++startTime;
            }

            if (Fire != null) {
                Fire();
            }
        }
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
