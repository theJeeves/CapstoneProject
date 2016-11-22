using UnityEngine;
using System.Collections;

public class ChargerRocketAnim : MonoBehaviour {

    private SpriteRenderer _rockets;

    private void Awake() {
        _rockets = GetComponent<SpriteRenderer>();
    }

	private void OnEnable() {
        ChargerLockOn.RocketAnim += StartAnim;
        ChargerLockOn.ResetRockets += ResetRockets;
    }

    private void OnDisable() {
        ChargerLockOn.RocketAnim -= StartAnim;
        ChargerLockOn.ResetRockets -= ResetRockets;
    }

    private void StartAnim(float time) {
        StartCoroutine(RocketAnim(time));
    }

    private void ResetRockets() {
        _rockets.enabled = false;
    }

    private IEnumerator RocketAnim(float time) {

        _rockets.enabled = true;
        yield return new WaitForSeconds(time / 4);
        _rockets.enabled = false;
        yield return new WaitForSeconds(time / 4);
        _rockets.enabled = true;
        yield return new WaitForSeconds(time / 4);
        _rockets.enabled = false;
        yield return new WaitForSeconds(time / 4);
        _rockets.enabled = true;
    }
}
