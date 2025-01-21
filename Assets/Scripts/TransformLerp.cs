using UnityEngine;
using System.Collections;
using Photon.Pun;

public class TransformLerp : MonoBehaviourPun
{
    public Transform initial;
    public Transform target;
    public float duration = 1.0f;
    public EventTimer eventTimer;

    private void RPCMoveToTarget()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            MoveToTarget();
        }
    }

    public void MoveToTarget()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            photonView.RPC("LerpToTarget", RpcTarget.All, target.position, target.rotation, target.localScale);
            Debug.Log("MoveToTarget");
            if (eventTimer != null)
            {
                eventTimer.StartTimer();
            }
        }
        
       /* LerpToInitial(target.position, target.rotation, target.localScale);*/
    }

    public void MoveToInitial()
    {
        /*        if (PhotonNetwork.IsMasterClient)
                    photonView.RPC("LerpToInitial", RpcTarget.All, initial.position, initial.rotation, initial.localScale);*/
        Debug.Log("MoveToInitail");
        LerpToInitial(initial.position, initial.rotation, initial.localScale);

    }

    [PunRPC]
    private void LerpToTarget(Vector3 endPosition, Quaternion endRotation, Vector3 endScale)
    {
        StartCoroutine(LerpTransformation(endPosition, endRotation, endScale, duration));
    }

    [PunRPC]
    private void LerpToInitial(Vector3 endPosition, Quaternion endRotation, Vector3 endScale)
    {
        StartCoroutine(LerpTransformation(endPosition, endRotation, endScale, duration));
    }

    private IEnumerator LerpTransformation(Vector3 endPosition, Quaternion endRotation, Vector3 endScale, float time)
    {
        Vector3 startPosition = transform.position;
        Quaternion startRotation = transform.rotation;
        Vector3 startScale = transform.localScale;
        float elapsedTime = 0;

        while (elapsedTime < time)
        {
            float t = elapsedTime / time;
            transform.position = Vector3.Lerp(startPosition, endPosition, t);
            transform.rotation = Quaternion.Lerp(startRotation, endRotation, t);
            transform.localScale = Vector3.Lerp(startScale, endScale, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = endPosition;
        transform.rotation = endRotation;
        transform.localScale = endScale;
    }

}

