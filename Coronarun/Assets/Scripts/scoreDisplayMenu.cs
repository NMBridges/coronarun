using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scoreDisplayMenu : MonoBehaviour
{

    void Start()
    {
        if(GameObject.Find("EventSystem").GetComponent<TransitionManager>().lastScore == 0)
        {
        	GetComponent<TMPro.TextMeshProUGUI>().text = null;
        } else
        {
        	if(transform.gameObject.name == "LastScoreScore")
        	{
        		GetComponent<TMPro.TextMeshProUGUI>().text = "" + GameObject.Find("EventSystem").GetComponent<TransitionManager>().lastScore;
        	}
        	if(transform.gameObject.name == "BestScoreScore")
        	{
        		GetComponent<TMPro.TextMeshProUGUI>().text = "" + GameObject.Find("EventSystem").GetComponent<TransitionManager>().bestScore;
        	}
        }
    }
}
