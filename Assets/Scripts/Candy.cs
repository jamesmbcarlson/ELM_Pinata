using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Candy : MonoBehaviour
{
    private PlayerController playerController;
    private Rigidbody rb;
    private CapsuleCollider capsuleCollider;
    public float rotationSpeed = 1.0f;
    public float spawnImpulse = 10f;
    private float lastY;

    void Awake()
    {
        transform.rotation = Quaternion.Euler(-90f, Random.Range(0f, 360f), 0f);
        rb = GetComponent<Rigidbody>();
        capsuleCollider = GetComponent<CapsuleCollider>();
    }

    private void OnEnable()
    {
        rb.AddForce(new Vector3(Random.Range(-1f, 1f), spawnImpulse, Random.Range(-1f, 1f)), ForceMode.Impulse);   
        capsuleCollider.enabled = false;
        lastY = transform.position.y;
    }

    void Update()
    {
        // turn on collider on after apex of jump
        if(!capsuleCollider.enabled)
        {
            if(transform.position.y < lastY)
            {
                capsuleCollider.enabled = true;
            }
            lastY = transform.position.y;
        }
        
        //spin! for fun!
        transform.Rotate(0f, rotationSpeed * Time.deltaTime, 0f, Space.World);

        // deactivate if below map
        if(transform.position.y < -1f)
        {
            transform.position = new Vector3(transform.position.x, 1f, transform.position.z);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.layer != LayerMask.NameToLayer("Player") && !collision.gameObject.GetComponent<Candy>())
        {
            rb.constraints = RigidbodyConstraints.FreezeAll;
            rb.isKinematic = true;
            capsuleCollider.isTrigger = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        CollectCandy(other);
    }

    private void OnTriggerExit(Collider other)
    {
        CollectCandy(other);
    }

    private void OnTriggerStay(Collider other)
    {
        CollectCandy(other);
    }

    private void CollectCandy(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            FindObjectOfType<PlayerController>().GrowPinata();
            rb.constraints = RigidbodyConstraints.None;
            rb.constraints = RigidbodyConstraints.FreezeRotation;
            rb.isKinematic = false;
            capsuleCollider.isTrigger = false;
            gameObject.SetActive(false);

        }
    }
}
