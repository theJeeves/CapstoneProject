using UnityEngine;
using System.Collections;

/*
 * This class will act as the base class for all weapons in the game.
 */

public abstract class AbstractGun : MonoBehaviour {

    // Delegate to work with UI
    public delegate void AbstractGunEvent(int numOfRounds);
    // Delegate to work with ScreenShake feature
    public delegate void AbstractGunEvent2();
    public delegate void AbstractGunEvent3(float reloadTime);

    [SerializeField]
    protected SOWeaponManager _weaponManager;
    [SerializeField]
    protected WeaponType _type;

    [SerializeField]
    protected MovementRequest _moveRequest;
    [SerializeField]
    protected ScreenShakeRequest _SSRequest;
    [SerializeField]
    protected SOEffects _SOEffect;

    [SerializeField]
    protected int _ammoCapacity;

    public int numOfRounds;

    [SerializeField]
    protected float _shotDelay;
    [SerializeField]
    protected float _normReloadTime;
    [SerializeField]
    protected float _fastReloadTime;

    protected float _reloadTime;

    protected bool _reloading = false;
    // This keeps a state of whether the player was in the air or not
    // when the reload was called. This is to prevent the player from calling
    // reload when they landed, switching weapons, and the the slow reload is called.
    protected bool _grounded;

    protected bool _canShoot = true;
    protected bool _damaged = false;

    protected ControllableObject _controller;
    protected PlayerCollisionState _collisionState;

    //[SerializeField]
    //protected GameObject _bullet;
    [SerializeField]
    protected Transform _barrel;

    protected virtual void Awake() {

        _controller = GameObject.FindGameObjectWithTag("Player").GetComponent<ControllableObject>();
        _collisionState = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCollisionState>();
    }

    // The player cannot attack for a brief amount of time after they have received damage.
    protected virtual void DamageReceived(int ignore) {
        StartCoroutine(DamageDelay());
    }

    protected virtual IEnumerator DamageDelay() {

        _damaged = true;
        _canShoot = false;
        yield return new WaitForSeconds(1.0f);
        _damaged = false;
        _canShoot = true;
    }

    protected abstract void OnButtonDown(Buttons button);
}

