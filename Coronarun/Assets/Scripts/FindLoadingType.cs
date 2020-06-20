using UnityEngine;
using UnityEngine.UI;

public class FindLoadingType : MonoBehaviour
{
	public TMPro.TMP_Dropdown dd;
	public Slider sl;
	int valuu;
	float volum;


	void Start()
	{
		if(FindObjectsOfType<LoadingScreenColor>() != null)
		{
			valuu = FindObjectsOfType<LoadingScreenColor>()[0].gameObject.GetComponent<LoadingScreenColor>().valu;
			if(dd != null)
			{
				dd.value = valuu;
			}
			volum = FindObjectsOfType<LoadingScreenColor>()[0].gameObject.GetComponent<LoadingScreenColor>().volumee;
			if(sl != null)
			{
				sl.value = volum;
			}
		} else
		{
			if(dd != null)
			{
				dd.value = valuu;
			}
			if(sl != null)
			{
				sl.value = volum;
			}
		}
	}

    public void findObject(int val)
    {
    	FindObjectsOfType<LoadingScreenColor>()[0].HandleInputData(val);
    }

    public void changeVolume(float val)
    {
    	FindObjectsOfType<LoadingScreenColor>()[0].HandleVolumeData(val);
		AudioSource[] sources = GameObject.Find("EventSystem").GetComponents<AudioSource>();
		Sound[] audman = GameObject.Find("EventSystem").GetComponent<AudioManager>().sounds;
		for(int i = 0; i < sources.Length; i++)
		{
			sources[i].volume = val * audman[i].volume;
		}
    }
}
