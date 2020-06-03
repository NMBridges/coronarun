using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class minimapcamscript : MonoBehaviour
{
    public Transform player;

    // Start is called before the first frame update
    void Start()
    {
    	transform.position = player.position + new Vector3(0f, 20f, 0f);
        transform.rotation = Quaternion.Euler(90f, 0f, 0f);
    }

    // Update is called once per frame
    void Update()
    {
    	transform.position = player.position + new Vector3(0f, 20f, 0f);
        transform.rotation = Quaternion.Euler(90f, 0f, 0f);
    }
}
