using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine;

public class SliderTimer : MonoBehaviour
{
    [SerializeField] Camera cam;
    [SerializeField] ParticleSystem speedLines;
    [SerializeField] AudioClip tickingSound;
    [SerializeField] Image fill;
    [SerializeField] float timerSpeed = 0f;
    [SerializeField] Slider slider;
    [SerializeField] Gradient colorGradient;
    [SerializeField] UnityEvent onTimerComplete;

    bool resetting = false;

    float lastSliderValue = 0f;
    AudioSource tickingSource;

    void Start()
    {
        onTimerComplete.AddListener(Finish);
        tickingSource = AudioManager.Instance.PlayOnce(tickingSound, transform.position);
    }
    void Update()
    {
        if (resetting) slider.value = Mathf.Max(0f, slider.value + Time.deltaTime * 5f);
        else slider.value = Mathf.Min(1f, slider.value + Time.deltaTime * timerSpeed * 0.1f);


        tickingSource.pitch = Mathf.Lerp(0.5f, 1f, EaseIn(slider.value));
        cam.fieldOfView = Mathf.Lerp(95f, 75f, EaseIn(slider.value));

        ParticleSystem.EmissionModule em = speedLines.emission;
        em.rateOverTime = Mathf.Lerp(0f, 400f, EaseIn(slider.value));

        ParticleSystem.VelocityOverLifetimeModule velOverLife = speedLines.velocityOverLifetime;
        velOverLife.speedModifier = Mathf.Lerp(0.5f, 3f, EaseIn(slider.value));

        if (slider.value >= 1f && lastSliderValue < 1f) onTimerComplete?.Invoke();

        fill.color = colorGradient.Evaluate(slider.value);
        lastSliderValue = slider.value;
    }

    public void Reset()
    {
        
    }

    float EaseIn(float t)
    {
        return t * t;
    }

    void Finish()
    {
        tickingSource.Stop();
    }
}
