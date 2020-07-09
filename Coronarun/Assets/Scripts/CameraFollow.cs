using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    
    public Transform player;
    public float cancelMovement;
    Vector3 rotH;
    Vector3 rotHH;
    public float pDir;
    int tranState;
    float tranCount;
    Vector3 follow;
    float magnitude;
    float lastTime;
    Vector3 newPoint;
    float period;

    void Start()
    {
        transform.position = player.position + new Vector3(0f, 1.5f, -5.5f);
        tranState = 0;
        magnitude = 0.1f;
        lastTime = Time.unscaledTime;
        newPoint = Vector3.zero;
        period = 0.08f;
    }

    void Update()
    {
        if(Time.unscaledTime - lastTime > period)
        {
            newPoint = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0f) * magnitude;
            lastTime = Time.unscaledTime;
        }
        follow = new Vector3(follow.x + (newPoint.x - follow.x) * 10f * Time.unscaledDeltaTime, follow.y + (newPoint.y - follow.y) * 10f * Time.unscaledDeltaTime, follow.z + (newPoint.z - follow.z) * 10f * Time.unscaledDeltaTime);

        if(!GameObject.Find("PlayerEmpty").GetComponent<OverallController>().playerIsMoving)
        {
            if(tranState == 0)
            {
                tranCount = 0f;
                tranState = 1;
                magnitude = 2f;
                period = 0.006f;
            }
            if(tranState == 1)
            {
                tranCount += 1f * Time.deltaTime;
                magnitude -= 4f * Time.unscaledDeltaTime;
                period += 0.05f * Time.unscaledDeltaTime;
                if(tranCount >= 1f)
                {
                    tranCount = 1f;
                    tranState = 2;
                }
                if(magnitude < 0f)
                {
                    magnitude = 0f;
                }
                if(period > 0.2f)
                {
                    period = 0.2f;
                }
                transform.position = player.position + new Vector3(0f, Mathf.SmoothStep(1.5f, 10f, tranCount), -5.5f) + follow;
                transform.LookAt(player, Vector3.up);
            }
            if(tranState == 2)
            {
                transform.position = player.position + new Vector3(0f, 10f, -5.5f) + follow;
                transform.LookAt(player, Vector3.up);
            }
        } else
        {
            transform.localPosition = new Vector3(0f, 2.6f, -5.5f) + follow;
        }
    }
}
