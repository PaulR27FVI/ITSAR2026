using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    [SerializeField] Image barFill;
    [SerializeField] TMP_Text healthValueText;
    Health myHealth;

    private void Awake()
    {
        myHealth = GetComponentInParent<Health>();
    }
    private void Start()
    {
        if (myHealth == null) myHealth = Health.playerHealth;
        Subscribe();
    }
    private void Subscribe()
    {
        myHealth.HealthChanged -= UpdateBarValues; // evita doppia sottoscrizione
        myHealth.HealthChanged += UpdateBarValues;
        UpdateBarValues(myHealth.Current, myHealth.Max);
    }

    private void OnDisable()
    {
        myHealth.HealthChanged -= UpdateBarValues;
    }

    private void UpdateBarValues(float curHp, float maxHp)
    {
        barFill.fillAmount = curHp / maxHp;
        healthValueText.text = $"{Mathf.Round(curHp)}/{Mathf.Round(maxHp)}";
    }
}
