using UnityEngine;

[CreateAssetMenu(fileName = "New PlayerStats", menuName = "Stats")]
public class PlayerStats : ScriptableObject
{
    [SerializeField] private float initialSpeed = 4f;
    public float InitialSpeed { get { return initialSpeed; } }

    [SerializeField] private float currentSpeed;
    public float CurrentSpeed { get { return currentSpeed; } set { currentSpeed = value; } }

    [SerializeField] private float playerSpeedWhileRunning = 6f;
    public float PlayerSpeedWhileRunning { get { return playerSpeedWhileRunning; } }

    [SerializeField] private float playerSpeedWhileIdleRolling = 3f;
    public float PlayerSpeedWhileIdleRolling { get { return playerSpeedWhileIdleRolling; } }

    [SerializeField] private float playerSpeedWhileWalkRolling = 6f;
    public float PlayerSpeedWhileWalkRolling { get { return playerSpeedWhileWalkRolling; } }

    [SerializeField] private float playerSpeedWhileRunRolling = 8f;
    public float PlayerSpeedWhileRunRolling { get { return playerSpeedWhileRunRolling; } }

    [SerializeField] private float playerSpeedLightAttacking = 1f;
    public float PlayerSpeedLightAttacking { get { return playerSpeedLightAttacking; } }

    [SerializeField] private float playerSpeedHeavyAttacking = 1f;
    public float PlayerSpeedHeavyAttacking { get { return playerSpeedHeavyAttacking; } }

    [SerializeField] private float playerSpeedDoingHurricane = 15f;
    public float PlayerSpeedDoingHurricane { get { return playerSpeedDoingHurricane; } }
}
