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
    [SerializeField] UnityEvent onTimerReset;
    [SerializeField] Vector2 fovChange;
    [SerializeField] int turn = 0;
    public int Turn => turn;
    public bool GameEnded => gameEnded;

    bool gameEnded = false;
    bool resetting = false;
    float resetSpeedRatio = 1;
    float timerSpeedMulti = 1f;
    float lastSliderValue = 0f;
    AudioSource tickingSource;

    void Start()
    {
        onTimerComplete.AddListener(Finish);
        onTimerReset.AddListener(DisableReset);

        tickingSource = AudioManager.Instance.PlayOnce(tickingSound, transform.position);
    }
    void Update()
    {
        if (resetting) slider.value = Mathf.Max(0f, slider.value - Time.deltaTime * resetSpeedRatio * 0.75f * (gameEnded ? 3f : 1f));
        else if (!gameEnded) slider.value = Mathf.Min(1f, slider.value + Time.deltaTime * timerSpeed * timerSpeedMulti * 0.1f);

        tickingSource.pitch = Mathf.Lerp(0.5f, 1f, EaseIn(slider.value));
        cam.fieldOfView = Mathf.Lerp(fovChange.x, fovChange.y, EaseIn(slider.value));

        ParticleSystem.EmissionModule em = speedLines.emission;
        em.rateOverTime = Mathf.Lerp(0f, 400f, EaseIn(slider.value));

        ParticleSystem.VelocityOverLifetimeModule velOverLife = speedLines.velocityOverLifetime;
        velOverLife.speedModifier = Mathf.Lerp(0.5f, 3f, EaseIn(slider.value));

        if (slider.value >= 1f && lastSliderValue < 1f)
        {
            onTimerComplete?.Invoke();
            resetSpeedRatio *= 0.25f;
        }
        if (slider.value <= 0f && lastSliderValue > 0f) onTimerReset?.Invoke();

        fill.color = colorGradient.Evaluate(slider.value);
        lastSliderValue = slider.value;
    }

    void DisableReset()
    {
        if (gameEnded) return;

        speedLines.Play();
        tickingSource.Play();
        resetting = false;
    }

    float EaseIn(float t)
    {
        return t * t;
    }

    public void Finish()
    {
        if (resetting) return;

        resetting = true;
        tickingSource.Stop();
        speedLines.Stop();
        turn++;
        timerSpeedMulti = 1f + (turn * 0.1f);
        resetSpeedRatio = slider.value;
    }
    public void EndGame()
    {
        gameEnded = true;
    }
}
