using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IExpirable<T> where T : struct
{
    #region Properties
    /// <summary>
    /// Returns the default value of the XFloat. When Reset() is called, this is the value
    /// the XFloat will be assigned.
    /// </summary>
    T DefaultValue { get; }

    /// <summary>
    /// Checks if the current IExpirable object has met or exceeded its timer duration.
    /// </summary>
    /// <returns></returns>
    bool IsExpired { get; }

    #endregion Properties

    #region Methods

    /// <summary>
    /// Resets the IExpirable object's current value to its originally set (Default) value.
    /// </summary>
    void Reset();

    #endregion Methods
}
