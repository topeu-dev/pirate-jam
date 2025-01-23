using Money;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class InteractionManagerTest : MonoBehaviour
{
    public GameObject demonAoePrefab;
    public WalletController walletController;

    private Camera _mainCamera;

    private int _selectedAction;

    private void Awake()
    {
        _mainCamera = Camera.main;
    }

    private void OnEnable()
    {
        InputActionSingleton.GeneralInputActions.Gameplay.Enable();
        InputActionSingleton.GeneralInputActions.Gameplay.Click.performed += OnClick;
        InputActionSingleton.GeneralInputActions.Gameplay.RightClick.performed += OnRightClick;
    }

    private void OnRightClick(InputAction.CallbackContext obj)
    {
        ResetSpellId();
    }

    private void OnDisable()
    {
        InputActionSingleton.GeneralInputActions.Gameplay.Disable();
        InputActionSingleton.GeneralInputActions.Gameplay.Click.performed -= OnClick;
        InputActionSingleton.GeneralInputActions.Gameplay.RightClick.performed += OnRightClick;
    }

    private void OnClick(InputAction.CallbackContext obj)
    {
        if (IsPointerOverUI())
        {
            return;
        }

        switch (_selectedAction)
        {
            case 0:
                return;
            case 1:
                Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out RaycastHit hit, 200f, LayerMask.GetMask("Field")))
                {
                    if (walletController.HasEnoughMoney(5))
                    {
                        walletController.Buy(5);
                        Instantiate(demonAoePrefab, hit.point, Quaternion.Euler(90f, 0f, 0f));
                    }

                    ResetSpellId();
                }

                return;
        }
    }

    private bool IsPointerOverUI()
    {
        return EventSystem.current.IsPointerOverGameObject();
    }

    public void SelectAction(int actionId)
    {
        _selectedAction = actionId;
    }

    private void ResetSpellId()
    {
        _selectedAction = 0;
    }
}