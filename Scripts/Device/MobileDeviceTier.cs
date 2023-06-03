using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Unidork.DeviceUtility
{
    /// <summary>
    /// Mobile device tier. Note that device tiers are relative and can (and should) change 
    /// between projects based on their quaility settings and needs.
    /// </summary>
    public enum MobileDeviceTier
    {
        Low,
        Mid,
        High
    }
}