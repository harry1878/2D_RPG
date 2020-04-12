using UnityEngine;

public class Gamemanager : MonoBehaviour
{
    private void Awake()
    {
        Physics2D.IgnoreLayerCollision(8, 9);
    }

}
