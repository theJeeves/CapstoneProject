using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public enum EffectEnums {
    AcidBall, AcidBallSplatter, AcidDamageEffect, AcidSquirt, CrystalBullet, CrystalImpact, ExplosionDamageEffect, LandingDust, LightningImpact,
    MGMuzzleFlash, PlayerRespawn, PodBatteryDamage, PodBatteryIndicator, PodExplosion, PodOilSpill1, PodOilSpill2, ShotgunBlast, SniperBullet,
    SniperBulletImpact, SniperDeathExplosion, SniperLaserEffect, SniperTellEffect,/* SwarmerBasicDamage,*/ SwarmerDeathExplosion, SwarmerExplosiveEffect,
    Flying_Swarmer_Exhaust, ChargerExhaust, Player_Death00, Player_Death01
}

//Create Asset Menu allows us to turn this scriptable object into an asset to be used in the game.
[CreateAssetMenu(menuName = "SO Effects/New SO Effect Handler")]
public class SOEffects : ScriptableObject {


    [System.Serializable]
    public class Effect {
        public Effect(GameObject go1, SpriterDotNetUnity.SpriterData go2 = null) {
            prefab = go1;
            data = go2;
        }

        public GameObject prefab;
        public SpriterDotNetUnity.SpriterData data;
    };

    private Dictionary<EffectEnums, Effect> _map = new Dictionary<EffectEnums, Effect>();

    public void LoadEffects() {

        foreach (EffectEnums type in System.Enum.GetValues(typeof(EffectEnums))) {
            if (type == EffectEnums.CrystalBullet || type == EffectEnums.ShotgunBlast) {
                _map.Add(type, new Effect(Resources.Load("MainCharacter/" + type.ToString(), typeof(GameObject)) as GameObject));
            }
            else {
                _map.Add(type, new Effect(Resources.Load("Effects/" + type.ToString(), typeof(GameObject)) as GameObject,
                    Resources.Load("Effects/" + type.ToString(), typeof(SpriterDotNetUnity.SpriterData)) as SpriterDotNetUnity.SpriterData));
            }
        }
    }

    //This is the primary function which will be called in many scripts. An EffectEnum must be given so the correct prefab is instantiated.
    //It is give a position + the offset, and optionally (priamrily for the weapons) the angle.
    public GameObject PlayEffect(EffectEnums type, Vector2 position, float angle = 0.0f) {

        if (!_map.ContainsKey(type)) {
            if (type == EffectEnums.CrystalBullet || type == EffectEnums.ShotgunBlast) {
                _map.Add(type, new Effect(Resources.Load("MainCharacter/" + type.ToString(), typeof(GameObject)) as GameObject));
            }
            else {
                _map.Add(type, new Effect(Resources.Load("Effects/" + type.ToString(), typeof(GameObject)) as GameObject,
                    Resources.Load("Effects/" + type.ToString(), typeof(SpriterDotNetUnity.SpriterData)) as SpriterDotNetUnity.SpriterData));
            }
        }

        Effect effect = _map[type];

        GameObject instance = Instantiate(effect.prefab, position, Quaternion.identity) as GameObject;
        instance.transform.position = new Vector3(position.x, position.y, -1.0f);

        instance.transform.localEulerAngles = new Vector3(0.0f, 0.0f, angle);

        // If the effect does not loop, this will automatically destroy the instance after its animation has completed.
        if (instance != null && effect.data != null && !effect.data.Spriter.Entities[0].Animations[0].Looping) {
            Destroy(instance, effect.data.Spriter.Entities[0].Animations[0].Length * 0.001f);
        }

        // This returns a reference to the instance for the effects which loop. Other scripts will need to explicity call
        // StopEffect to detroy it, otherwise the effect will loop forever.
        return instance;
    }

    //Other scripts will call this function when they are dealing with a looping effect animation to explicity delete it when the time is right.
    public void StopEffect(GameObject GO) {
        Destroy(GO);
    }
}