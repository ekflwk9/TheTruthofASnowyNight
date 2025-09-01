using GameMathods;
using UnityEngine;

public class StartQuitButton : Button,
IEnd
{
    protected override void Click() => Application.Quit();
    public void End() => GameService.progress.Remove(this);
}
