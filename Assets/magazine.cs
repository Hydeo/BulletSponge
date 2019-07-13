using PlayoVR;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class magazine : MonoBehaviour {
    public int ammoQuantity = 10;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<Gun>())
        {
            print("is gun");
            print(collision.gameObject.GetComponent<Gun>().gunType);
            collision.gameObject.GetComponent<Gun>().NewMagazine(ammoQuantity);
            PhotonDestroy();

        }
        else
        {
            print("not gun");
        }
    }

    public void PhotonDestroy()
    {
        PhotonNetwork.Destroy(GetComponent<PhotonView>());
    }
}
