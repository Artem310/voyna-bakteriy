using UnityEngine;

public class InputManager : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private Camera mainCamera;
    [SerializeField] private float swipeMinDistance = 0.5f;
    
    private Colony selectedColony;
    private Vector3 touchStartPosition;
    private bool isDragging;
    
    private void Start()
    {
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }
    }
    
    private void Update()
    {
        HandleInput();
    }
    
    private void HandleInput()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            HandleTouch(touch.phase, touch.position);
        }
        else if (Input.GetMouseButtonDown(0))
        {
            HandleTouch(TouchPhase.Began, Input.mousePosition);
        }
        else if (Input.GetMouseButton(0) && isDragging)
        {
            HandleTouch(TouchPhase.Moved, Input.mousePosition);
        }
        else if (Input.GetMouseButtonUp(0))
        {
            HandleTouch(TouchPhase.Ended, Input.mousePosition);
        }
    }
    
    private void HandleTouch(TouchPhase phase, Vector3 screenPosition)
    {
        Vector3 worldPosition = mainCamera.ScreenToWorldPoint(screenPosition);
        worldPosition.z = 0;
        
        switch (phase)
        {
            case TouchPhase.Began:
                touchStartPosition = worldPosition;
                selectedColony = BattleManager.Instance?.GetColonyAtPosition(worldPosition);
                isDragging = selectedColony != null && selectedColony.Owner == ColonyOwner.Player;
                break;
            
            case TouchPhase.Moved:
                if (isDragging && selectedColony != null)
                {
                    float distance = Vector3.Distance(touchStartPosition, worldPosition);
                    if (distance > swipeMinDistance)
                    {
                    }
                }
                break;
            
            case TouchPhase.Ended:
                if (isDragging && selectedColony != null)
                {
                    float distance = Vector3.Distance(touchStartPosition, worldPosition);
                    if (distance > swipeMinDistance)
                    {
                        Colony targetColony = BattleManager.Instance?.GetColonyAtPosition(worldPosition);
                        if (targetColony != null && targetColony != selectedColony)
                        {
                            float percentage = 0.5f;
                            BattleManager.Instance?.CreateTentacle(selectedColony, targetColony, percentage);
                        }
                    }
                    else
                    {
                        Colony tapTarget = BattleManager.Instance?.GetColonyAtPosition(worldPosition);
                        if (tapTarget != null && tapTarget != selectedColony && selectedColony.Owner == ColonyOwner.Player)
                        {
                            BattleManager.Instance?.CreateTentacle(selectedColony, tapTarget, 0.5f);
                        }
                    }
                }
                
                selectedColony = null;
                isDragging = false;
                break;
        }
    }
}
