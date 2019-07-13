using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;
using VRTK.Controllables.ArtificialBased;
using VRTK.Examples;

public class Rail : Photon.MonoBehaviour {
    static private int lastRecalculation = -1;
    private List<Transform> nodes;
    private List<ControllableReactor> motors;
    public int nodesLength = 0;
    public int motorsLength = 0;

    public float currentSpeed = 0;
    public float previousSpeed = 0;

	// Use this for initialization
	void Start () {
        nodes = transform.Find("Nodes").Cast<Transform>().ToList();
        nodesLength = nodes.Count;

        motors = transform.Find("Motors").GetComponentsInChildren<ControllableReactor>().ToList(); ;
        motorsLength = motors.Count;
       
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
        if (lastRecalculation == Time.frameCount || PhotonNetwork.isMasterClient)
            return;

        float previousSpeed = currentSpeed;
        float calculatedSpeed = 0;
        foreach (ControllableReactor motor in motors)
        {
            calculatedSpeed += motor.rotationSpeed;
        }
        currentSpeed = calculatedSpeed;
        /*if (Time.frameCount % 20 == 0)
        {
            //photonView.RPC("NetSetcurrentSpeed", PhotonTargets.All, calculatedSpeed);

        }*/
        
    }

    [PunRPC]
    void NetSetcurrentSpeed(float netCalculatedSpeed)
    {
        currentSpeed = netCalculatedSpeed;
    }

    public Vector3 LinearPosition(int nodeFrom, int nodeTo, float ratio)
    {
        Vector3 p1 = nodes[nodeFrom].position;
        Vector3 p2 = nodes[nodeTo].position;

        return Vector3.Lerp(p1, p2, ratio);
    }

    public Quaternion Orientation(int nodeFrom, int nodeTo, float ratio)
    {
        Quaternion q1 = nodes[nodeFrom].rotation;
        Quaternion q2 = nodes[nodeTo].rotation;

        return Quaternion.Lerp(q1, q2, ratio);
    }

    private void OnDrawGizmos()
    {
       /* for(int i = 0; i < nodesLength - 1; i++)
        {
            Handles.DrawDottedLine(nodes[i].position, nodes[i + 1].position, 3.0f);
        }
        Handles.DrawDottedLine(nodes[0].position, nodes[nodesLength-1].position, 3.0f);*/
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting && previousSpeed != currentSpeed)
        {
            float photonSpeed = currentSpeed;
            print("Send speed : " + photonSpeed);
            stream.Serialize(ref photonSpeed);
            previousSpeed = currentSpeed;
        }
        else
        {
            float photonSpeed = 0.0f;
            stream.Serialize(ref photonSpeed);
            print("Received speed : " + photonSpeed);
            this.currentSpeed = photonSpeed;
        }
    }
}
