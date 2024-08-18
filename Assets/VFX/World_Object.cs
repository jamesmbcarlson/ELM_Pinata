using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class World_Object : MonoBehaviour
{
    [SerializeField] int health = 1;
    [SerializeField] AudioClip onHitAudio;
    [SerializeField] AudioClip onDeathAudio;
    [SerializeField] GameObject candy;

    [SerializeField] GameObject onDeathVFX;

    Vector3 startLocation;
    void Start()
    {
        StartCoroutine(SelfDestructObjectandSpawnVFX());
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator SelfDestructObjectandSpawnVFX()
    {
    yield return new WaitForSeconds(5f);
        gameObject.transform.GetChild(0).gameObject.SetActive(false);
        gameObject.transform.GetChild(1).gameObject.SetActive(true);
        
        startLocation = gameObject.transform.localPosition;
        int i = Random.Range(2, 6);
        for(int x = i ; x>0; x--){Instantiate(candy, startLocation, Quaternion.identity); }
        
        StartCoroutine(SelfDestruct());

    }

     IEnumerator SelfDestruct()
    {
    yield return new WaitForSeconds(5f);
        Destroy(gameObject);
    }
}
