using UnityEngine;
using System.Collections;

public class EventTimer : MonoBehaviour
{
    public float timerDuration = 5.0f;
    private float currentTime;
    private bool isRunning = false;

    public void StartTimer()
    {
        if (!isRunning)
        {
            isRunning = true;
            StartCoroutine(TimerCoroutine());
        }
    }

    private IEnumerator TimerCoroutine()
    {
        for (currentTime = timerDuration; currentTime > 0; currentTime--)
        {
            yield return new WaitForSeconds(1f);
        }
        isRunning = false;
        Debug.Log("Timer ended!");
    }
}
