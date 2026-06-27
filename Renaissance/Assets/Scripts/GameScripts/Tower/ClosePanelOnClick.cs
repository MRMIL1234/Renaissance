using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Закрывает UpgradePanel при клике на пустое место.
/// Повесь на камеру или пустой объект на сцене.
/// </summary>
public class ClosePanelOnClick : MonoBehaviour
{
    [SerializeField] private GameObject panelToClose;
    [SerializeField] private LayerMask clickableLayer; // Слой башен

    private Camera mainCam;

    private void Awake()
    {
        if (mainCam == null)
            mainCam = Camera.main;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // Кликнули где-то
            Vector2 mousePos = mainCam.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero, Mathf.Infinity, clickableLayer);

            // Если не попали в башню и панель открыта — закрываем
            if (hit.collider == null && panelToClose != null && panelToClose.activeSelf)
            {
                // Проверяем что не кликнули по UI
                if (!EventSystem.current.IsPointerOverGameObject())
                {
                    panelToClose.SetActive(false);
                }
            }
        }
    }
}
