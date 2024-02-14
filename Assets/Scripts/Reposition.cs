using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameManager;

public class Reposition : MonoBehaviour
{
    void OnTriggerExit2D(Collider2D collision)
    {
        if (!cillision.CompareTag("Area"))
            return;
        Vector2 PlayerPos = GameManager.Instance().player.transform.position;
        Vector2 myPos = transform.position;
        float diffX = Mathf.Abs(PlayerPos.x - myPos.x);
        float diffY = Mathf.Abs(PlayerPos.y - myPos.y);
        
        Vector2 playerDir = GameManager.Instance().player.inputVec;
        float dirX = playerDir.x < 0 ? -1 : 1;
        float dirY = playerDir.y < 0 ? -1 : 1;

        switch (transform.tag)
        {
            case "Ground" : 
                break;
            case "Enemy" : 
                break;
        }
    }
}
