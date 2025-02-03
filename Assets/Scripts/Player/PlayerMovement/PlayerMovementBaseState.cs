using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerMovementBaseState
{
    public abstract void EnterState(PlayerMovementStateManager player);

    public abstract void UpdateState(PlayerMovementStateManager player);

    public abstract void OnCollisionEnter(PlayerMovementStateManager player);
}