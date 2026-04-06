using UnityEngine;

public class FireFlyController : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        
        // add firefly to the score
        GameManager.Instance.AddFireFly();
        
        // destroy the firefly 
        Destroy(gameObject);
    }
}
