using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class ScoreSystem : MonoBehaviourPun
{
    public float maxTime = 150f; // 2.5 minutes in seconds
    public Text Score;
    public GameObject Score_Panel;
    public GameObject Timer_Panel;

    public void CalculateScore(float completionTime)
    {
        int score = (int)(Mathf.Clamp01(completionTime / maxTime) * 100);
        photonView.RPC("UpdateScore", RpcTarget.All, score);
    }

    [PunRPC]
    private void UpdateScore(int score)
    {
        Debug.Log("Score Updated: " + score);
        Score_Panel.SetActive(true);
        Timer_Panel.SetActive(true);
        Score.text = "Score: " + score.ToString();
    }
}
