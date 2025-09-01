using GameMathods;
using UnityEngine;

public class StartBackGround : MonoBehaviour,
ILoad
{
    public void Load()
    {
        var sound = Find.SoundSource("StartBackGround");
        GameService.soundManager.OnBackGround(sound);
    }
}
