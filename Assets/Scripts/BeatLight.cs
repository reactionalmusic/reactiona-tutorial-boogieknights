using UnityEngine;
using UnityEngine.Rendering.Universal;

public class BeatLight : MonoBehaviour
{
    Light2D light2D;
    public float maxIntensity = 15f;
    public float subDivision = 1f;
   
    void Start()
    {
        light2D = GetComponent<Light2D>();
    }

    
    void Update()
    {
        if (Reactional.Playback.MusicSystem.GetCurrentBeat() < 1f)
            return;
        var currentBeat = Reactional.Playback.MusicSystem.GetCurrentBeat()/subDivision % 1;
        light2D.intensity = (1f-currentBeat) * maxIntensity;
    }
}
