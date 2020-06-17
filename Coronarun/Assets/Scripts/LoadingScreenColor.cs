using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingScreenColor : MonoBehaviour
{
    public Color col;
    public int valu;

    void Awake()
    {
    	DontDestroyOnLoad(gameObject);

        if(FindObjectsOfType<LoadingScreenColor>().Length > 1)
        {
            Destroy(gameObject);
            return;
        } else
        {
            valu = 0;
        }
    }

    public void HandleInputData(int val)
    {
    	if(val == 0)
    	{
    		col = new Color(1f, 1f, 1f, 1f);
    	}
    	if(val == 1)
    	{
    		col = new Color(0f, 0f, 0f, 1f);
    	}
        valu = val;
    }
}
