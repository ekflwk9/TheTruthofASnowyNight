using GameMathods;
using UnityEngine;

public class FadeComponent : MonoBehaviour,
IStarting
{
    private Animator anim;
    private Function function;

    public void Starting()
    {
        anim = GetComponent<Animator>();
        GameService.SetComponent(this);
    }

    public void PlayFade(bool _fadeOut, Function _function = null, float _fadeSpeed = 1f)
    {
        anim.SetFloat("Speed", _fadeSpeed);
        anim.Play(_fadeOut ? "FadeOut" : "FadeIn", -1, 0);

        function = _function;
    }

    private void EndFadeFuntion()
    {
        if (function != null)
        {
            function();
            function = null;
        }
    }
}
