using UnityEngine;
using System.Collections;

public class GunBarrelBehavior : MonoBehaviour {

    [SerializeField]
    private bool _aimingDown = false;
    [SerializeField]
    private GameObject _barrel;
    [SerializeField]
    private GameObject _end;

    private ControllableObject _controller;

    private void OnEnable() {
        _controller = GameObject.FindGameObjectWithTag("Player").GetComponent<ControllableObject>();
        
        if (_aimingDown) {
            _barrel.SetActive(false);
            _end.SetActive(false);
        }
        else {
            _barrel.SetActive(true);
            _end.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update () {
	
        if (_aimingDown && _controller.AimDirection == 270) {
            _barrel.SetActive(true);
            _end.SetActive(true);
        }
        else if (_aimingDown && _controller.AimDirection != 270) {
            _barrel.SetActive(false);
            _end.SetActive(false);
        }
        else if (!_aimingDown && _controller.AimDirection == 270) {
            _barrel.SetActive(false);
            _end.SetActive(false);
        }
        else if (!_aimingDown && _controller.AimDirection != 270) {
            _barrel.SetActive(true);
            _end.SetActive(true);
        }
    }
}
