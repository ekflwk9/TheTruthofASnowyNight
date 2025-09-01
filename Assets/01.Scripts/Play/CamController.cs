using GameMathods;
using UnityEngine;

public class CamController : MonoBehaviour,
IStarting
{
    private float sensitive = 2f;
    private Transform player;
    private Animator anim;

    private Vector2 mouse;
    private Vector3 lookPos;

    public void Starting()
    {
        //프레임 60으로 제한
        Application.targetFrameRate = 60;

        if (transform.parent == null) Debug.Log("부모 오브젝트가 존재하지 않음");
        player = transform.parent;

        anim = this.gameObject.GetComponent<Animator>();
        GameService.SetComponent(this);
    }

    private void Update()
    {
        if (!GameService.progress.isStop)
        {
            mouse.x += Input.GetAxisRaw("Mouse X") * sensitive;
            mouse.y += Input.GetAxisRaw("Mouse Y") * sensitive;

            if (mouse.y > 90) mouse.y = 90;
            else if (mouse.y < -70) mouse.y = -70;

            player.transform.rotation = Quaternion.Euler(0, mouse.x, 0);
            this.transform.rotation = Quaternion.Euler(-mouse.y, mouse.x, 0);
        }

        else if (lookPos != Vector3.zero)
        {
            var lookRote = Quaternion.LookRotation(lookPos);
            var newRote = Quaternion.RotateTowards(this.transform.rotation, lookRote, 0.8f);
            this.transform.rotation = newRote;
        }
    }

    public void LookNpc(Vector3 _targetPos)
    {
        if (_targetPos == Vector3.zero)
        {
            lookPos = Vector3.zero;
            anim.Play("ZoomOut", -1, 0);
        }

        else
        {
            lookPos = _targetPos - this.transform.position;
            anim.Play("ZoomIn", -1, 0);
        }
    }
    
    public void ChangeSensitive(float _sensitive) => sensitive = _sensitive;
}
