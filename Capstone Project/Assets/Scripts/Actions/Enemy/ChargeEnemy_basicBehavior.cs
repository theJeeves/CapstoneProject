using UnityEngine;
using System.Collections;

public class ChargeEnemy_basicBehavior : MonoBehaviour
{
    private bool isCharging = false;
    public GameObject player;
    private int visionDistance = 7;
    private float chaserSpeed = 15.5f;
    Vector2 chargePos;
    private int overDistance = 50;
    private Rigidbody2D rb;

    void Chase()
    {
        if (!isCharging)
        {
            if (Mathf.Abs(player.transform.position.x) - Mathf.Abs(transform.position.x) < visionDistance)
            {
                if (player.transform.position.x < transform.position.x)
                {
                    overDistance = overDistance * -1;
                }
                chargePos = new Vector2(player.transform.position.x + overDistance, transform.position.y);
                transform.position = Vector2.MoveTowards(transform.position, chargePos, chaserSpeed * Time.deltaTime);
                isCharging = true;
                if (player.transform.position.x + overDistance == transform.position.x)
                {
                    isCharging = false;
                }
            }
        }
    }
    void Start()
    {
        Chase();
    }
}
