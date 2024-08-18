using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class BashableObject : MonoBehaviour
{
    [Header("Stats")]
    public float healthPoints = 1;
    public float armorClass = 1; // this is set to the initial scale of the Pinata; if the pinata model changes,
                                    // I will need to update this, so I maybe need to actually get the different damage
                                    // thresholds from a script on the pinata 

    private PlayerController playerController;

    [Header("VFX")]
    public float minSmoke = 1;
    public float maxSmoke = 2;
    private PoolManager smokeManager;

    [Header("Candy!")]
    public int minCandy = 1;
    public int maxCandy = 3;
    private PoolManager candyManager;



    private void Awake()
    {
        smokeManager = GameObject.Find("ManagerSmoke").GetComponent<PoolManager>();
        candyManager = GameObject.Find("ManagerCandy").GetComponent<PoolManager>();
        playerController = FindObjectOfType<PlayerController>();
        armorClass *= playerController.initialScale;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "bat" && playerController.isSwinging && playerController.gameObject.transform.localScale.x >= armorClass)
        {
            healthPoints -= playerController.damage;
            if (healthPoints <= 0)
            {
                // grab smoke from pool
                GameObject smokeEffect = smokeManager.GetObject();
                smokeEffect.transform.position = transform.position;
                VisualEffect vfx = smokeEffect.GetComponent<VisualEffect>();
                vfx.SetFloat("MinRandomSize", minSmoke);
                vfx.SetFloat("MaxRandomSize", maxSmoke);
                smokeEffect.SetActive(true);

                // TO-DO: spawn candy!
                int candyAmount = Random.Range(minCandy, maxCandy + 1);
                for(int i = 0; i < candyAmount; i++)
                {
                    GameObject candyObject = candyManager.GetObject();
                    candyObject.transform.position = transform.position;
                    candyObject.SetActive(true);
                }
                

                Destroy(gameObject);
                
            }
        }
    }
}
