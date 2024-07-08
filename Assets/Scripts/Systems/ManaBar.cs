using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ManaBar : MonoBehaviour
{
    [SerializeField] Slider manaBarSlider;
    public TextMeshProUGUI manaBarValueText;

    public int maxMana;
    public int currMana;

    // Start is called before the first frame update
    void Start()
    {
        currMana = maxMana;
    }

    // Update is called once per frame
    void Update()
    {
        manaBarValueText.text = currMana.ToString() + " / " + maxMana.ToString();

        manaBarSlider.value = currMana;
        manaBarSlider.maxValue = maxMana;
    }   
}
