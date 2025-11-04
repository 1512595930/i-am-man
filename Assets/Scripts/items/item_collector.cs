using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class item_collector : MonoBehaviour
{
    [Header("预制体")]
    public GameObject orangePrefab;
    [Header("刷新时间")]
    public float RESPAWN_DELAY = 3f;
    private BetterJumpController playerJumpController;
     void Start()
    {
        playerJumpController = GetComponent<BetterJumpController>();
    }
    // Start is called before the first frame update
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("orange"))
        {
            Vector3 orangePosition = collision.gameObject.transform.position;//记录位置
            Destroy(collision.gameObject);//销毁橘子

            StartCoroutine(RespawnOrangeAfterDelay(orangePosition, RESPAWN_DELAY));
            //启动协程

            playerJumpController.setcurrentJumpCount();
        }
        if (collision.gameObject.CompareTag("spikes"))
        {
            Debug.Log("die,die");
        }
    }
    // 协程方法：延迟重新生成orange
    private IEnumerator RespawnOrangeAfterDelay(Vector3 spawnPosition, float delay)
    {
        // 暂停协程，等待指定秒数
        yield return new WaitForSeconds(delay);

        // 等待结束后，创建新的orange
        Instantiate(orangePrefab, spawnPosition, Quaternion.identity);
    }
}
