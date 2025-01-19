using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class InteractionManagerTest : MonoBehaviour
{
    private Camera _mainCamera;

    private bool _spellSelected;
    public GameObject demonAoePrefab;

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
        // spell unselected, also Esc should work same
        if (_spellSelected)
        {
            _spellSelected = false;
        }
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

        Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, 200f, LayerMask.GetMask("Field")))
        {
            //TODO refactor:
            Instantiate(demonAoePrefab, hit.point, Quaternion.Euler(90f, 0f, 0f));
            Debug.Log("Clicked");
        }
    }

    private bool IsPointerOverUI()
    {
        return EventSystem.current.IsPointerOverGameObject();
    }

    // private void SelectSceneObject()
    // {
    //     Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
    //     if (Physics.Raycast(ray, out RaycastHit hit, 200f))
    //     {
    //         selectedObject = hit.collider.gameObject;
    //         Debug.Log($"Selected object: {selectedObject.name}");
    //     }
    // }
}