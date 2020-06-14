using UnityEngine;

public class TimeRemap : MonoBehaviour
{
    public float timeFactor = 0.02f;
    public float slowdownLength = 3f;
    public AudioManager audioManager;
    bool updating;

    void Awake()
    {
    	updating = false;
    }

    void Update()
    {
    	if(updating)
    	{
    		Time.timeScale += (1f / slowdownLength) * Time.unscaledDeltaTime;
    		if(Time.timeScale > 1f)
    		{
    			Time.timeScale = Mathf.Clamp(Time.timeScale, 0f, 1f);
    			updating = false;
    		}
    		audioManager.updatePitch();
    		Time.fixedDeltaTime = 0.02f * Time.timeScale;
		}
    }

    public void SlowMotion()
    {
    	Time.timeScale = timeFactor;
    	Time.fixedDeltaTime = 0.02f * Time.timeScale;
    	updating = true;
    	audioManager.updatePitch();
    }
}
