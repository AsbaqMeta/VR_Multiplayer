using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Photon.Pun;


public class EventTimer : MonoBehaviourPun
{
    public float timerDuration = 5.0f;
    private float currentTime;
    private bool isRunning = false;
    public Text Timer_1;
    public Text Timer_2;

    public void StartTimer()
    {
        if (!isRunning)
        {
            isRunning = true;
            photonView.RPC("StartTimerRPC", RpcTarget.All);
        }
    }

    [PunRPC]
    private void StartTimerRPC()
    {
        StartCoroutine(TimerCoroutine());
    }

    private IEnumerator TimerCoroutine()
    {
        for (currentTime = timerDuration; currentTime > 0; currentTime--)
        {
            Timer_1.text = "Timer: " + currentTime.ToString();
            Timer_2.text = "Timer: " + currentTime.ToString();
            yield return new WaitForSeconds(1f);
        }
        isRunning = false;
        Debug.Log("Timer ended!");
    }
}
