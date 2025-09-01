using GameMathods;
using UnityEngine;

public class GroundSound : MonoBehaviour,
IStarting
{
    public void Starting() => GameService.soundManager.ChangeWalkSound(Find.SoundSource("SnowWalk")); 
}
