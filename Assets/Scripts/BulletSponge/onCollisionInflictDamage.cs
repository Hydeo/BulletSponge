using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ViridaxGameStudios.Controllers;

public class onCollisionInflictDamage : MonoBehaviour {

    public int amountDamage = 0;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

    }

    private void OnCollisionEnter(Collision collision)
    {
       if(collision.gameObject.tag == "Enemy")
        {
            collision.gameObject.GetComponent<AIController>().ReceiveDamage(amountDamage);
        } 
    }
    
}
