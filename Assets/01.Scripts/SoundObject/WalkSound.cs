using UnityEngine;

public class WalkSound : MonoBehaviour
{
    private void OnWalkSound() => GameService.soundManager.OnWalkSound();
}
