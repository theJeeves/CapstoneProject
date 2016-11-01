using UnityEngine;
using System.Collections;

public class LockOntoPlayer : MonoBehaviour {

    public delegate void LockOntoPlayerEvent();
    public static event LockOntoPlayerEvent Fire;

    [SerializeField]
    private float _timer;

	private void OnEnable() {

        StartCoroutine(LockOn());

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
}
