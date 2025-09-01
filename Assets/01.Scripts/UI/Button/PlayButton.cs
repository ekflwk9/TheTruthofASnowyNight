using UnityEngine.SceneManagement;
using UnityEngine;

public class PlayButton : Button
{
    protected override void Click()
    {
        //��ġ �̹��� ��Ȱ��ȭ
        touchImage.SetActive(false);

        //���� ȭ���� ��� �÷��̾�� ������ ���� �Ұ����� ���·�
        if(SceneManager.GetActiveScene().name == "Start")
        {
            GameService.eventManager.CallUi(UiCode.OffMenu);
            GameService.eventManager.CallUi(UiCode.OnStartWindow);
            GameService.dataManager.Save();
            return;
        }

        //�÷��� ���̶�� ���� ���� �� ���� ���·� ��ȯ
        var isStop = !GameService.progress.isStop;

        //���콺 ������
        Cursor.visible = isStop;
        GameService.progress.StopController(isStop);

        if (isStop)
        {
            Cursor.lockState = CursorLockMode.None;
            GameService.eventManager.CallUi(UiCode.OnMenu);
        }

        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            GameService.eventManager.CallUi(UiCode.OffMenu);
            GameService.dataManager.Save();
        }

        if (GameService.playerController != null) GameService.playerController.MoveAction(0f);
        else Debug.Log("�Ҵ�� �÷��̾ �������� ����");
    }
}
