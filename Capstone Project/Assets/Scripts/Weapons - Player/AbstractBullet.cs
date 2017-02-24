using UnityEngine;
using System.Collections;

public abstract class AbstractBullet : MonoBehaviour {

    [SerializeField]
    protected float _shotSpeed;
    [SerializeField]
    protected int _damageAmount;

    [SerializeField]
    protected Range _directionRange;

    protected Vector2 _start;
    protected Vector2 _end;
    protected Vector2 _direction;

    [System.Serializable]
    protected struct Range {

        [SerializeField]
        private float _min;
        public float Min {
            get { return _min; }
            set { _min = value; }
        }

        [SerializeField]
        private float _max;
        public float Max {
            get { return _max; }
            set { _max = value; }
        }
    }

    protected virtual void Start() {
        //_direction = (_end - _start).normalized;
    }

    protected virtual IEnumerator Shoot() {
        yield return 0;
    }

    public virtual void Fire(Vector2 direction) {
        _direction = direction;
    }
}
