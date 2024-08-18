using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Candy : MonoBehaviour
{
    private Rigidbody rb;
    public float rotationSpeed = 1.0f;
    public float spawnImpulse = 10f;

    // Start is called before the first frame update
    void Start()
    {
        transform.rotation = Quaternion.Euler(-90f, Random.Range(0f, 360f), 0f);
        rb = GetComponent<Rigidbody>();
        rb.AddForce(new Vector3(Random.Range(-1f, 1f), spawnImpulse, Random.Range(-1f, 1f)), ForceMode.Impulse);   
    }

    // Update is called once per frame
    void Update()
    {
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
        }
    }
}
