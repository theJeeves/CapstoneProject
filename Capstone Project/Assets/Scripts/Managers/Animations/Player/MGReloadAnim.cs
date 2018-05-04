
public class MGReloadAnim : ReloadAnimation {

    #region Initializers
    private void OnEnable() {

        // Wire-up events
        MachineGun.StartReloadAnimation += Reload;
        MachineGun.EmptyClip += ZeroFillAmount;
        MachineGun.DisplayAmmo += DisplayAmmo;
    }

    #endregion Initializers

    #region Finalizers
    protected override void OnDisable() {

        // Unwire events
        MachineGun.StartReloadAnimation -= Reload;
        MachineGun.EmptyClip -= ZeroFillAmount;
        MachineGun.DisplayAmmo -= DisplayAmmo;
        base.OnDisable();
    }

    #endregion Finalizers
}