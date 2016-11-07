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

        Vector3 unitVect = (_player.transform.position - transform.position).normalized;

        //Recall: Magnitude = Length of a vector
        //Recall: Normalization = convert to unit vector (x / length, y/length)

        while (true) {

            transform.position += unitVect * (_speed * Time.deltaTime);
            yield return 0;
        }
    }
}
