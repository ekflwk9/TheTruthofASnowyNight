using UnityEngine;
using GameMathods;

public class EffectSound : SoundComponent
{
    public override void Starting()
    {
        base.Starting();

        source.rolloffMode = AudioRolloffMode.Logarithmic;
        source.spatialBlend = 0;
    }

    public void OnSound(AudioClip _sound)
    {
        if(_sound != null) source.PlayOneShot(_sound);
        else source.Stop();
    }
}
