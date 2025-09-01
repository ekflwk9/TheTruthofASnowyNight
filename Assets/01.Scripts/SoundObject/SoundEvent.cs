using UnityEngine;
using GameMathods;

public class SoundEvent : SoundComponent,
IEnd
{
    public override void Starting()
    {
        base.Starting();
        var sound = Find.SoundSource("ClothesDrop");
        source.clip = sound;

        GameService.eventManager.AddEvent(EventCode.OnSoundEvent, OnEventSound);
        GameService.eventManager.AddEvent(EventCode.OffSoundEvent, OffEventSound);

        GameService.SetComponent(this);
    }

    public void End()
    {
        GameService.eventManager.RemoveEvent(EventCode.OnSoundEvent);
        GameService.eventManager.RemoveEvent(EventCode.OffSoundEvent);
    }

    private void OnEventSound() => source.Play();
    private void OffEventSound() => source.Stop();
}
