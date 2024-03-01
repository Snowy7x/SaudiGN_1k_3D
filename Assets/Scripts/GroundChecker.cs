using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundChecker : MonoBehaviour
{
    public Movement _movement;
    public Vector3 checkRange = Vector3.one;

    private void Update()
    {
        if (Physics.CheckBox(transform.position, checkRange, transform.rotation,_movement.groundLayer)) _movement.isGrounded = true;
        else _movement.isGrounded = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, checkRange);
    }
}
