using UnityEngine;
using System.Collections;

public class SniperAttack : MonoBehaviour {

    [SerializeField]
    private GameObject _endOfBarrel;
    [SerializeField]
    private GameObject _bullet;

	private void OnEnable() {
        SniperLockOn.Attack += Fire;
    }

    private void OnDisable() {
        SniperLockOn.Attack -= Fire;
    }

    private void Fire() {

        Instantiate(_bullet, _endOfBarrel.transform.position, Quaternion.identity);
    }
}
