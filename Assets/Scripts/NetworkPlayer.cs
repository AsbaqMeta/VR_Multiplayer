using UnityEngine;
using UnityEngine.XR;
using Photon.Pun;
using Unity.XR.CoreUtils;

public class NetworkPlayer : MonoBehaviourPun, IPunObservable
{
    public Transform Head;
    public Transform LeftHand;
    public Transform RightHand;

    private new PhotonView photonView;

    public Animator LeftHandAnimator;
    public Animator RightHandAnimator;

    private Transform HeadRig;
    private Transform LeftHandRig;
    private Transform RightHandRig;

    private Vector3 headPosition;
    private Quaternion headRotation;
    private Vector3 leftHandPosition;
    private Quaternion leftHandRotation;
    private Vector3 rightHandPosition;
    private Quaternion rightHandRotation;

    public float smoothTime = 0.1f; // Interpolation factor
    private Vector3 headOffset; // Stores initial head offset

    void Start()
    {
        photonView = GetComponent<PhotonView>();
        XROrigin rig = FindObjectOfType<XROrigin>();

        HeadRig = rig.transform.Find("Camera Offset/Main Camera");
        LeftHandRig = rig.transform.Find("Camera Offset/Left Controller");
        RightHandRig = rig.transform.Find("Camera Offset/Right Controller");

        // Store initial offset (fixes head sinking issue)
        headOffset = Head.position - HeadRig.position;

        // Hide local player’s model to prevent self-view
        if (photonView.IsMine)
        {
            foreach (var item in GetComponentsInChildren<Renderer>())
            {
                item.enabled = false;
            }
        }
    }

    void Update()
    {
        if (photonView.IsMine)
        {
            // Sync local movement
            MapPosition(Head, HeadRig, headOffset);
            MapPosition(LeftHand, LeftHandRig);
            MapPosition(RightHand, RightHandRig);

            // Sync hand animations
            UpdateHandAnimation(InputDevices.GetDeviceAtXRNode(XRNode.LeftHand), LeftHandAnimator);
            UpdateHandAnimation(InputDevices.GetDeviceAtXRNode(XRNode.RightHand), RightHandAnimator);
        }
        else
        {
            // Interpolate movement for other players
            Head.position = Vector3.Lerp(Head.position, headPosition, smoothTime);
            Head.rotation = Quaternion.Slerp(Head.rotation, headRotation, smoothTime);

            LeftHand.position = Vector3.Lerp(LeftHand.position, leftHandPosition, smoothTime);
            LeftHand.rotation = Quaternion.Slerp(LeftHand.rotation, leftHandRotation, smoothTime);

            RightHand.position = Vector3.Lerp(RightHand.position, rightHandPosition, smoothTime);
            RightHand.rotation = Quaternion.Slerp(RightHand.rotation, rightHandRotation, smoothTime);
        }
    }

    void UpdateHandAnimation(InputDevice targetDevice, Animator handAnimator)
    {
        if (handAnimator == null) return;

        if (targetDevice.TryGetFeatureValue(CommonUsages.trigger, out float triggerValue))
        {
            handAnimator.SetFloat("Trigger", triggerValue);
        }
        else
        {
            handAnimator.SetFloat("Trigger", 0);
        }

        if (targetDevice.TryGetFeatureValue(CommonUsages.grip, out float gripValue))
        {
            handAnimator.SetFloat("Grip", gripValue);
        }
        else
        {
            handAnimator.SetFloat("Grip", 0);
        }
    }

    void MapPosition(Transform target, Transform rigTransform, Vector3 offset = default)
    {
        target.position = rigTransform.position + offset;
        target.rotation = rigTransform.rotation;
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting) // Sending data
        {
            stream.SendNext(Head.position);
            stream.SendNext(Head.rotation);
            stream.SendNext(LeftHand.position);
            stream.SendNext(LeftHand.rotation);
            stream.SendNext(RightHand.position);
            stream.SendNext(RightHand.rotation);
        }
        else // Receiving data
        {
            headPosition = (Vector3)stream.ReceiveNext();
            headRotation = (Quaternion)stream.ReceiveNext();
            leftHandPosition = (Vector3)stream.ReceiveNext();
            leftHandRotation = (Quaternion)stream.ReceiveNext();
            rightHandPosition = (Vector3)stream.ReceiveNext();
            rightHandRotation = (Quaternion)stream.ReceiveNext();
        }
    }
}
