using GameMathods;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SnowWindSound : MonoBehaviour,
IStarting, IEnd
{
    private AudioClip inSound;
    private AudioClip outSound;

    public void Starting()
    {
        outSound = Find.SoundSource("SnowWind");
        inSound = Find.SoundSource("Wind");

        GameService.eventManager.AddEvent(EventCode.OnInSound, OnInSound);
        GameService.eventManager.AddEvent(EventCode.OnOutSound, OnOutSound);

        if (SceneManager.GetActiveScene().name == "Play") GameService.soundManager.OnBackGround(inSound);
        else GameService.soundManager.OnBackGround(outSound);

        GameService.SetComponent(this);
    }
    
    private void OnInSound() => GameService.soundManager.OnBackGround(inSound);
    private void OnOutSound() => GameService.soundManager.OnBackGround(outSound);

    public void End()
    {
        GameService.eventManager.RemoveEvent(EventCode.OnInSound);
        GameService.eventManager.RemoveEvent(EventCode.OnOutSound);
    }
}
