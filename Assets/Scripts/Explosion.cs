using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    [SerializeField] int chipAmount;
    [SerializeField] GameObject chipDebris;
    [SerializeField] GameObject crater;
    [SerializeField] AudioClip explosionSound;
    [SerializeField] CameraShaker shaker;
    [SerializeField] ParticleSystem explosion;
    [SerializeField] LayerMask CollidesWith;
    [SerializeField] ForceMode forceMode;
    [SerializeField] float explosionRadius;
    [SerializeField] float explosionForce;
    [SerializeField] float upwardsModifier;

    public void Explode()
    {
        explosion.Play();
        shaker.ShakeOnce(new PerlinShake(ShakeData.Create(20f, 10f, 1.5f, 5f)));
        crater.SetActive(true);
        AudioManager.Instance.PlayOnce(explosionSound, transform.position);

        for (int i = 0; i < chipAmount; i++) ObjectPooler.Instance.Spawn("Chips", transform.position + Random.insideUnitSphere, Quaternion.identity);

        Collider[] enemiesInRadius = Physics.OverlapSphere(transform.position, explosionRadius, CollidesWith);

        for (int i = 0; i < enemiesInRadius.Length; i++)
        {
            Transform enemy = enemiesInRadius[i].transform;
            Rigidbody rb = enemy.gameObject.GetComponent<Rigidbody>();
            if (rb == null) continue;

            rb.AddExplosionForce(explosionForce, transform.position, explosionRadius * 1.5f, upwardsModifier, forceMode);
            if (!rb.freezeRotation) rb.AddTorque(1.5f * explosionForce * (enemy.position - transform.position), forceMode);
        }
    }
}
