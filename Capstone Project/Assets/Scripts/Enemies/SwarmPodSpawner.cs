using UnityEngine;
using System.Collections;

public class SwarmPodSpawner : MonoBehaviour {

    [SerializeField]
    private GameObject _swarmPrefab;
    [SerializeField]
    private int _sizeOfSwarm = 0;

    private void OnEnable() {
        StartCoroutine(Delay());
    }

    private IEnumerator Delay() {
        yield return new WaitForSeconds(3.0f);
        SpawnSwarm();
    }

    public void SpawnSwarm() {

        for (int i = 0; i < _sizeOfSwarm; ++i) {
            Instantiate(_swarmPrefab, transform.position, Quaternion.identity);
        }
    }
}
