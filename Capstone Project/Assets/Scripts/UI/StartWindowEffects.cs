using UnityEngine;

public class StartWindowEffects : MonoBehaviour
{
    #region Constants
    private const float MIN_DEPLOY_TIME = 0.5f;
    private const float MAX_DEPLOY_TIME = 2.0f;
    private const float MIN_PITCH = 0.75f;
    private const float MAX_PITCH = 1.5f;
    private const float GRAVITY_SCALE = 2.0f;
    private const float MIN_X_FORCE = -10.0f;
    private const float MAX_X_FORCE = -5.0f;
    private const float MIN_Y_FORCE = 4.0f;
    private const float MAX_Y_FORCE = 15.0f;
    private const float TORQUE = 500.0f;
    private const int MIN_AUDIO_CLIP = 0;
    private const int MAX_AUDIO_CLIP = 2;

    #endregion Constants

    #region Fields
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

    private AudioSource m_Audio;
    private GameObject m_BodyPartsContainer;
    private Vector3 m_NormalScale = Vector3.zero;
    private Vector3 m_SmallScale = Vector3.zero;
    private Vector2 m_ForceVector = Vector2.zero;

    private XFloat m_DeployTime = 0.0f;
    private XFloat m_ResetTime = 60.0f;
    private bool m_Start = false;

    #endregion Fields

    #region Initializers
    private void OnEnable()
    {
        Reset();
        m_Audio = GetComponent<AudioSource>();
    }

    private void Start()
    {
        m_NormalScale = new Vector3(0.85f, 0.85f, 1.0f);
        m_SmallScale = new Vector3(0.2f, 0.2f, 1.0f);
    }

    #endregion Initializers

    #region Public Methods
    public void SetStart()
    {
        m_Start = true;
    }

    #endregion Public Methods

    #region Private Methods
    private void Update() {

        if (m_Start)
        {
            if (m_DeployTime.IsExpired)
            {
                DeployBodyParts();
                m_DeployTime = Random.Range(MIN_DEPLOY_TIME, MAX_DEPLOY_TIME);
            }
            if (m_ResetTime.IsExpired)
            {
                Reset();
                m_Audio.clip = m_audioClips[2];
                m_Audio.Play();
            }
        }
    }

    private void DeployBodyParts()
    {
        GameObject effectInstance = m_SOEffectHandler.PlayEffect(EffectEnums.SwarmerDeathExplosion, transform.position);
        effectInstance.transform.parent = m_BodyPartsContainer.transform;
        m_Audio.clip = m_audioClips[Random.Range(MIN_AUDIO_CLIP, MAX_AUDIO_CLIP)];
        m_Audio.pitch = Random.Range(MIN_PITCH, MAX_PITCH);
        m_Audio.Play();
        effectInstance.transform.localScale = m_NormalScale;
        effectInstance.GetComponent<AudioSource>().pitch = Random.Range(MIN_PITCH, MAX_PITCH);

        for (int i = 0; i < m_enemyBodyParts.bodyParts.Length; ++i)
        {
            GameObject instance = Instantiate(m_enemyBodyParts.bodyParts[i], transform.position, Quaternion.identity) as GameObject;
            instance.transform.parent = m_BodyPartsContainer.transform;
            instance.GetComponent<EnableCollider>().mainMenu = true;
            instance.GetComponent<Rigidbody2D>().gravityScale = GRAVITY_SCALE;
            instance.transform.localScale = m_SmallScale;

            m_ForceVector.Set(Random.Range(MIN_X_FORCE, MAX_X_FORCE), Random.Range(MIN_Y_FORCE, MAX_Y_FORCE));
            instance.GetComponent<Rigidbody2D>().AddForce(m_ForceVector, ForceMode2D.Impulse);
            instance.GetComponent<Rigidbody2D>().AddTorque(Random.Range(-TORQUE, TORQUE));
        }
    }

    private void Reset()
    {
        m_Start = false;
        m_DeployTime = Random.Range(MIN_DEPLOY_TIME, MAX_DEPLOY_TIME);
        m_ResetTime.Reset();

        if (m_BodyPartsContainer != null)
        {
            Destroy(m_BodyPartsContainer);
        }
        m_BodyPartsContainer = new GameObject
        {
            name = Tags.EffectsTag
        };
        Instantiate(m_player, m_playerSpawnPos.position, Quaternion.identity);
    }

    #endregion Private Methods
}
