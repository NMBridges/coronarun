using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadBlockScript : MonoBehaviour
{
    
    Transform pTransform;
    GameObject thisObj;

    void Start()
    {
        transform.localScale = new Vector3(0.6f, 0.6f, 0.6f);
        pTransform = GameObject.Find("PlayerEmpty").GetComponent<Transform>();
        thisObj = gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        pTransform = GameObject.Find("PlayerEmpty").GetComponent<Transform>();
        finalizeRoadblock();
    }

    void finalizeRoadblock()
    {
    	float fDist = Mathf.Sqrt((transform.position.x - pTransform.position.x) * (transform.position.x - pTransform.position.x) + (transform.position.z - pTransform.position.z) * (transform.position.z - pTransform.position.z));
		if(fDist < 56f)
		{
			if(fDist < 15f)
			{
				transform.localScale = new Vector3(0.6f, 0.6f, 0.6f);

			} else
			{
				transform.localScale = new Vector3(0.6f, Mathf.SmoothStep(0f, 0.6f, (Mathf.InverseLerp(50f, 15f, fDist))), 0.6f);
			}
		} else
		{
			transform.localScale = new Vector3(0f, 0f, 0f);
		}
        if(pTransform.position.z > transform.position.z + 40f && fDist > 50f)
        {
            Destroy(gameObject);
        }
    }
}
