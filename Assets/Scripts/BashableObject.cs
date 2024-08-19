using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class BashableObject : MonoBehaviour
{
    [Header("Stats")]
    public float healthPoints = 1;

    private PlayerController playerController;

    [Header("VFX")]
    public float minSmoke = 1;
    public float maxSmoke = 2;
    private PoolManager smokeManager;
    private PoolManager smokeManagerSmall;

    [Header("Candy!")]
    public int minCandy = 1;
    public int maxCandy = 3;
    private PoolManager candyManager;

    private void Awake()
    {
        smokeManager = GameObject.Find("ManagerSmoke").GetComponent<PoolManager>();
        smokeManagerSmall = GameObject.Find("ManagerSmokeSmall").GetComponent<PoolManager>();
        candyManager = GameObject.Find("ManagerCandy").GetComponent<PoolManager>();
        playerController = FindObjectOfType<PlayerController>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "bat" && playerController.isSwinging)
        {
            DoDamage(collision.transform.position);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "GroundCheck")
        {
            DoDamage(other.transform.position);
        }
    }

    private void DoDamage(Vector3 collisionPosition)
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

            // spawn candy!
            int candyAmount = Random.Range(minCandy, maxCandy + 1);
            for (int i = 0; i < candyAmount; i++)
            {
                GameObject candyObject = candyManager.GetObject();
                candyObject.transform.position = new Vector3(transform.position.x, transform.position.y + 1, transform.position.z);
                candyObject.SetActive(true);
            }

            Destroy(gameObject);

        }
        else
        {
            // grab small smoke from pool
            GameObject smokeEffect = smokeManagerSmall.GetObject();
            smokeEffect.transform.position = collisionPosition;
            smokeEffect.SetActive(true);
        }
    }
}
