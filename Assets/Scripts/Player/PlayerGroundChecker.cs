
using UnityEngine;

public class PlayerGroundChecker : MonoBehaviour, IPlayerGroundChecker
{
    [SerializeField] private Transform groundChecker;
    [SerializeField] private Vector3 groundCheckerBoxSize = new Vector3(0.25f, 0.02f, 0.25f);

    public bool IsGrounded()
    {
        if (groundChecker == null)
            throw new System.Exception("GroundChecker needs a transform!");

        return Physics.CheckBox(groundChecker.position, groundCheckerBoxSize, Quaternion.identity, LayerMask.GetMask("Default"), QueryTriggerInteraction.Ignore);
    }
}
