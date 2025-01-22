using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class ScoreSystem : MonoBehaviourPun
{
    // Maximum time allowed for the event (2.5 minutes)
    public float maxTime = 150f; // 2.5 minutes in seconds
    // UI Text element to display the score
    public Text Score;
    // UI Panels for the score and timer
    public GameObject Score_Panel;
    public GameObject Timer_Panel;
    // Reference to the EventTimer script
    public EventTimer eventTimer;


    // Method to calculate the score based on the event's completion time
    public void CalculateScore()
    {
        // Get the time taken to complete the event
        float completionTime = eventTimer.currentTime;

        // Calculate the score as a percentage of maxTime (normalized to a 0-100 range)
        // Clamps the completion time to a minimum of 0, then divides by maxTime and scales to 100
        int score = (int)(Mathf.Clamp01(completionTime / maxTime) * 100);

        // Call the RPC to update the score across all clients
        photonView.RPC("UpdateScore", RpcTarget.All, score);
    }

    // RPC method to update the score on all clients
    [PunRPC]
    private void UpdateScore(int score)
    {
        // Log the updated score for debugging purposes
        Debug.Log("Score Updated: " + score);

        // Ensure the Score and Timer panels are active to display the updated score
        Score_Panel.SetActive(true);
        Timer_Panel.SetActive(true);

        // Update the Score text to show the calculated score
        Score.text = "Score: " + score.ToString();
    }
}

