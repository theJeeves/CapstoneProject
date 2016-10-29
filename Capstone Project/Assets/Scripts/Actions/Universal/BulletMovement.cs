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

    /*
     * (P bi) Bullet Inital position = transform.postion
     * (S b) Bullet speed = _speed
     * (P ti) Target Initial postiion = _player.transform.position;
     * (V ti) Target Initial velociy = _player.GetComponent<RigidBody2D>().velocity;
     * (A t) Target Acceleration = n/a;
     * 
     * D = (Target Initial Postion - Bullet Initial position ).magnitude (length)
     * S t = target.velocity.magnitude (length)
     * Vt = Target velocity
     */

    private IEnumerator MoveTowards() {
        //float dist = _player.GetComponent<Rigidbody2D>().velocity.x;

        Vector3 unitVect = (_player.transform.position - transform.position).normalized;

        //Recall: Magnitude = Length of a vector
        //Recall: Normalization = convert to unit vector (x / length, y/length)

        //Vector3 targetPos = _player.transform.position;
        //double targetSpeed = _player.GetComponent<Rigidbody2D>().velocity.magnitude;
        //Vector3 bulletPos = transform.position;

        //double D = (targetPos - bulletPos).magnitude;
        //double St = _player.GetComponent<Rigidbody2D>().velocity.magnitude;
        //Vector3 Vt = _player.GetComponent<Rigidbody2D>().velocity;
        //double cosTheta = Vector3.Dot((bulletPos - targetPos).normalized, Vt.normalized);

        //double time1 = (-2.0 * D * St * cosTheta + Mathf.Sqrt( Mathf.Pow(2.0f * (float)D* (float)St * (float)cosTheta, 2.0f) )
        //    + 4 * (Mathf.Pow(_speed, 2.0f) - Mathf.Pow((float)St, 2.0f)) * Mathf.Pow((float)D, 2.0f) ) / 
        //    (2.0 * (Mathf.Pow(_speed, 2.0f) - Mathf.Pow((float)St, 2.0f) ) );

        //double time2 = (-2.0 * D * St * cosTheta - Mathf.Sqrt(Mathf.Pow(2.0f * (float)D * (float)St * (float)cosTheta, 2.0f))
        //    + 4 * (Mathf.Pow(_speed, 2.0f) - Mathf.Pow((float)St, 2.0f)) * Mathf.Pow((float)D, 2.0f)) /
        //    (2.0 * (Mathf.Pow(_speed, 2.0f) - Mathf.Pow((float)St, 2.0f)));


        //double time = time1 < time2 ? time1 : time2;

        //Vector3 Vb = Vt + (_player.transform.position - transform.position) / (float)time;

        while (true) {
            //transform.position += Vb.normalized * (_speed * Time.deltaTime);
            transform.position += unitVect* (_speed * Time.deltaTime);

            yield return 0;
        }
    }
}
