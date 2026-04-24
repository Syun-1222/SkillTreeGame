using UnityEngine;

public class BackgroundManager : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private Transform bgFront;
    [SerializeField] private Transform bgCenter;
    [SerializeField] private Transform bgBack;

    private float width;

    private void Start()
    {
        width = bgCenter.GetComponent<SpriteRenderer>().bounds.size.x;
    }

    private void Update()
    {
        
        float diff = player.position.x - bgCenter.position.x;

        // 右に移動（半分超えた）
        if (diff > width * 0.5f)
        {
            MoveRight();
        }
        // 左に移動
        else if (diff < -width * 0.5f)
        {
            MoveLeft();
        }
    }

    private void MoveRight()
    {
        // Backを前へ
        bgBack.position = bgFront.position + Vector3.right * width;

        // 入れ替え
        Transform temp = bgCenter;
        bgCenter = bgFront;
        bgFront = bgBack;
        bgBack = temp;
    }

    private void MoveLeft()
    {
        // Frontを後ろへ
        bgFront.position = bgBack.position - Vector3.right * width;

        // 入れ替え
        Transform temp = bgCenter;
        bgCenter = bgBack;
        bgBack = bgFront;
        bgFront = temp;
    }
}