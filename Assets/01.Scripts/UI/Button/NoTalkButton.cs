using GameMathods;
using UnityEngine;
using UnityEngine.EventSystems;

public class NoTalkButton : Button,
IStarting
{
    private int nextID;

    public override void Starting()
    {
        base.Starting();
        GameService.eventManager.AddUi(UiCode.OnNoButton, On);
        GameService.eventManager.AddUi(UiCode.OffNoButton, Off);
    }

    private void On()
    {
        if (nextID != 0) this.gameObject.SetActive(true);
    }

    private void Off()
    {
        nextID = GameService.talkManager.talkID;
        titleText.text = GameService.talkManager.talkButton ?? "";
        if (this.gameObject.activeSelf) this.gameObject.SetActive(false);
    }

    protected override void Click()
    {
        GameService.talkManager.NextTalk(nextID);
        touchImage.SetActive(false);
    }

    public override void OnPointerClick(PointerEventData eventData) => Click();
}
