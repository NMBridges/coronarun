using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tempgroundshift : MonoBehaviour
{
    Transform pTransform;
    void Start()
    {
        pTransform = GameObject.Find("PlayerEmpty").GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        pTransform = GameObject.Find("PlayerEmpty").GetComponent<Transform>();
        transform.position = new Vector3(pTransform.position.x, 0f,pTransform.position.z);
    }
}
