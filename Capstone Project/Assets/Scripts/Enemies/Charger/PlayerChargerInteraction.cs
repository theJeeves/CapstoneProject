using UnityEngine;
using System.Collections;

public class PlayerChargerInteraction : MonoBehaviour {
    //private float _knockBack;
    private float _launchDistance;
    private Rigidbody2D _playerBody2d;
    private Rigidbody2D _chargerBody2d;

    public delegate void PlayerKnockUpEvent();
    public static event PlayerKnockUpEvent PlayerKnockedUp;

    private void OnEnable()
    {
        //ChargerDealDamage.KnockUp += KnockUp;
    }

    private void OnDisable()
    {
        //ChargerDealDamage.KnockUp -= KnockUp;
    }

    void Start()
    {
        //_knockBack = 50.0f;
        _launchDistance = 20000.0f;
        _playerBody2d = GetComponent<Rigidbody2D>();
        //_chargerBody2d = GameObject.Find("Charger").GetComponentInChildren<Rigidbody2D>();

        if(PlayerKnockedUp != null)
        {
            PlayerKnockedUp();
        }
    }

    private void KnockUp()
    {
        //if (PlayerKnockedUp != null)
        //{

        //Debug.Log("launch player");
        _playerBody2d.AddForce(Vector2.up * _launchDistance, ForceMode2D.Impulse);
        

        //Debug.Log("Launced PLayer");
        //}
    }

}
