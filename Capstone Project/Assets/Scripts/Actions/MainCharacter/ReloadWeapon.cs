using UnityEngine;
using System.Collections;

public class ReloadWeapon : MonoBehaviour {

    public delegate void ReloadWeaponEvent();
    public static event ReloadWeaponEvent Reload;

    private void OnEnable() {
        ControllableObject.OnButtonDown += OnButtonDown;
    }

    private void OnDisable() {
        ControllableObject.OnButtonDown -= OnButtonDown;
    }

    private void OnButtonDown(Buttons button) {
        
        if (button == Buttons.Reload) {

            if (Reload != null) {
                Reload();
            }
        }
    }
}
