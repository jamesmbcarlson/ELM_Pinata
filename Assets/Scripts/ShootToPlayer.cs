using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootToPlayer : MonoBehaviour
{

    private PlayerController playerController;
    [SerializeField] float speed = 50f;
    private bool goodToGO = false;
    void Start()
    {
        playerController = FindObjectOfType<PlayerController>();
        //StartCoroutine(DoSomething(2));
          
    }

    
    void OnEnable()
    {
          StartCoroutine(Homeing());
    }

    void Update()
    {
        GoTime();
    }

    private IEnumerator Homeing()
    {
        yield return new WaitForSeconds(5);
        goodToGO = true;

    }

    private void GoTime()
    {
            if(goodToGO == true){transform.position += (playerController.transform.position - transform.position) * speed * Time.deltaTime;}
        
    }

}
