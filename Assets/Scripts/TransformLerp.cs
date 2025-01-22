using UnityEngine;
using System.Collections;
using Photon.Pun;

public class TransformLerp : MonoBehaviourPun
{
    public Transform initial;
    public Transform target;
    public float duration = 1.0f;
    public EventTimer eventTimer;

    private bool isLerping = false;
    
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
            // If Master Client, directly call and sync
            photonView.RPC("StartLerp", RpcTarget.All, target.position, target.rotation, target.localScale);
        }
        else
        {
            // If not Master Client, request Master to execute the movement
            photonView.RPC("RequestMoveToTarget", RpcTarget.MasterClient);
        }
    }

    public void MoveToInitial()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            photonView.RPC("StartLerp", RpcTarget.All, initial.position, initial.rotation, initial.localScale);
        }
        else
        {
            photonView.RPC("RequestMoveToInitial", RpcTarget.MasterClient);
        }
    }

    [PunRPC]
    private void RequestMoveToTarget()
    {
        if (PhotonNetwork.IsMasterClient) // Ensure only Master executes
        {
            MoveToTarget();
        }
    }

    [PunRPC]
    private void RequestMoveToInitial()
    {
        if (PhotonNetwork.IsMasterClient) // Ensure only Master executes
        {
            MoveToInitial();
        }
    }

    [PunRPC]
    private void StartLerp(Vector3 endPosition, Quaternion endRotation, Vector3 endScale)
    {
        if (!isLerping)
        {
            StartCoroutine(LerpTransformation(endPosition, endRotation, endScale, duration));
        }
    }

    private IEnumerator LerpTransformation(Vector3 endPosition, Quaternion endRotation, Vector3 endScale, float time)
    {
        isLerping = true;

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
        isLerping = false;
    }

}

