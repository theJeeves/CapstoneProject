using UnityEngine;
using System.Collections;

public class StartBattleRoom : MonoBehaviour {

    [SerializeField]
    private GameObject[] _doors;

    private void OnTriggerEnter2D(Collider2D otherGO) {

        if (otherGO.tag == "Player") {
            for(int i = 0; i < _doors.Length; ++i) {
                _doors[i].SetActive(true);
            }
        }
    }
}
