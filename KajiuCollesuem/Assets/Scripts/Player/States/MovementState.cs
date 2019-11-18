﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementState : BaseState
{
    PlayerStateController stateController;

    public MovementState(PlayerStateController controller) : base(controller.gameObject)
    {
        stateController = controller;
    }

    public override void Enter()
    {
        stateController._movementComponent.disableMovement = false;
    }

    public override void Exit()
    {
        stateController._movementComponent.disableMovement = true;
    }

    public override Type Tick()
    {
        //Get Input
        stateController._movementComponent.horizontalInput = stateController.horizontalInput;
        stateController._movementComponent.verticalInput = stateController.verticalInput;
        stateController._movementComponent.jumpInput = stateController.jumpInput;

        //Get OnGround
        stateController._movementComponent.OnGround = stateController.OnGround;

        //Dodge
        if (stateController.longDodgeInput || stateController.shortDodgeInput)
        {
            return typeof(DodgeState);
        }

        //Attack
        if (stateController.heavyAttackInput || stateController.quickAttackInput || stateController.powerInput > 0)
        {
            return typeof(AttackState);
        }

        //Dead
        if (stateController._playerAttributes.getHealth() <= 0)
        {
            return typeof(DeathState);
        }

        //Stunned
        if (stateController.Stunned)
        {
            return typeof(StunnedState);
        }

        return null;
    }
}
