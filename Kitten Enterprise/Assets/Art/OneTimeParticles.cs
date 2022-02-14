using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneTimeParticles : MonoBehaviour
{
    public ParticleSystem ps;

    private void Awake()
    {
        ps.Play();
    }

    private void Update()
    {
        if (ps == null || ps.isStopped) Destroy(gameObject);
    }
}
