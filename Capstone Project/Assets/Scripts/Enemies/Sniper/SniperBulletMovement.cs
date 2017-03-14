using UnityEngine;
using System.Collections;

public class SniperBulletMovement : MonoBehaviour {

    [SerializeField]
    private float _speed;

    private void OnEnable() {

        // Get the center of the player based on the collider
        Vector3 playerPos = GameObject.FindGameObjectWithTag("Player").GetComponent<BoxCollider2D>().bounds.center;
        // Offset where the bullet will go by 7.5 in the y-axis
        playerPos = new Vector3(playerPos.x, playerPos.y + 7.5f, playerPos.z);


        Vector3 unitVect = (playerPos - transform.position).normalized;
        GetComponent<Rigidbody2D>().AddForce(unitVect * _speed, ForceMode2D.Impulse);
    }
}
