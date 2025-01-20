using UnityEngine;
using System.Collections;

public class PositionLerp : MonoBehaviour
{
    public Transform target;
    public float duration = 1.0f;

    public void MoveToTarget()
    {
        StartCoroutine(LerpPosition(target.position, duration));
    }

    private IEnumerator LerpPosition(Vector3 endPosition, float time)
    {
        Vector3 startPosition = transform.position;
        float elapsedTime = 0;

        while (elapsedTime < time)
        {
            transform.position = Vector3.Lerp(startPosition, endPosition, elapsedTime / time);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = endPosition;
    }
}
