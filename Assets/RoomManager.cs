using UnityEngine;



public class RoomManager : MonoBehaviour 
{
    public void SetState(bool enabled)
    {
        gameObject.SetActive(enabled);
    }
}
