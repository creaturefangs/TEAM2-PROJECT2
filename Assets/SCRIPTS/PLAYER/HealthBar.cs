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

    public IEnumerator FlashHealthBarGreen(float timeBetweenFlashes)
    {
        Color originalHealthBarColor = background.GetComponent<Image>().material.color;

        background.color = Color.green;
        yield return new WaitForSeconds(timeBetweenFlashes);
        background.color = Color.red;
        yield return new WaitForSeconds(timeBetweenFlashes);
        background.color = Color.green;
        yield return new WaitForSeconds(timeBetweenFlashes);
        background.color = Color.red;
    }
}
