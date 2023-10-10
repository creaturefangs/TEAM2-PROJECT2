using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Slider healthBar;
    [SerializeField] private Image background;
    public void UpdateHealthBar(float currentHealth)
    {
        healthBar.value = currentHealth;
    }

    public IEnumerator FlashHealthBarGreen(float timeBetweenFlashes, int numberOfFlashes)
    {
        Color originalHealthBarColor = background.color;

        for (int i = 0; i < numberOfFlashes; i++)
        {
            background.color = Color.green;
            yield return new WaitForSeconds(timeBetweenFlashes);
            background.color = originalHealthBarColor;
            yield return new WaitForSeconds(timeBetweenFlashes);
        }
        background.color = originalHealthBarColor;  // Reset the healthbar color
    }
}
