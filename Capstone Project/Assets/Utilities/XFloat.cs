using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Implementing IExpirable concretely, this is a modified float.
/// Allows a float to be used for determining time durations.
/// </summary>
public class XFloat : IExpirable<float>
{
    #region Fields
    private float m_CurrentValue = float.NaN;

    #endregion Fields

    #region Constructor / Initializers
    public XFloat(float defaultValue)
    {
        DefaultValue = defaultValue;
        m_CurrentValue = DefaultValue;
    }

    public static implicit operator XFloat(float value)
    {
        return new XFloat(value);
    }

    public static implicit operator XFloat(double value)
    {
        return new XFloat((float)value);
    }

    #endregion Constructor / Initializers

    #region Properties
    /// <summary>
    /// Returns the default value of the XFloat. When Reset() is called, this is the value
    /// the XFloat will be assigned.
    /// </summary>
    public float DefaultValue { get; private set; }

    /// <summary>
    /// Checks if the current IExpirable object has met or exceeded its timer duration.
    /// </summary>
    /// <returns></returns>
    public bool IsExpired
    {
        get
        {
            if (m_CurrentValue.Equals(float.NaN)) { return true; }

            m_CurrentValue -= Time.deltaTime;
            return m_CurrentValue <= 0.0f ? true : false;
        }
    }

    #endregion Properties

    #region Public Methods
    /// <summary>
    /// Resets the IExpirable object to its originally set (Default) value.
    /// </summary>
    public void Reset()
    {
        m_CurrentValue = DefaultValue;
    }

    #endregion Public Methods
}
