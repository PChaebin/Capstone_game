using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthBar : MonoBehaviour
{
    [SerializeField] Slider healthBarSlider;
    public TextMeshProUGUI healthBarValueText;

    public int maxHealth;
    public int currHealth;

    // Start is called before the first frame update
    void Start()
    {
        currHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        healthBarValueText.text = currHealth.ToString() + " / " + maxHealth.ToString();

        healthBarSlider.value = currHealth;
        healthBarSlider.maxValue = maxHealth;
    }
}
