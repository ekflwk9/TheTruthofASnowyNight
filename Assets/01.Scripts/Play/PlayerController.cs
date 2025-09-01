using GameMathods;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour,
IStarting, ISave, IEnd
{
    [Header("이동속도")]
    [SerializeField] private float walkSpeed;
    [SerializeField] private float runSpeed;

    private Rigidbody rigid;
    private Animator anim;
    private bool isJump;
    private Vector3 jumpPos;

    public void Starting()
    {
        rigid = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();

        GameService.SetComponent(this);
        DontDestroyOnLoad(this.gameObject);
    }

    public void Save()
    {
        //var sceneName = SceneManager.GetActiveScene().name;
        //GameService.dataManager.AddData(new Data("SceneName", 0, false, 0, sceneName));
        //GameService.dataManager.AddData(new Data("PlayerPos", 0, false, 0, null, this.transform.position));
    }

    public void End()
    {
        //if (GameService.progress.isLoad)
        //{
        //    this.transform.position = GameService.dataManager.FindData("PlayerPos").vector;
        //}
    }

    private void Update()
    {
        if (!GameService.progress.isStop)
        {
            Move();
            //Jump();
        }
    }

    private void Move()
    {
        var vertical = 0;
        var horizontal = 0;

        //앞 뒤
        if (Input.GetKey(GameKey.key[ConstKey.Up])) vertical = 1;
        else if (Input.GetKey(GameKey.key[ConstKey.Down])) vertical = -1;

        //좌 우
        if (Input.GetKey(GameKey.key[ConstKey.Left])) horizontal = -1;
        else if (Input.GetKey(GameKey.key[ConstKey.Right])) horizontal = 1;

        //이동
        var moveSpeed = 0f;
        var animSpeed = 0f;
        var pos = transform.forward * vertical + transform.right * horizontal;

        if (vertical != 0 || horizontal != 0)
        {
            if (Input.GetKey(GameKey.key[ConstKey.Run]))
            {
                moveSpeed = runSpeed;
                animSpeed = 1.5f;
            }

            else
            {
                moveSpeed = walkSpeed;
                animSpeed = 1f;
            }
        }

        jumpPos.y = rigid.linearVelocity.y;
        rigid.linearVelocity = pos.normalized * moveSpeed + jumpPos;
        MoveAction(animSpeed);
    }

    public void MoveAction(float _speed)
    {
        _speed = isJump ? 0f : _speed;
        anim.SetFloat("MoveSpeed", _speed);
    }

    private void Jump()
    {
        if (Input.GetKeyDown(GameKey.key[ConstKey.Jump]))
        {
            if (!isJump) rigid.linearVelocity = Vector3.up * 5f;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground")) isJump = false;
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground")) isJump = true;
    }
}
