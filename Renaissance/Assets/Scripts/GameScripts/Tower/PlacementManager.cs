using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class PlacementManager : MonoBehaviour
{
    [Header("Настройки шару")]
    [SerializeField] private LayerMask placementLayer;
    [SerializeField] private int placedTowerLayerNumber = 6;

    [Header("Upgrade Panel UI")]
    [SerializeField] private GameObject upgradePanel;
    [SerializeField] private TextMeshProUGUI upgradeCostText;
    [SerializeField] private TextMeshProUGUI feedbackText;
    [SerializeField] private TextMeshProUGUI towerNameText;
    [SerializeField] private TextMeshProUGUI towerCostText;
    [SerializeField] private TextMeshProUGUI towerDamageText;
    [SerializeField] private TextMeshProUGUI towerLevelText;
    [SerializeField] private Image upgradeButtonImage;
    [SerializeField] private Button upgradeButton;
    [SerializeField] private Color affordableColor = new Color(0.2f, 0.7f, 0.2f);
    [SerializeField] private Color unaffordableColor = new Color(0.7f, 0.2f, 0.2f);
    [SerializeField] private Color disabledColor = new Color(0.5f, 0.5f, 0.5f);

    private GameObject currentTowerPrefab;
    private GameObject ghostTower;
    private Tower selectedTower;
    private Transform activeRangeIndicator;

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
                PlaceTower(mousePos); // тут
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

            int towerLayerMask = 1 << placedTowerLayerNumber;
            Collider2D hit = Physics2D.OverlapCircle(mousePos, 0.5f, towerLayerMask);

            if (hit != null)
            {
                Tower tower = hit.GetComponent<Tower>();
                if (tower != null)
                {
                    OpenUpgradePanel(tower);
                }
                else
                {
                    CloseUpgradePanel();
                }
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

        if (!GameEconomy.Instance.CanAfford(towerScript.Stats.BaseCost))
        {
            Debug.Log("Замало монет!");
            return;
        }

        CloseUpgradePanel();
        currentTowerPrefab = towerPrefab;

        ghostTower = Instantiate(towerPrefab);

        foreach (Collider2D col in ghostTower.GetComponents<Collider2D>())
        {
            col.enabled = false;
        }

        if (ghostTower.GetComponent<Tower>() != null) ghostTower.GetComponent<Tower>().enabled = false;

        Transform rangeIndicator = ghostTower.transform.Find("RangeIndicator");
        if (rangeIndicator != null)
        {
            rangeIndicator.gameObject.SetActive(true);
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

        Collider2D groundHit = Physics2D.OverlapBox(position, towerSize * 0.9f, 0f, placementLayer);

        if (groundHit != null && groundHit.gameObject.layer.Equals("Ground") )
        {
            Debug.Log("Ground");
        }
        else
        {
            CancelPlacement();
            return;
        }
        if (groundHit != null)
        {
            int towerLayerMask = 1 << placedTowerLayerNumber;
            Collider2D towerHit = Physics2D.OverlapBox(position, towerSize * 0.9f, 0f, towerLayerMask);

            if (towerHit == null)
            {
                Tower prefabScript = currentTowerPrefab.GetComponent<Tower>();
                GameEconomy.Instance.SpendCoins(prefabScript.Stats.BaseCost);

                GameObject newTower = Instantiate(currentTowerPrefab, position, Quaternion.identity);
                newTower.layer = placedTowerLayerNumber;

                CancelPlacement();
            }
        }
    }

    private void OpenUpgradePanel(Tower tower)
    {
        if (activeRangeIndicator != null) activeRangeIndicator.gameObject.SetActive(false);

        selectedTower = tower;

        if (selectedTower != null)
        {
            activeRangeIndicator = selectedTower.transform.Find("RangeIndicator");
            if (activeRangeIndicator != null)
            {
                float diameter = selectedTower.CurrentAttackRadius * 2f;
                activeRangeIndicator.localScale = new Vector3(diameter, diameter, 1f);
                activeRangeIndicator.gameObject.SetActive(true);
            }
        }

        if (upgradePanel != null) upgradePanel.SetActive(true);
        UpdateUpgradeUI();
    }

    private void CloseUpgradePanel()
    {
        if (activeRangeIndicator != null)
        {
            activeRangeIndicator.gameObject.SetActive(false);
            activeRangeIndicator = null;
        }

        selectedTower = null;
        if (upgradePanel != null) upgradePanel.SetActive(false);
        if (feedbackText != null) feedbackText.text = "";
    }

    private void UpdateUpgradeUI()
    {
        if (selectedTower == null) return;

        if (upgradeCostText != null)
            upgradeCostText.text = $"{selectedTower.CurrentUpgradeCost} Coins";

        if (towerNameText != null)
            towerNameText.text = $"{selectedTower.Stats.TowerName}";

        if (towerCostText != null)
            towerCostText.text = $"Cost: {selectedTower.CurrentUpgradeCost}";

        if (towerDamageText != null)
            towerDamageText.text = $"Damage: {selectedTower.CurrentDamage}";

        if (towerLevelText != null)
            towerLevelText.text = $"LVL: {selectedTower.CurrentLevel}";

        bool canAfford = GameEconomy.Instance.CanAfford(selectedTower.CurrentUpgradeCost);

        if (upgradeButtonImage != null)
            upgradeButtonImage.color = canAfford ? affordableColor : disabledColor;

        if (upgradeButton != null)
            upgradeButton.interactable = canAfford;

        if (feedbackText != null)
            feedbackText.text = "";

        if (activeRangeIndicator != null)
        {
            float diameter = selectedTower.CurrentAttackRadius * 2f;
            activeRangeIndicator.localScale = new Vector3(diameter, diameter, 1f);
        }
    }

    public void RefreshUpgradeUIIfActive()
    {
        if (upgradePanel != null && upgradePanel.activeInHierarchy && selectedTower != null)
        {
            UpdateUpgradeUI();
        }
    }

    public void TryUpgradeSelectedTower()
    {
        if (selectedTower == null) return;

        if (GameEconomy.Instance.CanAfford(selectedTower.CurrentUpgradeCost))
        {
            GameEconomy.Instance.SpendCoins(selectedTower.CurrentUpgradeCost);
            selectedTower.UpgradeTower();
            if (feedbackText != null) feedbackText.text = "";
            UpdateUpgradeUI();
        }
        else
        {
            if (feedbackText != null) feedbackText.text = "Not enough coins!";
        }
    }

    private void CancelPlacement()
    {
        currentTowerPrefab = null;
        if (ghostTower != null) Destroy(ghostTower);
    }
}