
using UnityEngine;

public class PlayerGroundChecker : MonoBehaviour, IPlayerGroundChecker
{
    [SerializeField] private Transform groundChecker;
    [SerializeField] private float groundCheckerRadius = 0.2f;

    public bool IsGrounded()
    {
        return Physics.CheckSphere(groundChecker.position, groundCheckerRadius, LayerMask.GetMask("Default"), QueryTriggerInteraction.Ignore);
    }
}
