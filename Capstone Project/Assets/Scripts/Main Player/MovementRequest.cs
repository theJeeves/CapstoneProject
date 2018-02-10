using UnityEngine;

public enum MovementType {
    None,
    Walking,
    Shotgun,
    MachineGun,
    AddForce,
    MainMenuWalking
}

[CreateAssetMenu(menuName ="Movement Request/New Movement")]
public class MovementRequest : ScriptableObject {

    #region Constant Fields
    private const string PLAYER_TAG = "Player";
    private const string ADD_IMPULSE_FORCE = "AddImpulseForce";
    private const string ENQUEUE = "Enqueue";
    #endregion Constant Fields

    #region Fields
    private System.Action<float, float>[] m_GunActions = new System.Action<float, float>[8];

    private GameObject m_Player;
    private float m_XVel;
    private float m_YVel;
    private bool m_Grounded;
    private Vector2 m_ForceRequest = new Vector2(0.0f, 0.0f);

    [SerializeField]
    private float m_WalkSpeed;
    [SerializeField]
    private float m_Recoil;
    [SerializeField]
    private float m_SetVel;
    [SerializeField]
    private float m_AddVel;
    [SerializeField]
    private float m_XMultiplier;
    [SerializeField]
    private float m_XImpulse;
    [SerializeField]
    private float m_YImpulse;
    #endregion Fields


    #region Properties
    public Buttons Button { get; private set; }

    public MovementType MovementType { get; private set; } = MovementType.None;

    #endregion Properties

    #region Initializers
    protected virtual void OnEnable() {

        if (MovementType == MovementType.Shotgun || MovementType == MovementType.MachineGun) {
            m_GunActions[0] = AimRight;
            m_GunActions[1] = AimUpAndRight;
            m_GunActions[2] = AimUp;
            m_GunActions[3] = AimUpAndLeft;
            m_GunActions[4] = AimLeft;
            m_GunActions[5] = AimDownAndLeft;
            m_GunActions[6] = AimDown;
            m_GunActions[7] = AimDownAndRight;
        }
    }

    #endregion Initializers

    #region Public Methods
    public virtual void RequestMovement(Buttons button) {
        Button = button;
        GetPlayerReference();
        m_Player.SendMessage(ENQUEUE, this);
    }

    public virtual void RequestMovement() {
        GetPlayerReference();
        m_Player.SendMessage(ENQUEUE, this);
    }

    public Vector2 Move(Vector3 values, bool grounded = false, int key = 0) {

        switch (MovementType) {
            case MovementType.Walking:

                if (grounded) {
                    if (Button == Buttons.MoveRight) {

                        return new Vector2(Mathf.Clamp(values.x + (m_WalkSpeed * Mathf.Clamp(values.z * 2.0f, 0, 1)), 0.0f, m_WalkSpeed), values.y);
                    }
                    else {
                        return new Vector2(Mathf.Clamp(values.x - (m_WalkSpeed * Mathf.Clamp(values.z * 2.0f, 0, 1)), -m_WalkSpeed, 0.0f), values.y);
                    }
                }
                else {
                    return new Vector2(values.x, values.y);
                }

            case MovementType.Shotgun:
                m_Grounded = grounded;
                m_GunActions[key].Invoke(values.x, values.y);

                return new Vector2(m_XVel, m_YVel);

            case MovementType.MachineGun:
                m_XVel = values.x;
                m_YVel = values.y;

                m_GunActions[key].Invoke(values.x, values.y);

                return new Vector2(m_XVel, m_YVel);


            case MovementType.AddForce:
                if (key == 5 || key == 6 || key == 7) {

                    if (key == 5) { ImpulseDownAndLeft(values.x); }
                    else if (key == 6) { ImpulseDown(); }
                    else if (key == 7) { ImpulseDownAndRight(values.x); }

                    m_Player.SendMessage(ADD_IMPULSE_FORCE, m_ForceRequest);
                }
                return new Vector2(0.0f, 0.0f);

            case MovementType.MainMenuWalking:
                return new Vector2(5.0f, values.y);

            default:
                return new Vector2(values.x, values.y);
        }
    }
    #endregion Public Methods

    #region Private Methods
    private void GetPlayerReference() {
        if (m_Player == null) {
            m_Player = GameObject.FindGameObjectWithTag(PLAYER_TAG);
        }
    }

    private void AimRight(float bodyXvel, float bodyYvel) {

        switch (MovementType) {
            case MovementType.Shotgun:
                m_XVel = !m_Grounded ? -m_Recoil : -m_Recoil * 0.5f;

                // MOVING LEFT OR MOVING RIGHT OR STANDING STILL
                m_YVel = bodyYvel;
                break;

            case MovementType.MachineGun:
                if (m_XVel >= -m_Recoil) {
                    m_XVel -= m_Recoil / 2.0f;
                }

                m_YVel = bodyYvel;
                break;
        }
    }

    private void AimUpAndRight(float bodyXvel, float bodyYvel) {

        switch (MovementType) {

            case MovementType.Shotgun:
                //FALLING
                if (bodyYvel < 0) {
                    m_YVel = bodyYvel + m_Recoil * -(m_AddVel);
                }

                //MOVING LEFT
                if (bodyXvel < 0) {

                    m_XVel = bodyXvel + m_Recoil * -(m_AddVel);

                    //RISING
                    if (bodyYvel >= 0) {
                        m_YVel = m_Recoil * -(m_AddVel);
                    }
                }

                //MOVING RIGHT OR NO X VELOCITY
                else if (bodyXvel >= 0) {

                    m_XVel = m_Recoil * -(m_SetVel);

                    //RISING
                    if (bodyYvel >= 0) {
                        m_YVel = m_Recoil * -(m_SetVel);
                    }
                }
                break;
        }
    }

    private void AimUp(float bodyXvel, float bodyYvel) {

        switch (MovementType) {

            case MovementType.Shotgun:
                m_XVel = bodyXvel;

                //FALLING
                if (bodyYvel < 0) {
                    m_YVel = bodyYvel + m_Recoil * -(m_SetVel);
                }

                //RISING OR STILL
                else if (bodyYvel >= 0) {
                    m_YVel = -(m_Recoil);
                }
                break;
        }
    }

    private void AimUpAndLeft(float bodyXvel, float bodyYvel) {

        switch (MovementType) {

            case MovementType.Shotgun:
                //FALLING
                if (bodyYvel < 0) {
                    m_YVel = bodyYvel + m_Recoil * -(m_AddVel);
                }

                //MOVING LEFT
                if (bodyXvel <= 0) {

                    m_XVel = m_Recoil * (m_SetVel);

                    //RISING
                    if (bodyYvel >= 0) {
                        m_YVel = m_Recoil * -(m_SetVel);
                    }
                }

                //MOVING RIGHT
                else if (bodyXvel > 0) {

                    m_XVel = bodyXvel + m_Recoil * m_AddVel;

                    //RISING
                    if (bodyYvel >= 0) {
                        m_YVel = m_Recoil * -(m_AddVel);
                    }
                }
                break;
        }
    }

    private void AimLeft(float bodyXvel, float bodyYvel) {

        //SHOTGUN
        if (MovementType == MovementType.Shotgun) {
            m_XVel = !m_Grounded ? m_Recoil : m_Recoil * 0.5f;

            //MOVING RIGHT OR MOVING LEFT OR STANDING STILL
            m_YVel = bodyYvel;
        }
        //MACHINEGUN
        else if (MovementType == MovementType.MachineGun) {

            if (m_XVel <= m_Recoil) {
                m_XVel += m_Recoil / 2.0f;
            }
            m_YVel = bodyYvel;
        }
    }

    private void AimDownAndLeft(float bodyXvel, float bodyYvel) {

        switch (MovementType) {

            case MovementType.Shotgun:
                //ON THE GROUND
                if (m_Grounded) {

                    m_YVel = m_Recoil * m_SetVel;

                    //MOVING RIGHT
                    if (bodyXvel > 0) {
                        m_XVel = Mathf.Clamp(bodyXvel + m_Recoil * m_AddVel, m_Recoil * m_AddVel, m_Recoil);
                    }

                    // MOVING LEFT OR STANDING STILL
                    else if (bodyXvel <= 0) {
                        m_XVel = m_Recoil * m_SetVel;
                    }
                }

                //IN THE AIR
                else if (!m_Grounded) {

                    //MOVING RIGHT
                    if (bodyXvel > 0.0f) {

                        m_XVel = Mathf.Clamp(bodyXvel + m_Recoil * m_AddVel, m_Recoil * m_AddVel, m_Recoil);

                        //FALLING (NEGATIVE Y VELOCITY)
                        if (bodyYvel < 0.0f) {
                            m_YVel = m_Recoil * m_SetVel;
                        }

                        //RISING OR ZERO Y VELOCITY
                        else if (bodyYvel >= 0.0f) {
                            m_YVel = m_Recoil * m_SetVel;
                        }
                    }

                    // MOVING LEFT OR STANDING STILL
                    else if (bodyXvel <= 0.0f) {
                        m_XVel = m_Recoil * m_SetVel;
                        m_YVel = m_Recoil * m_SetVel;
                    }
                }
                break;

            case MovementType.MachineGun:
                //FALLING DOWN AND THE Y-VELOCITY IS LESS THAN THE SET RECOIL
                if (m_YVel <= m_Recoil && bodyYvel < 0) {

                    //THIS QUICKLY SLOWS DOWN THE PLAYER FROM FALLING (IRON MAN EFFECT)
                    m_YVel += Mathf.Abs(m_Recoil * (1.3f * (bodyYvel / m_YVel)));

                    if (m_XVel <= m_Recoil * m_XMultiplier) {
                        m_XVel += m_Recoil;
                    }
                }
                break;
        }
    }

    private void AimDown(float bodyXvel, float bodyYvel) {

        switch (MovementType) {

            case MovementType.Shotgun:
                m_YVel = m_Recoil;

                //FALLING (NEGATVIE Y VELOCITY) OR RISING OR ZERO Y VELOCITY
                m_XVel = bodyXvel;
                break;

            case MovementType.MachineGun:
                //FALLING DOWN AND THE Y-VELOCITY IS LESS THAN THE SET RECOIL
                if (m_YVel <= m_Recoil && bodyYvel < 0) {

                    //THIS QUICKLY SLOWS DOWN THE PLAYER FROM FALLING (IRON MAN EFFECT)
                    m_YVel += Mathf.Abs(m_Recoil * (1.3f * (bodyYvel / m_YVel)));
                }

                //AIMING STRIGHT DOWN AND MOVING RIGHT
                else if (bodyXvel > 0) {
                    m_XVel -= 2.0f;
                }

                //AIMING STRIGHT DOWN AND MOVING LEFT
                else if (bodyXvel < 0) {
                    m_XVel += 2.0f;
                }
                break;
        }
    }

    private void AimDownAndRight(float bodyXvel, float bodyYvel) {

        switch (MovementType) {

            //SHOTGUN
            case MovementType.Shotgun:
                //ON THE GROUND
                if (m_Grounded) {

                    m_YVel = m_Recoil * m_SetVel;

                    // MOVING LEFT
                    if (bodyXvel < 0) {
                        m_XVel = -1.0f * Mathf.Clamp(Mathf.Abs(bodyXvel) + m_Recoil * m_AddVel, m_Recoil * m_AddVel, m_Recoil);
                    }

                    //MOVING RIGHT OR STANDING STILL
                    else if (bodyXvel >= 0) {
                        m_XVel = m_Recoil * -m_SetVel;
                    }
                }

                //IN THE AIR
                else if (!m_Grounded) {

                    //MOVING LEFT
                    if (bodyXvel < 0) {

                        m_XVel = -1.0f * Mathf.Clamp(Mathf.Abs(bodyXvel) + m_Recoil * m_AddVel, m_Recoil * m_AddVel, m_Recoil);

                        //FALLING (NEGATIVE Y VELOCITY)
                        if (bodyYvel < 0) {
                            m_YVel = m_Recoil * m_SetVel;
                        }

                        //RISING OR ZERO Y VELOCITY
                        else if (bodyYvel >= 0) {
                            m_YVel = Mathf.Clamp(bodyYvel, m_Recoil * m_AddVel, m_Recoil * 2);
                        }
                    }

                    //MOVING RIGHT OR STANDING STILL
                    else if (bodyXvel >= 0) {
                        m_XVel = m_Recoil * -m_SetVel;
                        m_YVel = m_Recoil * m_SetVel;
                    }
                }
                break;

            //MACHINE GUN
            case MovementType.MachineGun:
                //FALLING DOWN AND THE Y-VELOCITY IS LESS THAN THE SET RECOIL
                if (m_YVel <= m_Recoil && bodyYvel < 0) {

                    //THIS QUICKLY SLOWS DOWN THE PLAYER FROM FALLING (IRON MAN EFFECT)
                    m_YVel += Mathf.Abs(m_Recoil * (1.3f * (bodyYvel / m_YVel)));

                    if (m_XVel >= m_Recoil * -m_XMultiplier) {
                        m_XVel -= m_Recoil;
                    }
                }
                break;
        }
    }


    //THE NEXT THREE FUCNTIONS ARE FOR IMPULSE TYPES ONLY
    private void ImpulseDownAndLeft(float xVel) {
        // CHANGE THE FORCE DEPENDING ON IF THE PLAYER IS MOVING IN THE XDIRECTION OR NOT
        m_ForceRequest = xVel > -0.5f || xVel < 0.5f ? new Vector2(m_XImpulse, m_YImpulse) : new Vector2(0, m_YImpulse);
    }

    private void ImpulseDown() {
        // CHANGE THE FORCE DEPENDING ON IF THE PLAYER IS MOVING IN THE XDIRECTION OR NOT
        m_ForceRequest = new Vector2(0, m_YImpulse + 2500.0f);
    }

    private void ImpulseDownAndRight(float xVel) {
        // CHANGE THE FORCE DEPENDING ON IF THE PLAYER IS MOVING IN THE XDIRECTION OR NOT
        m_ForceRequest = xVel > -0.5f || xVel < 0.5f ? new Vector2(-m_XImpulse, m_YImpulse) : new Vector2(0, m_YImpulse);
    }
    #endregion Private Methods
}
