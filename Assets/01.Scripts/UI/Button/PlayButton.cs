using UnityEngine.SceneManagement;
using UnityEngine;

public class PlayButton : Button
{
    protected override void Click()
    {
        //터치 이미지 비활성화
        touchImage.SetActive(false);

        //시작 화면일 경우 플레이어는 여전히 조작 불가능한 상태로
        if(SceneManager.GetActiveScene().name == "Start")
        {
            GameService.eventManager.CallUi(UiCode.OffMenu);
            GameService.eventManager.CallUi(UiCode.OnStartWindow);
            GameService.dataManager.Save();
            return;
        }

        //플레이 중이라면 조작 가능 및 원래 상태로 전환
        var isStop = !GameService.progress.isStop;

        //마우스 포인터
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
        else Debug.Log("할당된 플레이어가 존재하지 않음");
    }
}
