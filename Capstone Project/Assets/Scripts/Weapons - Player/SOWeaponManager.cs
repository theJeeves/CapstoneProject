using UnityEngine;
using System.Collections;

public enum WeaponType {
    None,
    Shotgun,
    MachineGun
}

[CreateAssetMenu(menuName ="SOWeaponManager/Create SOWeaponManager")]
public class SOWeaponManager : ScriptableObject {

    [SerializeField]
    private int MGAmmoCapacity;     //Max ammo for machine gun
    public int GetMGAmmoCapacity {
        get { return MGAmmoCapacity; }
    }
    [SerializeField]
    private int SGAmmoCapacity;     //Max ammo for shotgun
    public int GetSGAmmoCapacity {
        get { return SGAmmoCapacity; }
    }

    public bool reloaded;

    private int MG_numOfRounds;     //Current number of rounds for machine gun
    private int SG_numOfRounds;     //Current number of rounds for shotgun
    private bool grounded;          //Keeps track if the player has landed

    private void OnEnable() {
        reloaded = false;
    }

    public void SetAmmoCapacity(WeaponType type, int ammoCapacity) {
        if (type == WeaponType.Shotgun) {
            SGAmmoCapacity = ammoCapacity;
        }
        else if (type == WeaponType.MachineGun) {
            MGAmmoCapacity = ammoCapacity;
        }
    }

    public int GetNumOfRounds(WeaponType type) {
        if (type == WeaponType.Shotgun) {
            return SG_numOfRounds;
        }
        else if (type == WeaponType.MachineGun) {
            return MG_numOfRounds;
        }
        else {
            return 0;
        }
    }

    public void Reload() {
        SG_numOfRounds = SGAmmoCapacity;
        MG_numOfRounds = MGAmmoCapacity;
        reloaded = true;
    }
}
