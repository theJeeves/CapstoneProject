using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//Create Asset Menu allows us to turn this scriptable object into an asset to be used in the game.
[CreateAssetMenu(menuName ="SO AV Effects/New SO AV Effect")]
public class SOEffects : ScriptableObject {

    [SerializeField]
    private GameObject _effectPrefab;
    [SerializeField]
    private SpriterDotNetUnity.SpriterData _effectData;

    //This is the primary function which will be called in many scripts. An EffectEnum must be given so the correct prefab is instantiated.
    //It is give a position + the offset, and optionally (priamrily for the weapons) the angle.
    public GameObject PlayEffect(Vector2 position, float angle = 0.0f) {

        return PlayVisualEffect(position, angle);
    }

    // Visual effect only. No audio.
    private GameObject PlayVisualEffect(Vector2 position, float angle = 0.0f) {

        if (_effectPrefab != null) {

            GameObject instance = Instantiate(_effectPrefab, position, Quaternion.identity) as GameObject;

            instance.transform.position = new Vector3(position.x, position.y, -1.0f);

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
            Debug.Log("Effect is Missing for " + name);
            return null;
        }
    }

    //Other scripts will call this function when they are dealing with a looping effect animation to explicity delete it when the time is right.
    public void StopEffect(GameObject GO) {
        Destroy(GO);
    }
}
