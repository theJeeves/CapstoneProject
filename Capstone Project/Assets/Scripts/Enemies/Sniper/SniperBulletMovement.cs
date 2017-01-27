using UnityEngine;
using System.Collections;

public class SniperBulletMovement : MonoBehaviour {

    [SerializeField]
    private float _speed;

    private void OnEnable() {

        Vector3 unitVect = (GameObject.FindGameObjectWithTag("Player").transform.position - transform.position).normalized;
        GetComponent<Rigidbody2D>().AddForce(unitVect * _speed, ForceMode2D.Impulse);
    }
}
