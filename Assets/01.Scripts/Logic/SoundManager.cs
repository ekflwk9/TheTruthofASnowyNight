using GameMathods;
using UnityEngine;

public class SoundManager
{
    public float effectVolume { get; private set; }

    public AudioClip walkSound { get; private set; }

    private EffectSound effectSound;
    private BackGrounSound backGroundSound;
    private SoundComponent[] itemSound = new SoundComponent[0];

    //게임 사운드
    private BreathSound breathSound;

    public void OnWalkSound() => effectSound.OnSound(walkSound);
    public void OnEffect(AudioClip _sound) => effectSound.OnSound(_sound);
    public void OnBackGround(AudioClip _sound) => backGroundSound.OnSound(_sound);
    public void ChangeWalkSound(AudioClip _sound) => walkSound = _sound;

    public void ResetScripts(bool _allRemove)
    {
        effectSound.OnSound(null);
        backGroundSound.OnSound(null);
        itemSound = new SoundComponent[0];

        if (_allRemove)
        {
            effectSound = null;
            backGroundSound = null;
            breathSound = null;
        }
    }

    public void AddScripts(SoundComponent _component)
    {
        if (_component is EffectSound isEffect) effectSound = isEffect;
        else if (_component is BackGrounSound isBackGround) backGroundSound = isBackGround;
        else if (_component is BreathSound isBreath) breathSound = isBreath; //
        else itemSound = Service.AddArray(itemSound, _component);
    }

    public void SoundVolume(float _volume)
    {
        effectVolume = _volume;
        effectSound.SoundVolume(_volume);
        backGroundSound.SoundVolume(_volume);
        breathSound.SoundVolume(_volume);

        for (int i = 0; i < itemSound.Length; i++)
        {
            itemSound[i].SoundVolume(_volume);
        }
    }
}
