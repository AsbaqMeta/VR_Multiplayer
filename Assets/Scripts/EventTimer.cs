using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Photon.Pun;


public class EventTimer : MonoBehaviourPun
{
    // Duration of the timer in seconds
    public float timerDuration = 150f;
    // Current time left on the timer
    public float currentTime;
    // Flag to indicate whether the timer is running
    private bool isRunning = false;
    // UI Text elements to display the timer
    public Text Timer_1;
    public Text Timer_2;

    // Method to start the timer (can be called from other scripts)
    public void StartTimer()
    {
        // Ensure the timer doesn't start multiple times
        if (!isRunning)
        {
            // Check if PhotonView is attached to the GameObject
            if (photonView == null)
                Debug.LogError("PhotonView is missing on " + gameObject.name);

            // Set the running flag to true and initiate the RPC
            isRunning = true;
            photonView.RPC("StartTimerRPC", RpcTarget.All);
        }
    }

    // RPC method to synchronize the timer across all players in the network
    [PunRPC]
    private void StartTimerRPC()
    {
        // Start the timer on all clients when the RPC is called
        StartCoroutine(TimerCoroutine());
    }

    // Coroutine to manage the countdown logic of the timer
    private IEnumerator TimerCoroutine()
    {
        // Run the timer as long as it is active
        while (isRunning)
        {
            // Loop through the countdown until the timer reaches zero
            for (currentTime = timerDuration; currentTime > 0; currentTime--)
            {
                // Update the UI Text elements with the current time
                Timer_1.text = "Timer: " + currentTime.ToString();
                Timer_2.text = "Timer: " + currentTime.ToString();

                // Wait for 1 second before updating again
                yield return new WaitForSeconds(1f);
            }

            // Once the timer ends, stop it and log the event
            isRunning = false;
            Debug.Log("Timer ended!");
        }
    }
}

