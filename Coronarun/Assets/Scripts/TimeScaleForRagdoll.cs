using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeScaleForRagdoll : MonoBehaviour
{

	Rigidbody rig;
	Collider col;
	float lastTimeScale;
    
    void Start()
    {
        rig = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
        lastTimeScale = 1f;
    }

    
    void Update()
    {
        if(col.enabled)
        {
        	rig.velocity *= Time.timeScale / lastTimeScale;
        	if(rig.velocity.magnitude > 5.5f)
        	{
        		rig.velocity = rig.velocity.normalized * 5.5f;
        	}
        	lastTimeScale = Time.timeScale;
        }
    }
}
