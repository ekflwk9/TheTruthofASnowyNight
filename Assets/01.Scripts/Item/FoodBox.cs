using GameMathods;
using UnityEngine;

public class FoodBox : MonoBehaviour,
IStarting, IInteraction, IEnd
{
    private int foodCount;
    private AudioClip sound;
    private TouchingComponent isTouch;

    public void Starting()
    {
        //��¥�� ���� ���ķ� ���� ����
        switch (GameService.progress.day)
        {
            case 1:
                foodCount = 6; 
                break;

            case 2:
                foodCount = 5;
                break;

            case 3:
                foodCount = 4;
                break;
        }

        isTouch = this.gameObject.AddComponent<TouchingComponent>();
        isTouch.Starting();

        sound = Find.SoundSource("FoodBox");
        GameService.SetComponent(this);
    }

    public void Interaction()
    {
        //�ķ��� ���� ���
        if (0 < foodCount)
        {
            if (GameService.itemController.GetItem("Canned" + foodCount.ToString()))
            {
                GameService.soundManager.OnEffect(sound);
                foodCount -= 1;
            }

            GameService.eventManager.CallUi(UiCode.PointEnter);
        }

        //�ķ��� ���� ��� ��Ʈ ���
        else
        {
            var talk = GameService.talkManager;
            var isKorean = GameService.progress.isKorean;

            switch (talk.talkID)
            {
                case 0:
                    isTouch.ShowOutLine(false);
                    talk.SetTalker(this);
                    talk.ShowTalk(isKorean ? "���� �ڽ� �ȿ��� �ƹ��͵� ���� ���� �ʾ�.." : "Now there's nothing left in the box..");

                    talk.ShowYesButton("...", -1);
                    talk.ShowNoButton(null, 0);
                    break;

                case -1:
                    isTouch.ShowOutLine(true);
                    talk.EndTalk(true);
                    break;
            }
        }
    }

    public virtual void End() => GameService.gameManager.Remove(this);
}
