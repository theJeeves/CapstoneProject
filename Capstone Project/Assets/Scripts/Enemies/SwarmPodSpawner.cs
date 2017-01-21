using UnityEngine;
using System.Collections;

public class SwarmPodSpawner : MonoBehaviour {

    [SerializeField]
    private GameObject _swarmPrefab;                //Prefab of the Pod sprite and its scripts
    [SerializeField]
    private int _sizeOfSwarm = 0;                   //The number of swarmers the developer wants to spawn from this instance.
    [SerializeField]
    private float _destructionDelay = 0.0f;         //How long the pod should remain in a damaged state.
    [SerializeField]
    private SOEffects _SOEffect;                    //Get a reference to the SOEffectHandler to start and stop effect animations.

    private bool _batteryDamaged = false;           //Bool to determine if the player has hit the battery already
    private float _timer = 0.0f;                    //For delay purposes in Update

    // Since these two effect animations are "looping", the effects manager will never stop or destroy them automatically.
    //Therefore, we need to have a reference to their instance at manually tell the SOEffectHandler when to destroy them.
    private GameObject _podBatteryIndicatorGO;
    private GameObject _podBatteryDamageGO;

    private void OnEnable() {

        // Start the battery indication effect animation
        _podBatteryIndicatorGO = _SOEffect.PlayEffect(EffectEnum.PodBatteryIndicator, GetComponentInChildren<Transform>().position);
    }

    private void Update() {

        // if the player has hit the battery with the shotgun blast. Once they have, this waits for the delay
        // before starting the explosion effect animation, stoping the battery damage effect animation, and hides
        // sprite representing the pod. Lastly, it calls the fucntion to spawn all the swarmers.
        if (_batteryDamaged) {

            if (Time.time - _timer > _destructionDelay) {
                _SOEffect.PlayEffect(EffectEnum.SwarmPodDestruction, transform.position);
                _SOEffect.StopEffect(_podBatteryDamageGO);
                GetComponent<SpriteRenderer>().enabled = false;
                SpawnSwarm();
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
            _SOEffect.StopEffect(_podBatteryIndicatorGO);
            _podBatteryDamageGO = _SOEffect.PlayEffect(EffectEnum.PodBatteryDamage, GetComponentInChildren<Transform>().position);
        }
    }

    public void SpawnSwarm() {

        // Instantiate as many swarmers as the developer has requested in the inspector.
        for (int i = 0; i < _sizeOfSwarm; ++i) {
            Instantiate(_swarmPrefab, transform.position, Quaternion.identity);
        }

        // Destroy the pod object
        Destroy(gameObject);
    }
}
