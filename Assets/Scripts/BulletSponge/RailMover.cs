using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RailMover : MonoBehaviour {

    public Rail rail;
    private int nodeFrom;
    private int nodeTo;
    private float transition;
    public float speed;
    public int owner;

    // Use this for initialization
    void Start () {
        nodeFrom = 0;
        nodeTo = 1;
        speed = 2;
        owner = GetComponent<PhotonView>().ownerId;
    }
	
	// Update is called once per frame
	void Update () {
       
        if (!rail)
        {
            return;
        }

        if (PhotonNetwork.isMasterClient)
        {
            speed = 0.05f + rail.currentSpeed;   
            Play();
        }
        
	}

    private void Play()
    {
        if(speed > 1)
        {
            speed = 1;
        }
        else if(speed < 0)
        {
            speed = 0;
        }

        transition +=  0.01f * speed / 1f;

       
        if(transition > 1)
        {
            transition = 0;
            if (nodeFrom + 1 >= rail.nodesLength)
            {
                nodeFrom = 0;
            }
            else
            {
                nodeFrom++;
            }

            if (nodeTo + 1 >= rail.nodesLength)
            {
                nodeTo = 0;
            }
            else
            {
                nodeTo = nodeFrom + 1;
            }
        }
        /*else if(transition < 0)
        {
            transition = 1;
            nodeFrom--;
        }*/


        //rb.MovePosition(rail.LinearPosition(nodeFrom, nodeTo, transition));
        transform.position = rail.LinearPosition(nodeFrom, nodeTo, transition);
        transform.rotation = rail.Orientation(nodeFrom, nodeTo, transition);

    }
}
