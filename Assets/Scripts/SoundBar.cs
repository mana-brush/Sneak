using UnityEngine;
using UnityEngine.UI; 

public class SoundBar : MonoBehaviour
{
    public Slider slider;
    
    public void SetSoundLevel(float soundLevel)
    {
        slider.value = soundLevel;
    }
}
