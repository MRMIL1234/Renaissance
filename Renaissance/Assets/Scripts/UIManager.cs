using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Slider hpSlider;
    [SerializeField] private TextMeshProUGUI hpText;

    public void UpdateHP(int health)
    {
        if (health <= 0)
        {
            hpSlider.value = 0;
            hpText.text = "HP: 0";
        }
        else
        {
            hpSlider.value = health;
            hpText.text = "HP: " + health;
        }
    }
    public void ReloadScene()
    {

    }
}
