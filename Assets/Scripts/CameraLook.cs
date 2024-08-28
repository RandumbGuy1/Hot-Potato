using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLook : MonoBehaviour
{
    [SerializeField] Transform pivot;
    [SerializeField] CameraShaker shaker;

    [Header("Look Settings")]
    [SerializeField] [Range(0, 1)] float sensitivity;
    [SerializeField] float smoothSpeed;
    [SerializeField] Vector2 positiveClamp;
    [SerializeField] Vector2 negativeClamp;

    Vector3 rawRotation;
    Vector3 smoothRotation;

    [Header("Sway Settings")]
    [SerializeField] float swayAmplitude;
    [SerializeField] float swayFrequency;

    float scroll = 0f;
    Vector3 swayOffset = Vector3.zero;
    Vector3 noiseOffset = Vector3.zero;

    Vector3 startEulerAngles;

    void Start()
    {
        noiseOffset.x = Random.Range(0f, 1024f);
        noiseOffset.y = Random.Range(0f, 1024f);
        noiseOffset.z = Random.Range(0f, 1024f);

        startEulerAngles = pivot.rotation.eulerAngles;

        LockCursorState(true);
    }

    void Update()
    {
        rawRotation += new Vector3(-Input.GetAxisRaw("Mouse Y"), Input.GetAxisRaw("Mouse X"), 0) * sensitivity;
        rawRotation.x = Mathf.Clamp(rawRotation.x, negativeClamp.x, positiveClamp.x);
        rawRotation.y = Mathf.Clamp(rawRotation.y, negativeClamp.y, positiveClamp.y);

        smoothRotation = Vector3.Slerp(smoothRotation, rawRotation, Time.deltaTime * smoothSpeed);
        swayOffset = Vector3.Slerp(swayOffset, GetSwayOffset(swayAmplitude, swayFrequency), Time.deltaTime * swayAmplitude);

        pivot.rotation = Quaternion.Euler(startEulerAngles + smoothRotation + swayOffset + shaker.Offset);
    }

    public Vector3 GetSwayOffset(float amplitude, float frequency)
    {
        scroll += Time.deltaTime * frequency;
        Vector3 noise = new Vector3(Mathf.PerlinNoise(scroll, noiseOffset.x), Mathf.PerlinNoise(scroll, noiseOffset.y), Mathf.PerlinNoise(scroll, noiseOffset.z)).normalized;
        noise = (noise - Vector3.one * 0.5f) * 2f;
        return noise * amplitude;
    }

    public void LockCursorState(bool locked)
    {
        Cursor.lockState = locked ? CursorLockMode.Locked : CursorLockMode.None;
        Cursor.visible = !locked;
    }
}
