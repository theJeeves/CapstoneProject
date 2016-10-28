using UnityEngine;
using System.Collections;

public class SniperAttack : MonoBehaviour {

    [SerializeField]
    private GameObject _endOfBarrel;
    [SerializeField]
    private GameObject _bullet;

	private void OnEnable() {
        LockOntoPlayer.Fire += Fire;
    }

    private void OnDisable() {
        LockOntoPlayer.Fire -= Fire;
    }

    private void Fire() {

        Instantiate(_bullet, _endOfBarrel.transform.position, Quaternion.identity);
    }
}
