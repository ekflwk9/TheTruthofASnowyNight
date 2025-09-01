using GameMathods;
using UnityEngine;

public abstract class SoundComponent : MonoBehaviour,
IStarting
{
    [Header("소리 범위")]
    [SerializeField] protected float maxDistance;

    protected AudioSource source;

    public virtual void Starting()
    {
        var checkComponent = GetComponent<AudioSource>();
        if (checkComponent != null) DestroyImmediate(checkComponent);

        source = gameObject.AddComponent<AudioSource>();
        GameService.SetComponent(this);

        source.loop = true;
        source.playOnAwake = false;
        source.rolloffMode = AudioRolloffMode.Custom;
        source.spatialBlend = 1;
        source.maxDistance = maxDistance;
        source.volume = GameService.soundManager.effectVolume;
    }

    public virtual void SoundVolume(float _volume) => source.volume = _volume;
}
