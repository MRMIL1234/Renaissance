using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Обрабатывает клики по башням для открытия меню апгрейда.
/// </summary>
public class TowerClickHandler : MonoBehaviour
{
    [Header("Настройки")]
    [SerializeField] private LayerMask towerLayer; // Слой башен
    [SerializeField] private Camera mainCamera;
    [SerializeField] private UIManager uiManager;

    private void Awake()
    {
        // Если камера не назначена — берём главную
        if (mainCamera == null)
            mainCamera = Camera.main;
    }

    private void Update()
    {
        // Клик левой кнопкой мыши
        if (Input.GetMouseButtonDown(0))
        {
            TryClickTower();
        }
    }

    private void TryClickTower()
    {
        // Не реагируем если кликнули по UI
        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
            return;

        // Получаем позицию мыши в мире
        Vector2 mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);

        // Raycast от позиции мыши
        RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero, Mathf.Infinity, towerLayer);

        if (hit.collider != null)
        {
            Tower tower = hit.collider.GetComponent<Tower>();
            Debug.Log($"Click detected! Tower: {tower}, UIManager: {uiManager}"); // Добавь это

            if (tower != null && uiManager != null)
            {
                uiManager.OpenUpgradePanel(tower);
            }
            else if (tower == null)
            {
                Debug.LogWarning("Tower component not found on hit object!");
            }
            else if (uiManager == null)
            {
                Debug.LogError("UIManager is NULL in TowerClickHandler!");
            }
        }
    }

    // Для отладки: рисуем луч в редакторе
    private void OnDrawGizmos()
    {
        if (mainCamera == null) return;

        Vector2 mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(mousePos, 0.2f);
    }
}
