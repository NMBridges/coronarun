using UnityEngine;
using UnityEngine.UI;

public class FindLoadingType : MonoBehaviour
{
	public TMPro.TMP_Dropdown dd;
	int valuu;
	void Awake()
	{
		if(FindObjectsOfType<LoadingScreenColor>() != null)
		{
			valuu = FindObjectsOfType<LoadingScreenColor>()[0].gameObject.GetComponent<LoadingScreenColor>().valu;
			dd.value = valuu;
		} else
		{
			dd.value = 0;
		}
	}

    public void findObject(int val)
    {
    	FindObjectsOfType<LoadingScreenColor>()[0].HandleInputData(val);
    }
}
