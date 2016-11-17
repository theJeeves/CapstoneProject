using UnityEngine;
using System.Collections;

public class WeaponSelect : AbstractPlayerActions {

    public delegate void WeaponSelectEvent(int weapon);
    public static event WeaponSelectEvent SwapWeapon;

    [SerializeField]
    private GameObject shotgun;
    [SerializeField]
    public GameObject machineGun;

    protected override void OnButtonDown(Buttons button) {
        
        if (button == Buttons.WeaponSwap) {

            if (shotgun.activeInHierarchy) {
                shotgun.SetActive(false);
                machineGun.SetActive(true);
                if (SwapWeapon != null) {
                    SwapWeapon(1);
                }
            }
            else if (machineGun.activeInHierarchy) {
                machineGun.SetActive(false);
                shotgun.SetActive(enabled);
                if (SwapWeapon != null) {
                    SwapWeapon(0);
                }
            }
        }
    }
}
