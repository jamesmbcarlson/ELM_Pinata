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
        rb.AddForce(new Vector3(Random.Range(-1f, 1f), spawnImpulse, Random.Range(-1f, 1f)));   
    }

    // Update is called once per frame
    void Update()
    {
        //spin! for fun!
        float newY = transform.position.y + rotationSpeed * Time.deltaTime;
        transform.Rotate(0f, newY, 0f, Space.World);

        // deactivate if below map
        if(transform.position.y < 0f)
        {
            transform.position = new Vector3(transform.position.x, 1f, transform.position.z);
        }
    }
}
