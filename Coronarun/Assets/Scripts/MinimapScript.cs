using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapScript : MonoBehaviour
{
    float turnPrev;
    
    void Start()
    {
        turnPrev = 0f;
        transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
    }

    // Update is called once per frame
    void Update()
    {
        float turn = GameObject.Find("PlayerEmpty").GetComponent<OverallController>().transform.rotation.eulerAngles.y;
        if(Time.deltaTime <= 1)
        {
            turnPrev = Mathf.SmoothStep(turnPrev, turn, 10f * Time.deltaTime);
        } else
        {
            turnPrev = turn;
        }        
        transform.localRotation = Quaternion.Euler(5f + Mathf.Sin(Time.time), 5f + Mathf.Cos(Time.time), turnPrev);
    }
}
