using UnityEngine;
using System.Collections;

public class movingPlatform : MonoBehaviour {

    public Transform[] patrol;
    private int current_location;
    public float patrol_speed;


    void Start()
    {
        transform.position =  new Vector3(patrol[0].position.x, patrol[0].position.y, transform.position.z);
        current_location = 0;
    }

    void Update()
    {
            if (transform.position.x == patrol[current_location].position.x && transform.position.y == patrol[current_location].position.y)
            {
                //transform.Rotate(0, 0, 180);
                current_location++;

            }

            if (current_location >= patrol.Length)
            {
                //transform.Rotate(0, 0, -180);
                current_location = 0;
            }
            Vector3 _scroll = new Vector3(patrol[current_location].position.x, patrol[current_location].position.y, transform.position.z);
            transform.position = Vector3.MoveTowards(transform.position, _scroll, patrol_speed * Time.deltaTime);


        }


    }

