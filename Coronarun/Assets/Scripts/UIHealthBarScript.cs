using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHealthBarScript : MonoBehaviour
{
    int level;
    public Texture image;
    float prevLev;

    void Start()
    {
    	prevLev = 0f;
        level = GameObject.Find("PlayerEmpty").GetComponent<OverallController>().coronaLevel;
        gameObject.GetComponent<RawImage>().material.SetFloat("_Value", (float) level / 15f);
    }

    // Update is called once per frame
    void Update()
    {
        level = GameObject.Find("PlayerEmpty").GetComponent<OverallController>().coronaLevel;
       	level = Mathf.Clamp(level, 0, 15);
       	prevLev += ((float)level - prevLev) * 0.2f;
        gameObject.GetComponent<RawImage>().material.SetFloat("_Value", prevLev / 15f);
    }
}
