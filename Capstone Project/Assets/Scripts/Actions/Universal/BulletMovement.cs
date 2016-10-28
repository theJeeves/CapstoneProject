using UnityEngine;
using System.Collections;

public class BulletMovement : MonoBehaviour {

    [SerializeField]
    private float _speed;

    private GameObject _player;

    private void OnEnable() {

        _player = GameObject.FindGameObjectWithTag("Player");

        StartCoroutine(MoveTowards());
    }

    private IEnumerator MoveTowards() {
        float dist = _player.GetComponent<Rigidbody2D>().velocity.x;

        Vector3 target = _player.transform.position;
        target.x += dist;
        Vector3 diff = (target - transform.position) / (target - transform.position).magnitude;

        while (true) {
            transform.position += diff * (_speed * Time.deltaTime);
            yield return 0;
        }
    }
}
