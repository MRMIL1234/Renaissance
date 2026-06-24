using UnityEngine;
using UnityEngine.EventSystems;

public class PlacementManager : MonoBehaviour
{
    [Header("Настройки")]
    public LayerMask towerLayer;
    public GameObject startTowerPrefab;

    private GameObject currentTowerPrefab;
    private GameObject ghostTower;

    void Start()
    {
        if (startTowerPrefab != null)
        {
            SelectTower(startTowerPrefab);
        }
    }

    void Update()
    {
        if (currentTowerPrefab == null) return;

        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (ghostTower != null)
        {
            ghostTower.transform.position = mousePos;
        }

        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            PlaceTower(mousePos);
        }

        if (Input.GetMouseButtonDown(1))
        {
            CancelPlacement();
        }
    }

    public void SelectTower(GameObject towerPrefab)
    {
        currentTowerPrefab = towerPrefab;
        if (ghostTower != null) Destroy(ghostTower);

        ghostTower = Instantiate(towerPrefab);
        ghostTower.GetComponent<BoxCollider2D>().enabled = false;

        Tower towerScript = ghostTower.GetComponent<Tower>();
        if (towerScript != null)
        {
            towerScript.enabled = false;

            Transform rangeIndicator = ghostTower.transform.Find("RangeIndicator");
            if (rangeIndicator != null)
            {
                rangeIndicator.gameObject.SetActive(true);

                float diameter = towerScript.stats.attackRadius * 2f;
                rangeIndicator.localScale = new Vector3(diameter, diameter, 1f);

                SpriteRenderer rangeSr = rangeIndicator.GetComponent<SpriteRenderer>();
                if (rangeSr != null)
                {
                    rangeSr.color = new Color(1f, 0f, 0f, 0.35f);
                }
            }
        }

        SpriteRenderer sr = ghostTower.GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            sr.color = new Color(1f, 1f, 1f, 0.6f);
        }
    }

    private void PlaceTower(Vector2 position)
    {
        BoxCollider2D prefabCollider = currentTowerPrefab.GetComponent<BoxCollider2D>();
        Vector2 towerSize = prefabCollider.size;

        Collider2D hit = Physics2D.OverlapBox(position, towerSize, 0f, towerLayer);

        if (hit == null)
        {
            GameObject newTower = Instantiate(currentTowerPrefab, position, Quaternion.identity);
            newTower.layer = Mathf.RoundToInt(Mathf.Log(towerLayer.value, 2));

            CancelPlacement();
        }
        else
        {
            Debug.Log("Место занято!");
        }
    }

    private void CancelPlacement()
    {
        currentTowerPrefab = null;
        if (ghostTower != null) Destroy(ghostTower);
    }
}