using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class BashableObject : MonoBehaviour
{
    [Header("Stats")]
    public float healthPoints = 1;
    public float armorClass = 0.4f; // this is set to the initial scale of the Pinata; if the pinata model changes,
                                    // I will need to update this, so I maybe need to actually get the different damage
                                    // thresholds from a script on the pinata 

    [Header("VFX")]
    public float minSmoke = 1;
    public float maxSmoke = 2;
    private PoolManager smokeManager;



    private void Awake()
    {
        smokeManager = GameObject.Find("ManagerSmoke").GetComponent<PoolManager>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "bat")
        {
            healthPoints -= 1;
            if (healthPoints <= 0)
            {
                // grab smoke from pool
                GameObject smokeEffect = smokeManager.GetObject();
                smokeEffect.transform.position = transform.position;
                VisualEffect vfx = smokeEffect.GetComponent<VisualEffect>();
                vfx.SetFloat("MinRandomSize", minSmoke);
                vfx.SetFloat("MaxRandomSize", maxSmoke);

                smokeEffect.SetActive(true);
                
            }
        }
    }
}
