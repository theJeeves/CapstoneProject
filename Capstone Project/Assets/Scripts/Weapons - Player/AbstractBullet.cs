using UnityEngine;
using System.Collections;

public abstract class AbstractBullet : MonoBehaviour {

    public delegate void AbstractBulletEvent(int damage, GameObject whatGotHit);
    public delegate void AbstractBulletEvent2(GameObject whatGotHit, Vector3 direction);

    [SerializeField]
    protected float _shotSpeed;
    [SerializeField]
    protected int _damageAmount;

    [SerializeField]
    protected Range _directionRange;

    protected Vector3 _start;
    protected Vector3 _end;
    protected Vector3 _direction;

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
        _start = GameObject.FindGameObjectWithTag("Start").transform.position;
        _end = GameObject.FindGameObjectWithTag("End").transform.position;
        _direction = (_end - _start).normalized;
    }

    protected virtual IEnumerator Shoot() {
        yield return 0;
    }
}
