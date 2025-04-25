using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInput : MonoBehaviour
{
    public static GameInput Instance { get; private set;}
    public event EventHandler OnPausedAction;

    private PlayerInputAction playerInputAction;
    private void Awake()
    {
        Instance = this;

        playerInputAction = new PlayerInputAction();

        playerInputAction.Enable();

        playerInputAction.Player.Pause.performed += Pause_performed;
    }

    private void Pause_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnPausedAction?.Invoke(this, EventArgs.Empty);
    }

    private void OnDestroy()
    {
        playerInputAction.Player.Pause.performed -= Pause_performed;

        playerInputAction.Dispose();
    }

    public Vector2 getMovermentNormalize()
    {
        Vector2 inputVector = playerInputAction.Player.Move.ReadValue<Vector2>();
        inputVector = inputVector.normalized;

        return inputVector;
    }
}