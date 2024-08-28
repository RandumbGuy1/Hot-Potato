using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine;

public class SliderTimer : MonoBehaviour
{
    [SerializeField] Image fill;
    [SerializeField] float timerSpeed = 0f;
    [SerializeField] Slider slider;
    [SerializeField] Gradient colorGradient;
    [SerializeField] UnityEvent timerComplete;

    float lastSliderValue = 0f;
    void Update()
    {
        slider.value = Mathf.Min(1f, slider.value + Time.deltaTime * timerSpeed * 0.1f);

        if (slider.value >= 1f && lastSliderValue < 1f) timerComplete?.Invoke();

        fill.color = colorGradient.Evaluate(slider.value);
        lastSliderValue = slider.value;
    }
}
