using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class PlacementManager : MonoBehaviour
{
    [Header("Настройки шарів")]
    [SerializeField] private LayerMask towerLayer;

    [Header("UI Панель Апгрейду")]
    [SerializeField] private GameObject upgradePanel;
    [SerializeField] private TextMeshProUGUI upgradeCostText;
    [SerializeField] private TextMeshProUGUI towerInfoText;

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
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero, 0.1f, towerLayer);

            if (hit.collider != null)
            {
                Tower tower = hit.collider.GetComponent<Tower>();
                if (tower != null) OpenUpgradePanel(tower);
            }
            else
            {
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

        if (ghostTower.GetComponent<BoxCollider2D>() != null) ghostTower.GetComponent<BoxCollider2D>().enabled = false;
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

        Collider2D hit = Physics2D.OverlapBox(position, towerSize, 0f, towerLayer);

        if (hit == null)
        {
            Tower prefabScript = currentTowerPrefab.GetComponent<Tower>();
            GameEconomy.Instance.SpendCoins(prefabScript.Stats.BaseCost);

            GameObject newTower = Instantiate(currentTowerPrefab, position, Quaternion.identity);
            newTower.layer = Mathf.RoundToInt(Mathf.Log(towerLayer.value, 2));

            CancelPlacement();
        }
    }

    private void OpenUpgradePanel(Tower tower)
    {
        selectedTower = tower;
        upgradePanel.SetActive(true);
        UpdateUpgradeUI();
    }

    private void CloseUpgradePanel()
    {
        selectedTower = null;
        if (upgradePanel != null) upgradePanel.SetActive(false);
    }

    private void UpdateUpgradeUI()
    {
        if (selectedTower == null) return;

        upgradeCostText.text = $"{selectedTower.CurrentUpgradeCost} Coins";
        // Звертаємося до TowerName
        towerInfoText.text = $"{selectedTower.Stats.TowerName}\n" +
                             $"LVL: {selectedTower.CurrentLevel}\n" +
                             $"DMG: {selectedTower.CurrentDamage:F1}\n" +
                             $"SPD: {selectedTower.CurrentCooldown:F2}s";
    }

    public void TryUpgradeSelectedTower()
    {
        if (selectedTower == null) return;

        if (GameEconomy.Instance.CanAfford(selectedTower.CurrentUpgradeCost))
        {
            GameEconomy.Instance.SpendCoins(selectedTower.CurrentUpgradeCost);
            selectedTower.UpgradeTower();
            UpdateUpgradeUI();
        }
    }

    private void CancelPlacement()
    {
        currentTowerPrefab = null;
        if (ghostTower != null) Destroy(ghostTower);
    }
}