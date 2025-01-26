using Money;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using Utility;
using Visualizer;

public class InteractionManagerTest : MonoBehaviour
{
    public GameObject demonAoePrefab;
    public WalletController walletController;


    public EnchanterVisualizer enchanterVisualizer;

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
                if (Physics.Raycast(ray, out RaycastHit hit, 200f, LayerMask.GetMask("Field",
                        "Obstacle")))
                {
                    
                    if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Obstacle"))
                    {
                        EventManager.NotificationEvent.OnWrongSpawnPoint?.Invoke(this);
                        return;
                    }
                    if (walletController.HasEnoughMoney(5))
                    {
                        walletController.Buy(5);

                        var closestCitizen = FindClosestWithTag(hit.point, 15f, "Citizen");

                        if (closestCitizen)
                        {
                            Instantiate(demonAoePrefab, hit.point,
                                Quaternion.LookRotation(hit.point - closestCitizen.transform.position));
                        }
                        else
                        {
                            Instantiate(demonAoePrefab, hit.point, Quaternion.identity);
                        }
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
        if (_selectedAction == 1)
        {
            enchanterVisualizer.EnableVisualizer();
        }
    }

    private void ResetSpellId()
    {
        _selectedAction = 0;
        enchanterVisualizer.DisableVisualizer();
    }


    GameObject FindClosestWithTag(Vector3 center, float radius, string tag)
    {
        Collider[] colliders = Physics.OverlapSphere(center, radius); // Все объекты в радиусе
        GameObject closestObject = null;
        float closestDistance = Mathf.Infinity;

        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag(tag)) // Проверяем, соответствует ли объект нужному тегу
            {
                float distance = Vector3.Distance(center, collider.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestObject = collider.gameObject;
                }
            }
        }

        return closestObject;
    }
}