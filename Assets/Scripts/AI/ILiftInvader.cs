using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ILiftInvader
{
    void UpdateState();
    void WalkTo();
    void OpenLift();
}
public enum LiftInvaderStates
{
    WalkTo,
    OpenLift
}