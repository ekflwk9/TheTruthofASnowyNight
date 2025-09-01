using GameMathods;
using UnityEngine;

public class UiController : MonoBehaviour,
IStarting
{
    public void Starting()
    {
        GameService.SetComponent(this);
        DontDestroyOnLoad(this.gameObject);
    }

    private void Update()
    {
        if(!GameService.progress.isStop)
        {
            OnMenu();
        }
    }

    private void OnMenu()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            var isStop = !GameService.progress.isStop;

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
            }

            if (GameService.playerController != null) GameService.playerController.MoveAction(0f);
            else Debug.Log("할당된 플레이어가 존재하지 않음");
        }
    }
}
