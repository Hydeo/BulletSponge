using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager_BS : Photon.MonoBehaviour {

    private PhotonView photonView = null;
	// Use this for initialization
	void Start () {
        photonView = GetComponent<PhotonView>();
             
	}
	
	// Update is called once per frame
	void Update () {
       
	}
}
