using UnityEngine;

[CreateAssetMenu(fileName = "NewTowerData", menuName = "Tower Defense/Tower Data")]
public class TowerData : ScriptableObject
{
    public string towerName;
    public Sprite sprite;
    public int cost;
    public float damage;
    public float attackRadius;
    public float attackCooldown;
    public int level = 1;
    public int upgradeCost;
}