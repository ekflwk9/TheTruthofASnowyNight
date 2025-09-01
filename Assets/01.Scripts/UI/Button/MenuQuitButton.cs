using UnityEngine;

public class MenuQuitButton : Button
{
    protected override void Click()
    {
        GameService.dataManager.Save();
        Application.Quit();
    }
}
