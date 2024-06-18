using UnityEngine;

public class Reposition : MonoBehaviour
{
    private Collider2D _coll;
    private GameManager _gameManager;
    private void Start()
    {
        _coll = GetComponent<Collider2D>();
        _gameManager = GameManager.Instance;
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Area"))
            return;
        Vector2 playerPos = _gameManager.player.transform.position;
        Vector2 myPos = transform.position;

        switch (transform.tag)
        {
            case "Ground":
                
                float diffX = playerPos.x - myPos.x;
                float diffY = playerPos.y - myPos.y;
                float dirX = diffX < 0 ? -1 : 1;
                float dirY = diffY < 0 ? -1 : 1;
                diffX = Mathf.Abs(diffX);
                diffY = Mathf.Abs(diffY);
                
                if (diffX > diffY)
                    transform.Translate(Vector3.right * dirX * other.bounds.size.x * 2);
                else if (diffX < diffY)
                    transform.Translate(Vector3.up * dirY * other.bounds.size.y * 2);
                break;
            case "Enemy":
                if (_coll.enabled)
                {
                    Vector3 dist = playerPos - myPos;
                    transform.Translate(dist * 2);
                }
                break;
        }
    }
}
