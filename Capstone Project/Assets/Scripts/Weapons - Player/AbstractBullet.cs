using UnityEngine;
using System.Collections;

public abstract class AbstractBullet : MonoBehaviour {

    #region Fields
    [SerializeField]
    protected float m_ShotSpeed;
    [SerializeField]
    protected int m_DamageAmount;

    [SerializeField]
    protected Range m_DirectionRange;

    protected Vector2 m_Start;
    protected Vector2 m_End;
    protected Vector2 m_Direction;

    #endregion Fields

    #region Initializers
    protected virtual void Start() {}

    #endregion Initializers

    #region Protected Methods
    protected virtual IEnumerator Shoot() {
        yield return 0;
    }

    #endregion Protected Methods

    #region Public Methods
    public virtual void Fire(Vector2 direction) {
        m_Direction = direction;
    }

    #endregion Public Methods

    #region Structs
    [System.Serializable]
    protected struct Range {

        #region Properties
        [SerializeField]
        public float Min { get; set; }

        [SerializeField]
        public float Max { get; set; }

        #endregion Properties
    }
    #endregion Structs
}
