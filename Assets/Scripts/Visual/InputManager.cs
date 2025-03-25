using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance {get; private set;}
    private InputSystem_Actions playerOneInputActions;

    void Awake()
    {
        if(Instance != null)
        {
            Debug.LogError($"More than one Instance of InputManager {transform}");
            return;
        }
        Instance = this;

        playerOneInputActions = new InputSystem_Actions();
        playerOneInputActions.Player1.Enable();
    }

    public float GetHorizontalMovePlayerOne()
    {
        return playerOneInputActions.Player1.PlayerOneHorizontalMove.ReadValue<float>();
    }

    public bool IsRotateButtonPressedPlayerOne()
    {
        return playerOneInputActions.Player1.PlayerOneRotate.WasPressedThisFrame();
    }

    public float GetHorizontalMovePlayerTwo()
    {
        return playerOneInputActions.Player1.PlayerTwoHorizontalMove.ReadValue<float>();
    }

    public bool IsRotateButtonPressedPlayerTwo()
    {
        return playerOneInputActions.Player1.PlayerTwoRotate.WasPressedThisFrame();
    }
}

