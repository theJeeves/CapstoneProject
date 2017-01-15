﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum EffectEnum {
    None,
    Grounded,
    MGImpact,
    MGMuzzelFlash,
    SniperDeathExplosion,
    SwarmerDeathExplosion
}

[CreateAssetMenu(menuName ="SO Effects/New SO Effect")]
public class SOEffects : ScriptableObject {

    [SerializeField]
    private List<EffectProperties> _effects;

    private Dictionary<EffectEnum, EffectProperties> _effectsTable = new Dictionary<EffectEnum, EffectProperties>();

    [System.Serializable]
    private struct EffectProperties {

        public EffectEnum effectType;
        public GameObject prefab;
        public AnimationClip _animationClip;
        public Vector2 offset;
    };

    private void OnEnable() {

        foreach(EffectProperties effect in _effects) {
            _effectsTable.Add(effect.effectType, effect);
        }
    }

    public void PlayEffect(EffectEnum effectType, Vector3 position, float angle = 0.0f) {

        GameObject instance = Instantiate(_effectsTable[effectType].prefab, position, Quaternion.identity) as GameObject;
        instance.transform.position = new Vector3(position.x + _effectsTable[effectType].offset.x, 
                                                  position.y + _effectsTable[effectType].offset.y, position.z);
        instance.transform.localEulerAngles = new Vector3(0.0f, 0.0f, angle);

        instance.GetComponent<Animator>().Play(_effectsTable[effectType]._animationClip.name);
        Destroy(instance, _effectsTable[effectType]._animationClip.length);
    }
}