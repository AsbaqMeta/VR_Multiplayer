using UnityEngine.SceneManagement;
using UnityEngine;
using Photon.Pun;

public class memory : MonoBehaviour
{

   public void SceneLoad()
    {
       // SceneManager.LoadSceneAsync(1, LoadSceneMode.Single);
        PhotonNetwork.LoadLevel(1);
    }

}
