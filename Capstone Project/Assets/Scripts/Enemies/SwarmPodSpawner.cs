using UnityEngine;
using System.Collections;

public class SwarmPodSpawner : MonoBehaviour {

    public delegate void SwarmPodEvent(GameObject[] enemies);
    public static event SwarmPodEvent AllClear;

    [Header("Swarmer Pod Variables")]
    [SerializeField]
    private GameObject[] _swarmPrefab;                //Prefab of the Pod sprite and its scripts

    public int sizeOfSwarm = 0;                   //The number of swarmers the developer wants to spawn from this instance.
    [SerializeField]
    private float _destructionDelay = 0.0f;         //How long the pod should remain in a damaged state.

    private GameObject[] _swarm;

    [Space]
    [Header("Effects")]
    [SerializeField]
    private SOEffects _SOEffectHandler;

    private Transform[] _effectPositions;           // Positions where the effects will play from.
    private bool _batteryDamaged = false;           //Bool to determine if the player has hit the battery already
    private float _timer = 0.0f;                    //For delay purposes in Update

    // Since these two effect animations are "looping", the effects manager will never stop or destroy them automatically.
    //Therefore, we need to have a reference to their instance at manually tell the SOEffectHandler when to destroy them.
    private GameObject _podBatteryIndicatorGO;
    private GameObject _podBatteryIndicatorGO1;
    private GameObject _podBatteryDamageGO;
    private GameObject _oilSpill1;
    private GameObject _oilSpill2;

    private bool _move = false;

    private void OnEnable() {
        _effectPositions = GetComponentsInChildren<Transform>();

        // Start the battery indication effect animation
        _podBatteryIndicatorGO = _SOEffectHandler.PlayEffect(EffectEnums.PodBatteryIndicator, _effectPositions[1].position);
        _podBatteryIndicatorGO1 = _SOEffectHandler.PlayEffect(EffectEnums.PodBatteryIndicator, _effectPositions[6].position);

        _swarm = new GameObject[sizeOfSwarm];
        _timer = Time.time;
    }

    private void Update() {

        // if the player has hit the battery with the shotgun blast. Once they have, this waits for the delay
        // before starting the explosion effect animation, stoping the battery damage effect animation, and hides
        // sprite representing the pod. Lastly, it calls the fucntion to spawn all the swarmers.
        if (_batteryDamaged) {

            if (Time.time - _timer > _destructionDelay) {

                _SOEffectHandler.PlayEffect(EffectEnums.PodExplosion, _effectPositions[5].position);

                _SOEffectHandler.StopEffect(_podBatteryDamageGO);
                _SOEffectHandler.StopEffect(_oilSpill1);
                _SOEffectHandler.StopEffect(_oilSpill2);

                GetComponent<SpriteRenderer>().enabled = false;
                SpawnSwarm();
            }
            else {

                // Have the pod shake violently before it explodes
                if (_move) {
                    transform.position = new Vector2(transform.position.x + 2.5f, transform.position.y);
                    _move = false;
                }
                else if (!_move) {
                    transform.position = new Vector2(transform.position.x - 2.5f, transform.position.y);
                    _move = true;
                }
            }
        }
    }

    public void DestroyPod() {
        
        // This function ensures the player has not hit the battery before. It then sets all necessary variables so the Update
        // function works properly. It also stops the battery indicator effect animation and starts the battery damaged
        //effect animation.
        if (!_batteryDamaged) {
            _timer = Time.time;
            _batteryDamaged = true;
            _SOEffectHandler.StopEffect(_podBatteryIndicatorGO);
            _SOEffectHandler.StopEffect(_podBatteryIndicatorGO1);
            _podBatteryDamageGO = _SOEffectHandler.PlayEffect(EffectEnums.PodBatteryDamage, _effectPositions[2].position);
            _oilSpill1 = _SOEffectHandler.PlayEffect(EffectEnums.PodOilSpill1, _effectPositions[3].position);
            _oilSpill2 = _SOEffectHandler.PlayEffect(EffectEnums.PodOilSpill2, _effectPositions[4].position);
        }
    }

    public void SpawnSwarm() {

        // Destroy the pod object
        if (AllClear != null) {
            AllClear(_swarm);
        }
        Destroy(gameObject);

        // Instantiate as many swarmers as the developer has requested in the inspector.
        for (int i = 0; i < sizeOfSwarm; ++i) {
            _swarm[i] = Instantiate(_swarmPrefab[Random.Range(0, _swarmPrefab.Length)], transform.position, Quaternion.identity) as GameObject;
        }
    }
}
