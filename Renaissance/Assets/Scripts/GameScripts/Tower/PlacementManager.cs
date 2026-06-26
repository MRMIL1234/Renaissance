using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class PlacementManager : MonoBehaviour
{
    [Header("Настройки шару")]
    [SerializeField] private LayerMask placementLayer;
    [SerializeField] private int placedTowerLayerNumber = 8;

    [Header("UI Панель Апгрейду")]
    [SerializeField] private GameObject upgradePanel;
    [SerializeField] private TextMeshProUGUI upgradeCostText;
    [SerializeField] private TextMeshProUGUI feedbackText;
    [SerializeField] private TextMeshProUGUI towerInfoText;
    [SerializeField] private Image upgradeButtonImage;
    [SerializeField] private Color affordableColor = new Color(0.2f, 0.7f, 0.2f);
    [SerializeField] private Color unaffordableColor = new Color(0.7f, 0.2f, 0.2f);

    private GameObject currentTowerPrefab;
    private GameObject ghostTower;
    private Tower selectedTower;

    private void Start()
    {
        if (upgradePanel != null) upgradePanel.SetActive(false);
    }

    private void Update()
    {
        if (currentTowerPrefab != null)
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            if (ghostTower != null) ghostTower.transform.position = mousePos;

            if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
            {
                PlaceTower(mousePos);
            }

            if (Input.GetMouseButtonDown(1))
            {
                CancelPlacement();
            }
            return;
        }

        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            // OverlapCircle работает с trigger-коллайдерами, в отличие от Raycast
            Collider2D hit = Physics2D.OverlapCircle(mousePos, 0.3f);

            if (hit != null)
            {
                Tower tower = hit.GetComponent<Tower>();
                if (tower != null)
                {
                    Debug.Log($"[PlacementManager] Башня найдена: {tower.Stats.TowerName}");
                    OpenUpgradePanel(tower);
                }
                else
                {
                    Debug.Log($"[PlacementManager] Кликнули не на башню: {hit.name}");
                    CloseUpgradePanel();
                }
            }
            else
            {
                Debug.Log("[PlacementManager] Кликнули в пустоту");
                CloseUpgradePanel();
            }
        }
    }

    public void SelectTower(GameObject towerPrefab)
    {
        Tower towerScript = towerPrefab.GetComponent<Tower>();
        if (towerScript == null || towerScript.Stats == null) return;

        // Звертаємося до BaseCost з великої літери
        if (!GameEconomy.Instance.CanAfford(towerScript.Stats.BaseCost))
        {
            Debug.Log("Замало монет!");
            return;
        }

        CloseUpgradePanel();
        currentTowerPrefab = towerPrefab;

        ghostTower = Instantiate(towerPrefab);

        // Отключаем ВСЕ коллайдеры на ghost-башне, чтобы она не перехватывала клики
        foreach (Collider2D col in ghostTower.GetComponents<Collider2D>())
        {
            col.enabled = false;
        }

        if (ghostTower.GetComponent<Tower>() != null) ghostTower.GetComponent<Tower>().enabled = false;

        Transform rangeIndicator = ghostTower.transform.Find("RangeIndicator");
        if (rangeIndicator != null)
        {
            rangeIndicator.gameObject.SetActive(true);
            // Звертаємося до AttackRadius
            float diameter = towerScript.Stats.AttackRadius * 2f;
            rangeIndicator.localScale = new Vector3(diameter, diameter, 1f);

            SpriteRenderer rangeSr = rangeIndicator.GetComponent<SpriteRenderer>();
            if (rangeSr != null) rangeSr.color = new Color(1f, 0f, 0f, 0.35f);
        }

        SpriteRenderer towerSr = ghostTower.GetComponent<SpriteRenderer>();
        if (towerSr != null) towerSr.color = new Color(1f, 1f, 1f, 0.6f);
    }

    private void PlaceTower(Vector2 position)
    {
        BoxCollider2D prefabCollider = currentTowerPrefab.GetComponent<BoxCollider2D>();
        Vector2 towerSize = prefabCollider != null ? prefabCollider.size : new Vector2(1f, 1f);

        Collider2D hit = Physics2D.OverlapBox(position, towerSize, 0f, placementLayer);

        if (hit == null)
        {
            Tower prefabScript = currentTowerPrefab.GetComponent<Tower>();
            GameEconomy.Instance.SpendCoins(prefabScript.Stats.BaseCost);

            GameObject newTower = Instantiate(currentTowerPrefab, position, Quaternion.identity);
            newTower.layer = placedTowerLayerNumber;

            CancelPlacement();
        }
    }

    private void OpenUpgradePanel(Tower tower)
    {
        selectedTower = tower;
        Debug.Log($"[PlacementManager] OpenUpgradePanel: {tower.Stats.TowerName}, panel={upgradePanel}");
        if (upgradePanel != null) upgradePanel.SetActive(true);
        UpdateUpgradeUI();
    }

    private void CloseUpgradePanel()
    {
        selectedTower = null;
        if (upgradePanel != null) upgradePanel.SetActive(false);
        if (feedbackText != null) feedbackText.text = "";
    }

    private void UpdateUpgradeUI()
    {
        if (selectedTower == null) return;

        if (upgradeCostText != null)
            upgradeCostText.text = $"{selectedTower.CurrentUpgradeCost} Coins";

        if (towerInfoText != null)
            towerInfoText.text = $"{selectedTower.Stats.TowerName}\n" +
                                 $"LVL: {selectedTower.CurrentLevel}\n" +
                                 $"DMG: {selectedTower.CurrentDamage:F1}\n" +
                                 $"SPD: {selectedTower.CurrentCooldown:F2}s";

        bool canAfford = GameEconomy.Instance.CanAfford(selectedTower.CurrentUpgradeCost);

        if (upgradeButtonImage != null)
            upgradeButtonImage.color = canAfford ? affordableColor : unaffordableColor;

        if (feedbackText != null)
            feedbackText.text = canAfford ? "" : "Недостаточно монет!";

        Debug.Log($"[PlacementManager] UpdateUpgradeUI вызван. Tower: {selectedTower.Stats.TowerName}, Cost: {selectedTower.CurrentUpgradeCost}, CanAfford: {canAfford}");
    }

    public void TryUpgradeSelectedTower()
    {
        if (selectedTower == null)
        {
            Debug.Log("[PlacementManager] Апгрейд: selectedTower == null");
            return;
        }

        Debug.Log($"[PlacementManager] TryUpgrade: Cost={selectedTower.CurrentUpgradeCost}, HasCoins={GameEconomy.Instance.CanAfford(selectedTower.CurrentUpgradeCost)}");

        if (GameEconomy.Instance.CanAfford(selectedTower.CurrentUpgradeCost))
        {
            GameEconomy.Instance.SpendCoins(selectedTower.CurrentUpgradeCost);
            selectedTower.UpgradeTower();
            if (feedbackText != null) feedbackText.text = "";
            UpdateUpgradeUI();
        }
        else
        {
            if (feedbackText != null) feedbackText.text = "Недостаточно монет!";
        }
    }

    private void CancelPlacement()
    {
        currentTowerPrefab = null;
        if (ghostTower != null) Destroy(ghostTower);
    }
}