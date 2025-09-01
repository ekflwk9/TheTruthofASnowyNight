using UnityEngine;
using System.Text;
using GameMathods;
using TMPro;

public class TypingComponent : MonoBehaviour,
IStarting
{
    private int charCount;

    private AudioClip sound;
    private TMP_Text showText;
    private StringBuilder builder = new StringBuilder(50);

    public void Starting()
    {
        GameService.eventManager.AddUi(UiCode.OnTalk, On);
        GameService.eventManager.AddUi(UiCode.OffTalk, Off);

        sound = Find.SoundSource("TypingActionSound");
        showText = Find.Child(this.transform, "TalkText").GetComponent<TMP_Text>();
        showText.text = "";
    }

    private void On()
    {
        charCount = 0;
        builder.Clear();
        GameService.soundManager.OnEffect(sound);

        if (!this.gameObject.activeSelf) this.gameObject.SetActive(true);
    }

    private void Off()
    {
        showText.text = "";
        this.gameObject.SetActive(false);
    }

    private void FixedUpdate()
    {
        if (charCount < GameService.talkManager.talk.Length)
        {
            var talk = GameService.talkManager.talk;

            builder.Append(talk[charCount]);
            showText.text = builder.ToString();
            charCount++;

            if (talk.Length == charCount) GameService.talkManager.ShowButtonMenu();
        }
    }
}
