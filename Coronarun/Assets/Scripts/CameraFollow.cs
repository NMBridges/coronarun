using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    
    public Transform player;
    public float cancelMovement;
    Vector3 rotH;
    float shift;
    Vector3 rotHH;
    public float pDir;
    int tranState;
    float tranCount;

    void Start()
    {
        transform.position = player.position + new Vector3(0f, 1.5f, -5.5f);
        tranState = 0;
        //shift = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        if(!GameObject.Find("PlayerEmpty").GetComponent<OverallController>().playerIsMoving)
        {
            if(tranState == 0)
            {
                tranCount = 0f;
                tranState = 1;
            }
            if(tranState == 1)
            {
                tranCount += 1f * Time.deltaTime;
                if(tranCount >= 1f)
                {
                    tranCount = 1f;
                    tranState = 2;
                }
                transform.position = player.position + new Vector3(0f, Mathf.SmoothStep(1.5f, 10f, tranCount), -5.5f);
                transform.LookAt(player, Vector3.up);
            }
        }
    	//Quaternion rot = Quaternion.Euler(10f, player.localRotation.eulerAngles.y, 0f);
    	//rotH = rot * new Vector3(0f, 3f, -5.5f);
    	//shift += cancelMovement * 0.3f;
    	//rotHH = new Vector3(shift, 0f, 0f);
        //transform.position = Quaternion.Euler(0f, pDir, 0f) * rotHH + rotH + new Vector3(Mathf.SmoothStep(transform.position.x, player.position.x, 0.5f), 0f, Mathf.SmoothStep(transform.position.z, player.position.z, 0.5f));
    	//transform.localRotation = Quaternion.Euler(10f, player.localRotation.eulerAngles.y, 0f);
    }
}
