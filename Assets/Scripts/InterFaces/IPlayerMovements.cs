using UnityEngine;

public interface IPlayerMovements
{
    bool IsMoving { get; set; }
    bool CanMove { get; set; }
    bool CanTurn { get; set; }
    bool CanRun { get; set; }
    bool CanCrouch { get; set; }
    bool IsRunning { get; set; }
    bool IsCrouching { get; set; }

    void ProcessMovements();
}