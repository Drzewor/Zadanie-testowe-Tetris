using System;
using System.Collections;
using UnityEngine;

public class BlockGroup : MonoBehaviour
{
    public event EventHandler<OnValidCollisionEventArgs> OnValidCollision;
    public class OnValidCollisionEventArgs : EventArgs {
        public bool isControlled;
        public PlayerType playerType;
    }

    private Rigidbody2D rb;
    private Coroutine currentCoroutine;
    private Vector2 targetPosition;
    private PlayerType playerType;
    private bool isMoving = false;
    private bool isControlled = false;
    private bool isDetaching = false;
    private const float MoveSpeed = 7f;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        foreach (Transform child in transform)
        {
            if(child.TryGetComponent(out Block block))
            {
                block.OnBlockDestroyed += Block_OnBlockDestroyed;
            }
        } 
    }

    // when One of blocks i group is destroyed group is detach, every block gets its own Rigidbody2D so it can fall
    // on his own. Also Destroy this BlockGroup at the end.
    private void Block_OnBlockDestroyed(object sender, EventArgs e)
    {
        if(isDetaching) return;

        isDetaching = true;
        foreach (Transform child in transform)
        {
            if(child.GetComponent<Block>() == (Block)sender) continue;

            Rigidbody2D childrb = child.gameObject.AddComponent<Rigidbody2D>();
            childrb.freezeRotation = true;
            child.parent = null;
        }
        
        Destroy(gameObject);
    }

    /// <summary>
    /// Rotate this BlockGroup by 90 degrees
    /// </summary>
    public void Rotate()
    {
        float newRotation = rb.rotation + 90;
        Vector3 newEuler = new Vector3(0,0,newRotation);
        transform.rotation = Quaternion.Euler(newEuler);
    }

    /// <summary>
    /// Move this BlockGroup by 1 in left or right, depending on moveValue
    /// </summary>
    /// <param name="moveValue"></param>
    public void MoveGroup(float moveValue)
    {
        if(isMoving) return;

        Vector2 moveDirection = moveValue > 0 ? Vector2.right : Vector2.left;
        currentCoroutine = StartCoroutine(MoveToPosition(moveDirection));
    }

    /// <summary>
    /// Smoothly move group by 1 in given direction wiile keeping gravity fall velocity. At end its snap 
    /// whole group exactly at desire position.
    /// </summary>
    /// <param name="moveDirection"></param>
    /// <returns></returns>
    private IEnumerator MoveToPosition(Vector2 moveDirection)
    {
        isMoving = true;

        targetPosition = rb.position + moveDirection;

        while(Math.Abs(rb.position.x - targetPosition.x) > 0.05f)
        {
            rb.linearVelocity = new Vector2((targetPosition.x - rb.position.x) * MoveSpeed, rb.linearVelocity.y);
            yield return null;
        }

        rb.position = new Vector2(targetPosition.x, rb.position.y);
        rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);

        isMoving = false;
        currentCoroutine = null;
    }

    //When block group hit somthing its automaticly snap to nearest round x position. When its valid target like 
    //floor of arena or other blocks it invoke OnValidCollision event.
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Floor" || collision.gameObject.tag == "Block")
        {
            OnValidCollision?.Invoke(this, new OnValidCollisionEventArgs {
                isControlled = isControlled,
                playerType = playerType
            });
        }

        rb.linearVelocityX = 0;

        float endXPosition = Mathf.Round(rb.position.x);
        Vector2 endPosition = new Vector2(endXPosition,rb.position.y);
        rb.MovePosition(endPosition);

        StartCoroutine(MoveCoolDown());

        if(currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine);
        }
    }

    /// <summary>
    /// When group hit somthing np. wall its cannot be move again for 1 second 
    /// </summary>
    /// <returns></returns>
    private IEnumerator MoveCoolDown()
    {
        yield return new WaitForSeconds(1f);

        isMoving = false;
    }

    public bool IsControlled()
    {
        return isControlled;
    }

    public void SetIsController(bool isControlled)
    {
        this.isControlled = isControlled;
    }

    public void SetPlayerType(PlayerType playerType)
    {
        this.playerType = playerType;
    }
}
