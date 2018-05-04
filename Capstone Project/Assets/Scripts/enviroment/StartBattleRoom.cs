using UnityEngine;

public class StartBattleRoom : MonoBehaviour {

    #region Private Fields
    [SerializeField]
    private GameObject[] _doors;
    [SerializeField]
    private GameObject[] _objectsToEnable;
    [SerializeField]
    private GameObject[] _objectsToDisable;
    [SerializeField]
    private GameObject[] _enemies;
    [SerializeField]
    private bool _normalEnemies = false;

    private bool m_Set = false;
    private GameObject[] m_Swarm = null;

    #endregion Private Fields

    #region Private Initializers
    private void OnEnable() {

        // Wire-up events
        SwarmPodSpawner.AllClear += OpenDoors;
    }

    #endregion Private Initializers

    #region Private Finalizers
    private void OnDisable() {

        // Unwire events
        SwarmPodSpawner.AllClear -= OpenDoors;
    }

    #endregion Private Finalizers

    #region Private Methods
    private void Update() {

        bool allowAction = m_Swarm != null;
        allowAction &= m_Swarm?.Length > 0;

        if (allowAction) {

            int counter = 0;
            for (int i = 0; i < m_Swarm.Length; ++i) {
                counter = m_Swarm[i] == null ? ++counter : 0;
            }

            if (counter >= m_Swarm.Length) {

                for (int i = 1; i < _doors.Length; ++i) {
                    _doors[i].SetActive(false);
                }

                if (_objectsToEnable != null) {
                    for (int i = 0; i < _objectsToEnable.Length; ++i) {
                        _objectsToEnable[i].SetActive(true);
                    }
                }
            }
        }

        if (_normalEnemies) {
            int Scounter = 0;
            for (int i = 0; i < _enemies.Length; ++i) {
                if (_enemies[i] == null) {
                    if (++Scounter >= _enemies.Length) { _objectsToDisable[0].SetActive(false); }
                }
                else {
                    Scounter = 0;
                }

            }
        }
    }

    private void OnTriggerStay2D(Collider2D otherGO) {

        bool allowAction = otherGO.tag == StringConstantUtility.PLAYER_TAG;
        allowAction &= m_Set == false;

        if (allowAction) {

            for (int i = 0; i < _doors.Length; ++i) {
                _doors[i].SetActive(true);
            }
            m_Set = true;
        }
    }

    private void OpenDoors(object sender, GameObject[] enemies)
    {
        m_Swarm = enemies;
    }

    #endregion Private Methods
}
