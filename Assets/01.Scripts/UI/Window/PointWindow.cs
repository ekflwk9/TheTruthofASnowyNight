using GameMathods;
using UnityEngine;
using UnityEngine.UI;

public class PointWindow : MonoBehaviour,
IStarting
{
    private Animator anim;

    public void Starting()
    {
        anim = GetComponent<Animator>();

        GameService.eventManager.AddUi(UiCode.PointEnter, PointEnter);
        GameService.eventManager.AddUi(UiCode.PointExit, PointExit);
        GameService.eventManager.AddUi(UiCode.PointClick, PointClick);
    }

    private void PointEnter() => anim.Play("PointEnter", -1, 0);
    private void PointExit() => anim.Play("PointExit", -1, 0);
    private void PointClick() => anim.Play("PointClick", -1, 0);
}
