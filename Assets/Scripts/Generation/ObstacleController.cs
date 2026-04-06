using System;
using UnityEngine;

public class ObstacleController : MonoBehaviour
{
    // type of obstacle 
    public enum ObstacleType
    {
        Absolute, // change lane to avoid
        Jumpable, // jump over it
        Fragile, // charge to destroy it
    }
    
    [Header("Settings")]
    [SerializeField] private ObstacleType _obstacleType;

    private void OnTriggerEnter(Collider other)
    {
        // check if the player hit an obstacle
        if (!other.CompareTag("Player")) return;

        PlayerController player = other.GetComponent<PlayerController>();
        if (player == null) return;

        switch (_obstacleType)
        {
            case ObstacleType.Absolute:
                HandleAbsoluteCollision(player);
                break;
            
            case ObstacleType.Jumpable:
                HandleJumpableCollision(player);
                break;
                
            case ObstacleType.Fragile:
                HandleFragileCollision(player);
                break;
        }
    }

    private void HandleAbsoluteCollision(PlayerController player)
    {
        Debug.Log("player hit an absolute obastacle");
        player.TakeDamage();
    }
    
    private void HandleJumpableCollision(PlayerController player)
    {
        // if the player in jumping, ignore the collision
        if (player.StateMachine.CurrentState == player.JumpingState) return;
        
        Debug.Log(("player hit a jumpable obastacle"));
        player.TakeDamage();
    }
    
    private void HandleFragileCollision(PlayerController player)
    {
        // if the player charging , destroy the obsatcle 
        if (player.StateMachine.CurrentState == player.ChargingState)
        {
            Debug.Log("player destroy a fragile obastacle");
            Destroy(gameObject);
            return;
        }
        
        Debug.Log(("player hit an absolute obastacle"));
        player.TakeDamage();
    }
    
}
