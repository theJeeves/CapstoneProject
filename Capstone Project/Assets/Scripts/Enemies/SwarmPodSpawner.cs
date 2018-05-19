using UnityEngine;
using UnityEngine.Events;

public class SwarmPodSpawner : MonoBehaviour {

    #region Constant Fields
    private const float X_OFFSET = 2.5f;

    #endregion Constnat Fields

    #region Public Fields
    public int sizeOfSwarm = 0;                   //The number of swarmers the developer wants to spawn from this instance.

    #endregion Public Fields

    #region Private Fields
    [Header("Swarmer Pod Variables")]
    [SerializeField]
    private GameObject[] _swarmPrefab;                //Prefab of the Pod sprite and its scripts
    [SerializeField]
    private float _destructionDelay = 0.0f;         //How long the pod should remain in a damaged state.

    [Space]
    [Header("Additional Game Objects")]

    [SerializeField]
    private GameObject[] _additionalGOs;

    [Space]
    [Header("Effects")]
    [SerializeField]
    private SOEffects _SOEffectHandler;

    private Transform[] m_EffectPositions = null;           // Positions where the effects will play from.
    private bool m_BatteryDamaged = false;           //Bool to determine if the player has hit the battery already

    // Since these two effect animations are "looping", the effects manager will never stop or destroy them automatically.
    //Therefore, we need to have a reference to their instance at manually tell the SOEffectHandler when to destroy them.
    private GameObject m_PodBatteryIndicatorGO;
    private GameObject m_PodBatteryIndicatorGO1;
    private GameObject m_PodBatteryDamageGO;
    private GameObject m_OilSpill1;
    private GameObject m_OilSpill2;
    private GameObject[] m_Swarm;
    private bool m_Move = false;

    #endregion Private Fields

    #region Private Initializers
    private void OnEnable() {
        m_EffectPositions = GetComponentsInChildren<Transform>();

        // Start the battery indication effect animation
        m_PodBatteryIndicatorGO = _SOEffectHandler.PlayEffect(EffectEnums.PodBatteryIndicator, m_EffectPositions[1].position);
        m_PodBatteryIndicatorGO1 = _SOEffectHandler.PlayEffect(EffectEnums.PodBatteryIndicator, m_EffectPositions[6].position);

        m_Swarm = new GameObject[sizeOfSwarm];
    }

    #endregion Private Initializers

    #region Events
    public static event UnityAction<GameObject[]> AllClear;

    #endregion Events

    #region Public Methods
    public void DestroyPod() {

        // This function ensures the player has not hit the battery before. It then sets all necessary variables so the Update
        // function works properly. It also stops the battery indicator effect animation and starts the battery damaged
        //effect animation.
        if (!m_BatteryDamaged) {
            m_BatteryDamaged = true;
            _SOEffectHandler.StopEffect(m_PodBatteryIndicatorGO);
            _SOEffectHandler.StopEffect(m_PodBatteryIndicatorGO1);
            m_PodBatteryDamageGO = _SOEffectHandler.PlayEffect(EffectEnums.PodBatteryDamage, m_EffectPositions[2].position);
            m_OilSpill1 = _SOEffectHandler.PlayEffect(EffectEnums.PodOilSpill1, m_EffectPositions[3].position);
            m_OilSpill2 = _SOEffectHandler.PlayEffect(EffectEnums.PodOilSpill2, m_EffectPositions[4].position);
        }
    }

    public void SpawnSwarm() {

        // Destroy the pod object
        AllClear?.Invoke(m_Swarm);
        Destroy(gameObject);

        // Instantiate as many swarmers as the developer has requested in the inspector.
        for (int i = 0; i < sizeOfSwarm; ++i) {
            m_Swarm[i] = Instantiate(_swarmPrefab[UnityEngine.Random.Range(0, _swarmPrefab.Length)], transform.position, Quaternion.identity) as GameObject;
        }
    }

    #endregion Public Methods

    #region Private Methods
    private void Update() {

        // if the player has hit the battery with the shotgun blast. Once they have, this waits for the delay
        // before starting the explosion effect animation, stoping the battery damage effect animation, and hides
        // sprite representing the pod. Lastly, it calls the fucntion to spawn all the swarmers.
        if (m_BatteryDamaged) {

            if ( TimeTools.TimeExpired(ref _destructionDelay) ) {

                _SOEffectHandler.PlayEffect(EffectEnums.PodExplosion, m_EffectPositions[5].position);

                _SOEffectHandler.StopEffect(m_PodBatteryDamageGO);
                _SOEffectHandler.StopEffect(m_OilSpill1);
                _SOEffectHandler.StopEffect(m_OilSpill2);

                GetComponent<SpriteRenderer>().enabled = false;
                SpawnSwarm();

                if (_additionalGOs.Length > 0) {
                    for (int i = 0; i < _additionalGOs.Length; ++i) {
                        _additionalGOs[i].SetActive(true);
                    }
                }
            }
            else {

                // Have the pod shake violently before it explodes
                if (m_Move) {
                    transform.position = new Vector2(transform.position.x + X_OFFSET, transform.position.y);
                    m_Move = false;
                }
                else if (!m_Move) {
                    transform.position = new Vector2(transform.position.x - X_OFFSET, transform.position.y);
                    m_Move = true;
                }
            }
        }
    }

    #endregion Private Methods
}
