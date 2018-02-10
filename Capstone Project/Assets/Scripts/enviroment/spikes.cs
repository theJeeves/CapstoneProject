using UnityEngine;
using System.Collections;

public class spikes : MonoBehaviour {

    [SerializeField]
    protected int _damage;

    private void OnCollisionEnter2D(Collision2D otherGO)
    {

        string tag = otherGO.gameObject.tag;

        if (tag == "Player") {
            otherGO.gameObject.GetComponent<PlayerHealth>().KillPlayer();
        }

    }


}
