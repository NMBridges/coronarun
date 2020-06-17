using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MMCamFollow : MonoBehaviour
{
    void Update()
    {
    	float x = Mathf.Clamp((Input.mousePosition.y - Screen.height / 2f) / 50f, -7f, 7f);
    	float y = Mathf.Clamp((Input.mousePosition.x - Screen.width / 2f) / 50f, -12f, 12f);
        transform.rotation = Quaternion.Euler(-x - 8f, y + 8f, 0f);
    }
}
