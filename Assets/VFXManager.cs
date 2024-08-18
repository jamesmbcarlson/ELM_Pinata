using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class VFXManager : MonoBehaviour
{
    VisualEffect visualEffect;

    private void Awake()
    {
        visualEffect = GetComponent<VisualEffect>();
    }

    // Update is called once per frame
    void Update()
    {
        if (visualEffect.aliveParticleCount == 0)
        {
            gameObject.SetActive(false);
        }
    }
}
