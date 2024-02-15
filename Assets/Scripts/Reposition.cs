using UnityEngine;

public class Reposition : MonoBehaviour
{
    private Collider2D _coll;
    private void Awake()
    {
        _coll = GetComponent<Collider2D>();
    }
    void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Area"))
            return;
        Vector2 playerPos = GameManager.Instance.player.transform.position;
        Vector2 myPos = transform.position;
        float diffX = Mathf.Abs(playerPos.x - myPos.x);
        float diffY = Mathf.Abs(playerPos.y - myPos.y);
        
        Vector2 playerDir = GameManager.Instance.player.InputVec;
        float dirX = playerDir.x < 0 ? -1 : 1;
        float dirY = playerDir.y < 0 ? -1 : 1;

        switch (transform.tag)
        {
            case "Ground":
                if (diffX > diffY)
                    transform.Translate(Vector3.right * dirX * collision.bounds.size.x * 2);
                else if (diffX < diffY)
                    transform.Translate(Vector3.up * dirY * collision.bounds.size.y * 2);
                break;
            case "Enemy":
                if (_coll.enabled)
                {
                    transform.Translate(playerDir * collision.bounds.size * 2);
                }
                break;
        }
    }
}
