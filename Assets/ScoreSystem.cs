using UnityEngine;

public class ScoreSystem : MonoBehaviour
{
    public float maxTime = 150f; // 2.5 minutes in seconds
    private float score;

/*    private void Start()
    {
        Debug.Log(CalculateScore(10));
    }*/

    public float CalculateScore(float completionTime)
    {
        int score = (int)(Mathf.Clamp01((completionTime) / (maxTime)) * 100);
        Debug.Log(score);
        return score;
    }
}
