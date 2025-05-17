using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public Slider ramAHealth;
    public Slider ramBHealth;
    public TMP_Text winnerText;
    public GoatStats ramAStats;
    public GoatStats ramBStats;

    void Awake() => Instance = this;

    public void UpdateHealthBars()
    {
        ramAHealth.value = ramAStats.GetHealthPercent();
        ramBHealth.value = ramBStats.GetHealthPercent();
    }

    public void ShowWinner(string winner)
    {
        winnerText.text = winner;
        winnerText.gameObject.SetActive(true);
    }
}
