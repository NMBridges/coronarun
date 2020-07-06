using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GarbageCanPartsScript : MonoBehaviour
{
	Rigidbody rig;
	Transform pTransform;
	float fDist;

	void Start()
	{
		rig = GetComponent<Rigidbody>();
		rig.constraints = RigidbodyConstraints.FreezeAll;
	}
    // Update is called once per frame
    void Update()
    {
        if(transform.position.y < 0f)
        {
        	transform.position = new Vector3(transform.position.x, 0f, transform.position.z);
        	rig.velocity = Vector3.zero;
        }
        pTransform = GameObject.Find("PlayerEmpty").GetComponent<Transform>();
        fDist = Mathf.Sqrt((transform.position.x - pTransform.position.x) * (transform.position.x - pTransform.position.x) + (transform.position.z - pTransform.position.z) * (transform.position.z - pTransform.position.z));
        if(fDist < 2f)
        {
			rig.constraints = RigidbodyConstraints.None;
        }
    }
}
