using UnityEngine;
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

    #region Constants
    private const float MS_CONVERSION = 0.001f;

    #endregion Constants

    #region Private Fields
    private Dictionary<EffectEnums, Effect> m_Map = new Dictionary<EffectEnums, Effect>();

    #endregion Private Fields

    #region Public Methods
    public void LoadEffects() {

        foreach (EffectEnums type in System.Enum.GetValues(typeof(EffectEnums))) {
            if (type == EffectEnums.CrystalBullet || type == EffectEnums.ShotgunBlast) {
                m_Map.Add(type, new Effect(Resources.Load(StringConstantUtility.MAIN_CHARACTER_PATH + type.ToString(), typeof(GameObject)) as GameObject));
            }
            else {
                m_Map.Add(type, new Effect(Resources.Load(StringConstantUtility.EFFECTS_PATH + type.ToString(), typeof(GameObject)) as GameObject,
                    Resources.Load(StringConstantUtility.EFFECTS_PATH + type.ToString(), typeof(SpriterDotNetUnity.SpriterData)) as SpriterDotNetUnity.SpriterData));
            }
        }
    }

    /// <summary>
    /// This is the primary function which will be called in many scripts. An EffectEnum must be given so the correct prefab is instantiated.
    /// It is give a position + the offset, and optionally (priamrily for the weapons) the angle.
    /// </summary>
    /// <param name="type"></param>
    /// <param name="position"></param>
    /// <param name="angle"></param>
    /// <param name="X_direction"></param>
    /// <param name="Y_direction"></param>
    /// <returns></returns>
    public GameObject PlayEffect(EffectEnums type, Vector2 position, float angle = 0.0f, float X_direction = 0.0f, float Y_direction = 0.0f) {

        if (!m_Map.ContainsKey(type)) {
            if (type == EffectEnums.CrystalBullet || type == EffectEnums.ShotgunBlast) {
                m_Map.Add(type, new Effect(Resources.Load(StringConstantUtility.MAIN_CHARACTER_PATH + type.ToString(), typeof(GameObject)) as GameObject));
            }
            else {
                m_Map.Add(type, new Effect(Resources.Load(StringConstantUtility.EFFECTS_PATH + type.ToString(), typeof(GameObject)) as GameObject,
                    Resources.Load(StringConstantUtility.EFFECTS_PATH + type.ToString(), typeof(SpriterDotNetUnity.SpriterData)) as SpriterDotNetUnity.SpriterData));
            }
        }

        Effect effect = m_Map[type];

        GameObject instance = Instantiate(effect.prefab, position, Quaternion.identity) as GameObject;
        instance.transform.position = new Vector3(position.x, position.y, -1.0f);

        instance.transform.localEulerAngles = new Vector3(0.0f, 0.0f, angle);

        // If the effect does not loop, this will automatically destroy the instance after its animation has completed.
        if (instance != null && effect.data != null && !effect.data.Spriter.Entities[0].Animations[0].Looping) {
            Destroy(instance, effect.data.Spriter.Entities[0].Animations[0].Length * MS_CONVERSION);
        }

        // SPECIAL INSTRUCTIONS FOR THE BULLETS (CRYSTALS AND LIGHTNING)
        if (type == EffectEnums.CrystalBullet || type == EffectEnums.ShotgunBlast) {
            instance.GetComponent<AbstractBullet>().Fire(new Vector2(X_direction, Y_direction));
        }

        // This returns a reference to the instance for the effects which loop. Other scripts will need to explicity call
        // StopEffect to detroy it, otherwise the effect will loop forever.
        return instance;
    }

    /// <summary>
    /// Other scripts will call this function when they are dealing with a looping effect animation to explicity delete it when the time is right.
    /// </summary>
    /// <param name="GO"></param>
    public void StopEffect(GameObject GO) {
        Destroy(GO);
    }

    #endregion Public Methods

    #region Classes
    [System.Serializable]
    public class Effect {

        #region Public Fields
        public GameObject prefab;
        public SpriterDotNetUnity.SpriterData data;

        #endregion Public Fields

        #region Public Initializers
        public Effect(GameObject go1, SpriterDotNetUnity.SpriterData go2 = null) {
            prefab = go1;
            data = go2;
        }

        #endregion Public Initializers
    };

    #endregion Classes
}