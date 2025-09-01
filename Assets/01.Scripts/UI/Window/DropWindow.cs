using GameMathods;
using TMPro;
using UnityEngine;

public class DropWindow : MonoBehaviour,
IStarting, ILenguage
{
    private TMP_Text infoText;
    private TMP_Text buttonText;

    public void Starting()
    {
        GameService.SetComponent(this);
        GameService.eventManager.AddUi(UiCode.OnDrop, On);
        GameService.eventManager.AddUi(UiCode.OffDrop, Off);

        infoText = Find.Child(this.transform,"InfoText").GetComponent<TMP_Text>();
        buttonText = Find.Child(this.transform,"ButtonText").GetComponent<TMP_Text>();
    }

    private void On()
    {
        if (!this.gameObject.activeSelf)
        {
            buttonText.text = GameKey.key[ConstKey.Drop].ToString();
            this.gameObject.SetActive(true);
        }
    }

    private void Off()
    {
        if (this.gameObject.activeSelf) this.gameObject.SetActive(false);
    }

    public void ChangeLenguage() => infoText.text = GameService.progress.isKorean ? "아이템 던지기" : "Drop Item";
}
