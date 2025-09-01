using UnityEngine;
using GameMathods;

public class TestSound : SoundComponent
{
    public override void Starting()
    {
        base.Starting();

        //source.clip = Find.Resource("Sound").GetComponent<AudioSource>().clip;
        //source.playOnAwake = true;
        //source.loop = true;
    }
}
