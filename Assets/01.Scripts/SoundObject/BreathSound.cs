using UnityEngine;
using GameMathods;

public class BreathSound : SoundComponent
{
    private AudioClip breathSound;

    public override void Starting()
    {
        base.Starting();
        breathSound = Find.SoundSource("HeavyBreath");
        source.clip = breathSound;

        GameService.eventManager.AddEvent(EventCode.OnBreath, OnSound);
        GameService.eventManager.AddEvent(EventCode.OffBreath, OffSound);
    }

    private void OnSound() => source.Play();
    private void OffSound() => source.Stop();
}
