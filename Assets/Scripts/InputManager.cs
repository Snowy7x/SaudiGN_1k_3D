using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance;
    
    private Vector2 _moveDir;
    private Vector2 _lookDir;
    public bool jump;
    public bool fire;
    public UnityEvent onInteract;
    private bool _canInteract = false;

    private void Awake()
    {
        if (Instance)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        _canInteract = true;
    }

    #region Input Callbacks

    public void Move(InputAction.CallbackContext context)
    {
        _moveDir = context.ReadValue<Vector2>();
    }
    
    public void Look(InputAction.CallbackContext context)
    {
        _lookDir = context.ReadValue<Vector2>();
    }
    
    public void Fire(InputAction.CallbackContext context)
    {
        if (HUD.instance.IsPause()) return;
        if (context.performed) fire = true;
        if (context.canceled) fire = false;
    }
    
    public void Interact(InputAction.CallbackContext context)
    {
        if (HUD.instance.IsPause()) return;
        if (!_canInteract) return;
        StartCoroutine(Interaction());
        onInteract?.Invoke();
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (HUD.instance.IsPause()) return;
        jump = true;
    }

    public void Pause(InputAction.CallbackContext context)
    {
        HUD.instance.Pause();
    }

    #endregion

    #region Public Functions

    public Vector2 GetMoveDir()
    {
        return _moveDir;
    }

    public Vector2 GetLookDir()
    {
        return _lookDir;
    }

    public void ResetJump()
    {
        jump = false;
    }

    #endregion

    IEnumerator Interaction()
    {
        _canInteract = false;
        yield return new WaitForSeconds(0.2f);
        _canInteract = true;
    }
}
