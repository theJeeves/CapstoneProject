using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//// This list will continue to grow. This is a list of all the possible effects we will have in the game.
//public enum EffectEnum {
//    None,
//    PlayerGrounded, PlayerRespawn,
//    CrystalBulletImpact, MGMuzzelFlash, LightningContact, SGMuzzleFlash,
//    SniperDeathExplosion,
//    SwarmerDeathExplosion,
//    SwarmPodDestruction, PodBatteryDamage, PodBatteryIndicator, PodOilSpill_1, PodOilSpill_2,
//    AcidSwarmerSpill, AcidSwarmerBall, AcidBallSplatter, AcidDamageEffect, ExplosionDamageEffect,
//    ExplosiveSwarmerEffect, SniperTellEffect, SniperLaserEffect, SniperBulletImpact
//}

//Create Asset Menu allows us to turn this scriptable object into an asset to be used in the game.
[CreateAssetMenu(menuName ="SO AV Effects/New SO AV Effect")]
public class SOEffects : ScriptableObject {

    [Header("Visual Effect")]
    [SerializeField]
    private GameObject _effectPrefab;
    [SerializeField]
    private SpriterDotNetUnity.SpriterData _effectData;
    [SerializeField]
    private Vector2 _offset;

    [Space]

    [Header("Sound Effect")]
    [SerializeField]
    private AudioClip _audioClip;
    [SerializeField]
    private VolumeRange _volume;
    [SerializeField]
    private PitchRange _pitch;

    [System.Serializable]
    private struct VolumeRange {

        [Range(0.0f, 1.0f)]
        public float min;
        [Range(0.0f, 1.0f)]
        public float max;
    };

    [System.Serializable]
    private struct PitchRange {

        [Range(0.0f, 2.0f)]
        public float min;
        [Range(0.0f, 2.0f)]
        public float max;
    };

    //// This List is used to populate the dictionary AND allows the developers to keep track of all the effects
    //// in the inspector. DO NOT CLEAR THIS IN THE CODE AS IT WILL ERASE THE SETTINGS EVERY TIME.
    //[SerializeField]
    //private List<EffectProperties> _effects;

    ////A Dictionary is used to receive O(1) search times and ensure there is no lag when an effect animation is requested.
    //private Dictionary<EffectEnum, EffectProperties> _effectsTable = new Dictionary<EffectEnum, EffectProperties>();

    ////This struct is used to hold any additional data developers need when declaring a new usable effect animation.
    //[System.Serializable]
    //private struct EffectProperties {

    //    public EffectEnum effectType;                   //Enum which defines which effect should be played
    //    public GameObject prefab;                       //The prefab to be instantiated
    //    public SpriterDotNetUnity.SpriterData data;     //All aditional data which comes with the prefab. This is needed because of the SpriterDotNet tool
    //    public Vector2 offset;                          //Create an offset if the prefab needs to be adjusted in world space.
    //};

    //private void OnEnable() {

    //    // From the list, populate the dictionary
    //    foreach(EffectProperties effect in _effects) {
    //        _effectsTable.Add(effect.effectType, effect);
    //    }
    //}

    //This is the primary function which will be called in many scripts. An EffectEnum must be given so the correct prefab is instantiated.
    //It is give a position + the offset, and optionally (priamrily for the weapons) the angle.
    public GameObject PlayEffect(AudioSource source, Vector2 position, float angle = 0.0f) {

        PlayAudioEffect(source);

        return PlayVisualEffect(position, angle);
    }

    // Visual effect only. No audio.
    public GameObject PlayVisualEffect(Vector2 position, float angle = 0.0f) {

        if (_effectPrefab != null) {

            GameObject instance = Instantiate(_effectPrefab, position, Quaternion.identity) as GameObject;

            instance.transform.position = new Vector3(position.x + _offset.x, position.y + _offset.y, -1.0f);

            instance.transform.localEulerAngles = new Vector3(0.0f, 0.0f, angle);

            // If the effect does not loop, this will automatically destroy the instance after its animation has completed.
            if (_effectData != null && !_effectData.Spriter.Entities[0].Animations[0].Looping) {
                Destroy(instance, _effectData.Spriter.Entities[0].Animations[0].Length * 0.001f);
            }

            // This returns a reference to the instance for the effects which loop. Other scripts will need to explicity call
            // StopEffect to detroy it, otherwise the effect will loop forever.
            return instance;
        }
        else {
            Debug.Log("Effect is Missing for " + _effectPrefab);
            return null;
        }
    }

    public void PlayAudioEffect(AudioSource source) {

        if (_audioClip != null) {

            source.clip = _audioClip;
            source.volume = Random.Range(_volume.min, _volume.max);
            source.pitch = Random.Range(_pitch.min, _pitch.max);
            source.Play();
        }
        else {
            Debug.Log("No audio clips available.");
        }
    }

    //Other scripts will call this function when they are dealing with a looping effect animation to explicity delete it when the time is right.
    public void StopEffect(GameObject GO) {
        Destroy(GO);
    }
}
