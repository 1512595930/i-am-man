using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{

    [SerializeField] private Transform player;
    [SerializeField] private float smoothSpeed = 11f;  // 添加平滑移动
    [SerializeField] private Vector3 offset = new Vector3(0, 0, -8);  // 相机偏移

    void LateUpdate()  // 使用LateUpdate确保在玩家移动后执行
    {
        if (player == null) return;  // 空引用检查
        // 平滑移动
        transform.position = Vector3.Lerp(transform.position, player.position + offset, smoothSpeed * Time.deltaTime);
    }
}