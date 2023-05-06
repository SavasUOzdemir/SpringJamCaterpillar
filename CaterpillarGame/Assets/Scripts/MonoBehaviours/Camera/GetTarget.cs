using UnityEngine;
using Cinemachine;

public class GetTarget : MonoBehaviour
{
    [SerializeField] private CinemachineFreeLook virtualCamera;
    const string PLAYER_TAG = "Player";

    private void Awake()
    {
        if (virtualCamera.Follow == null || virtualCamera.LookAt == null)
        {
            virtualCamera.Follow = GameObject.FindGameObjectWithTag(PLAYER_TAG).transform;
            virtualCamera.LookAt = GameObject.FindGameObjectWithTag(PLAYER_TAG).transform;
        }
    }
}