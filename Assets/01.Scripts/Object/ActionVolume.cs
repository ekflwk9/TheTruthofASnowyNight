using GameMathods;
using UnityEngine;

public class ActionVolume : MonoBehaviour,
IStarting
{
    private Animator anim;

    public void Starting()
    {
        anim = GetComponent<Animator>();

        GameService.eventManager.AddUi(UiCode.StopVolume, OffVolume);
        GameService.eventManager.AddUi(UiCode.OnVignette, OnVignette);
        GameService.eventManager.AddUi(UiCode.OnFilm, OnFilm);
    }

    private void OffVolume() => anim.Play("StopVolume", -1, 0);
    private void OnVignette() => anim.Play("Vinette", -1, 0);
    private void OnFilm() => anim.Play("Film", -1, 0);
}
