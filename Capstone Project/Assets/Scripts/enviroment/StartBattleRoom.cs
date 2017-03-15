using UnityEngine;
using System.Collections;

public class StartBattleRoom : MonoBehaviour {

    [SerializeField]
    private GameObject[] _doors;
    private bool _set = false;

    private GameObject[] _swarm;

    private void OnEnable() {
        SwarmPodSpawner.AllClear += OpenDoors;
    }

    private void OnDisable() {
        SwarmPodSpawner.AllClear -= OpenDoors;
    }

    private void Update() {

        if (_swarm != null && _swarm.Length > 0) {

            int counter = 0;
            for (int i = 0; i < _swarm.Length; ++i) {
                counter = _swarm[i] == null ? ++counter : 0;
            }

            if (counter >= _swarm.Length) {

                for (int i = 1; i < _doors.Length; ++i) {
                    _doors[i].SetActive(false);
                }
            }
        }
    }

    private void OnTriggerStay2D(Collider2D otherGO) {

        if (otherGO.tag == "Player" && !_set) {

            for (int i = 0; i < _doors.Length; ++i) {
                _doors[i].SetActive(true);
            }
            _set = true;
        }
    }

    private void OpenDoors(GameObject[] enemies) {

        _swarm = enemies;
    }
}
