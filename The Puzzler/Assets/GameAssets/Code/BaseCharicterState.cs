using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseCharicterState : MonoBehaviour
{

    public void Enter()
    {

    }

    public void Exit()
    {

    }

    public CHARICTER_STATES Input()
    {
        return CHARICTER_STATES.STAND;
    }

    public CHARICTER_STATES Cycle()
    {
        return CHARICTER_STATES.STAND;
    }

    public CHARICTER_STATES Collision(DIRECTIONS direction, string tag)
    {
        return CHARICTER_STATES.STAND;
    }

    public CHARICTER_STATES NotCollided(DIRECTIONS direction)
    {
        return CHARICTER_STATES.STAND;
    }

}

