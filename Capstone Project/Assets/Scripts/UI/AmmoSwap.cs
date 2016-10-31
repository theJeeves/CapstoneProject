using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class AmmoSwap : MonoBehaviour {

    [SerializeField]
    private Sprite _shotgun;
    [SerializeField]
    private Sprite _machineGun;

	private void OnEnable() {
        WeaponSelect.SwapWeapon += SwapAmmoType;
    }

    private void OnDisable() {
        WeaponSelect.SwapWeapon -= SwapAmmoType;
    }

    /*
     * We have to change the positions and the scale at this point because
     * I just grabbed some clip art form online and they are not the same size.
     * This should not be an issue when we have Hao create the art assest in house.
     */
    private void SwapAmmoType(int weapon) {

        if (weapon == 0) {
            transform.localPosition = new Vector3(-13.0f, 0.0f, 0.0f);
            transform.localScale = new Vector3(0.25f, 0.25f, 1.0f);

            GetComponent<Image>().sprite = _shotgun;
        }
        else if (weapon == 1) {
            transform.localPosition = new Vector3(-16.0f, 0.0f, 0.0f);
            transform.localScale = new Vector3(0.4f, 0.4f, 1.0f);

            GetComponent<Image>().sprite = _machineGun;
        }
    }
}
