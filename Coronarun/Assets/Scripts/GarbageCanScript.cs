using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GarbageCanScript : MonoBehaviour
{
    float fDist;
    Transform pTransform;

    void Start()
    {
        
    }

    void Update()
    {
    	pTransform = GameObject.Find("PlayerEmpty").GetComponent<Transform>();
        fDist = Mathf.Sqrt((transform.position.x - pTransform.position.x) * (transform.position.x - pTransform.position.x) + (transform.position.z - pTransform.position.z) * (transform.position.z - pTransform.position.z));
        if(pTransform.position.z > transform.position.z + 40f && fDist > 50f)
        {
            Destroy(gameObject);
        } else
        {
        	if(fDist < 15f)
        	{
        		transform.localScale = new Vector3(0.6f, 0.6f, 0.6f);
        	} else if(fDist < 56f)
        	{
        		transform.localScale = new Vector3(0.6f, Mathf.SmoothStep(0.1f, 0.6f, Mathf.InverseLerp(56f, 15f, fDist)), 0.6f);
        	} else
        	{
        		transform.localScale = new Vector3(0.6f, 0.1f, 0.6f);
        	}
        }
    }
}
