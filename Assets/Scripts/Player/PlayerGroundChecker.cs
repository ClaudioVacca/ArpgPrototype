
using UnityEngine;

public class PlayerGroundChecker : MonoBehaviour, IPlayerGroundChecker
{
    [SerializeField] private Transform groundChecker;
    [SerializeField] private float groundCheckerRadius = 0.2f;

    public bool IsGrounded()
    {
        if (groundChecker == null)
            throw new System.Exception("GroundChecker needs a transform!");

        return Physics.CheckSphere(groundChecker.position, groundCheckerRadius, LayerMask.GetMask("Default"), QueryTriggerInteraction.Ignore);
    }
}
