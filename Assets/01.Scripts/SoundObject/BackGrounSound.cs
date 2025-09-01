using GameMathods;
using UnityEngine;

public class BackGrounSound : SoundComponent
{
    public override void Starting()
    {
        base.Starting();

        source.rolloffMode = AudioRolloffMode.Logarithmic;
        source.spatialBlend = 0;
    }

    public void OnSound(AudioClip _sound)
    {
        if(_sound == null)
        {
            source.Stop();
        }

        else
        {
            source.clip = _sound;
            source.Play();
        }
    }
}
