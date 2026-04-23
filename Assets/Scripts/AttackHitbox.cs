using UnityEngine;
using System.Collections;

public class AttackHitbox : MonoBehaviour
{
    private Collider2D col;
    private AttackData data;
    private Coroutine routine;
    private Vector2 debugSize;

    private void Awake()
    {
        col = GetComponent<Collider2D>();
        col.enabled = false;
    }

    public void Play(AttackData attackData)
    {
        data = attackData;

        StopAllCoroutines();
        StartCoroutine(Run());
    }

    private IEnumerator Run()
    {
        col.enabled = true;

        // 小判定
        SetSize(data.startSize);
        yield return new WaitForSeconds(data.startTime);

        // 最大判定
        SetSize(data.maxSize);
        yield return new WaitForSeconds(data.maxTime);

        // 終了
        col.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (data == null) return;
        if (collision.TryGetComponent<Enemy>(out var enemy))
        {
            int damage = data.damage * Player.Instance.AttackPower;
            enemy.TakeDamage(damage);
            Debug.Log($"敵にヒット！ ダメージ: {damage}");
        }
    }

    private void SetSize(Vector2 size)
    {
        debugSize = size;
        if (col is BoxCollider2D box)
        {
            box.size = size;
        }
        float dir = Mathf.Sign(transform.root.localScale.x);

        transform.localPosition = new Vector3(
        data.offset.x * dir,
        data.offset.y,
        0);
    }

    private void OnDrawGizmos()
    {
        if (!Application.isPlaying) return;

        Gizmos.color = Color.red;

        Vector3 pos = transform.position;

        Gizmos.DrawWireCube(pos, new Vector3(debugSize.x, debugSize.y, 0));
    }
}