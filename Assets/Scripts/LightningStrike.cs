using UnityEngine;
using UnityEngine.Rendering.Universal;
using Reactional.Core;
using System.Collections;
using System.Threading;

public class LightningStrike : MonoBehaviour
{
    int barCount = 0;
    public Light2D light2D;
    public void BarBeat(double offset, int bar, int beat)
    {
        barCount = bar % 4;
        if (barCount == 0 && beat == 1)
        {
            StartCoroutine(Strike());
            GetComponent<AudioSource>().Play();
        }
        
    }

    IEnumerator Strike()
    {
        int repeat = 3;
        for (int i = 0; i < repeat; i++)
        {
            light2D.intensity = 10f;
            yield return new WaitForSeconds(0.05f);
            light2D.intensity = 1f;
            yield return new WaitForSeconds(0.05f);
        }
    }
}
