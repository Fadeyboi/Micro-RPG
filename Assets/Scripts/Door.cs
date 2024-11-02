using UnityEngine;

public class Door : MonoBehaviour
{
    public Transform doorEndpointPos;

    public void OpenDoor()
    {
        FindAnyObjectByType<Player>().transform.position = doorEndpointPos.position;
    }
}
