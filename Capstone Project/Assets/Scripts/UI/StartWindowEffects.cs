using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StartWindowEffects : MonoBehaviour {

    [SerializeField]
    private Transform m_playerSpawnPos;
    [SerializeField]
    private GameObject m_player;
    [SerializeField]
    private SOEffects m_SOEffectHandler;
    [SerializeField]
    private SOEnemyBodyParts m_enemyBodyParts;

    [SerializeField]
    private AudioClip[] m_audioClips;

    private AudioSource _audio;

    private GameObject m_bodyPartsContainer;

    private float m_deployTime = 0.0f;

    private float m_resetTime = 0.0f;
    private float m_defaultResetTime = 60.0f;

    private bool m_start = false;

    private void OnEnable() {
        Reset();

        _audio = GetComponent<AudioSource>();
    }

    private void Update() {

        if (m_start) {
            if (TimeTools.TimeExpired(ref m_deployTime)) {
                DeployBodyParts();
                m_deployTime = Random.Range(0.5f, 2.0f);
            }
            if (TimeTools.TimeExpired(ref m_resetTime)) {
                Reset();
                _audio.clip = m_audioClips[2];
                _audio.Play();
            }
        }
    }

    private void DeployBodyParts() {

        GameObject effectInstance = m_SOEffectHandler.PlayEffect(EffectEnums.SwarmerDeathExplosion, transform.position);
        effectInstance.transform.parent = m_bodyPartsContainer.transform;
        _audio.clip = m_audioClips[Random.Range(0, 2)];
        _audio.pitch = Random.Range(0.75f, 1.5f);
        _audio.Play();
        effectInstance.transform.localScale = new Vector3(0.85f, 0.85f, 1.0f);
        effectInstance.GetComponent<AudioSource>().pitch = Random.Range(0.75f, 1.5f);

        for (int i = 0; i < m_enemyBodyParts.bodyParts.Length; ++i) {
            GameObject instance = Instantiate(m_enemyBodyParts.bodyParts[i], transform.position, Quaternion.identity) as GameObject;
            instance.transform.parent = m_bodyPartsContainer.transform;
            instance.GetComponent<EnableCollider>().mainMenu = true;
            instance.GetComponent<Rigidbody2D>().gravityScale = 2.0f;
            instance.transform.localScale = new Vector3(0.2f, 0.2f, 1.0f);
            instance.GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(-10.0f, -5.0f), Random.Range(4.0f, 15.0f)), ForceMode2D.Impulse);
            instance.GetComponent<Rigidbody2D>().AddTorque(Random.Range(-500.0f, 500.0f));
        }
    }

    public void SetStart() {
        m_start = true;
    }

    private void Reset() {
        m_start = false;
        m_deployTime = Random.Range(0.5f, 2.0f);
        m_resetTime = m_defaultResetTime;
        if (m_bodyPartsContainer != null) {
            Destroy(m_bodyPartsContainer);
        }
        m_bodyPartsContainer = new GameObject();
        m_bodyPartsContainer.name = "Effects";
        Instantiate(m_player, m_playerSpawnPos.position, Quaternion.identity);
    }
}
