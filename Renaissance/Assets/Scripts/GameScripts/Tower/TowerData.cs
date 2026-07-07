using UnityEngine;

[CreateAssetMenu(fileName = "NewTowerData", menuName = "Tower Defense/Smart Tower Data")]
public class TowerData : ScriptableObject
{
    [Header("Базові налаштування")]
    [SerializeField] private string towerName;
    [SerializeField] private Sprite sprite;
    [SerializeField] private int baseCost;

    [Header("Характеристики 1-го рівня")]
    [SerializeField] private float baseDamage;
    [SerializeField] private float attackRadius;
    [SerializeField] private float baseAttackCooldown;

    [Header("Коефіцієнти Апгрейду")]
    [SerializeField] private int baseUpgradeCost;
    [SerializeField, Tooltip("На скільки множиться шкода (наприклад, 1.3 = +30%)")]
    private float damageMultiplier = 1.3f;
    [SerializeField, Tooltip("На скільки множиться кулдаун (наприклад, 0.9 = на 10% швидше)")]
    private float cooldownMultiplier = 0.9f;
    [SerializeField, Tooltip("На скільки дорожчає наступний рівень (наприклад, 1.5 = +50%)")]
    private float costMultiplier = 1.5f;
    [SerializeField, Tooltip("На скільки множиться радіус атаки (наприклад, 1.1 = +10% до радіусу)")]
    private float radiusMultiplier = 1.1f;

    // Публічні властивості тільки для читання (Getters)
    public string TowerName => towerName;
    public Sprite Sprite => sprite;
    public int BaseCost => baseCost;
    public float BaseDamage => baseDamage;
    public float AttackRadius => attackRadius;
    public float BaseAttackCooldown => baseAttackCooldown;
    public int BaseUpgradeCost => baseUpgradeCost;
    public float DamageMultiplier => damageMultiplier;
    public float CooldownMultiplier => cooldownMultiplier;
    public float CostMultiplier => costMultiplier;
    public float RadiusMultiplier => radiusMultiplier;
}