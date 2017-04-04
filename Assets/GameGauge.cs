using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameGauge : MonoBehaviour
{
    Text text;
    Slider slider;
    void Start()
    {
        text = GetComponentInChildren<Text>();
        slider = GetComponent<Slider>();
    }

    public void SetSliderValue(float value)
    {
        slider.value = value;
        text.text = ((int)value).ToString();
    }

    public int GetSliderValue()
    {
        if (slider == null)
            return 0;

        return (int)slider.value;
    }
}
