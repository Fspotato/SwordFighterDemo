using System;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Camera _camera;
    public List<GameObject> player;
    public Vector3 minPosition; //Camera的最小位置
    public Vector3 maxPosition; //Camera的最大位置
    public Vector3 targetPosition; //Camera的目標座標
    public float size;  //Camera的目標大小
    Vector3 min, max; //用於記錄所有角色中的最左上及最右上的位置
    // Start is called before the first frame update
    void Start()
    {
        minPosition = new Vector3(0.9f, 0f, -10f);
        maxPosition = new Vector3(22.5f, 12.5f, -10f);
    }

    // Update is called once per frame
    void Update()
    {

    }
    void Control()
    {
        if (player.Count != 0)
        {
            min = new Vector3(99f, 99f, -10f);
            max = new Vector3(-99f, -99f, -10f);
            foreach (var temp in player)
            {
                min.x = Math.Min(min.x, temp.transform.position.x);
                min.y = Math.Min(min.y, temp.transform.position.y);
                max.x = Math.Max(max.x, temp.transform.position.x);
                max.y = Math.Max(max.y, temp.transform.position.y);
            }
            //控制相機大小
            size = Vector2.Distance(min, max) / 2f;
            size = Mathf.Clamp(size, 4f, 6f);
            _camera.orthographicSize = size;
            //控制相機位置
            float CameraWidth = size * _camera.aspect;
            float CameraHeight = size;
            float ClampedX = Mathf.Clamp(min.x + CameraWidth - 6f, minPosition.x + CameraWidth, maxPosition.x - CameraWidth);
            float ClampedY = Mathf.Clamp(min.y + CameraHeight - 3f, minPosition.y + CameraHeight, maxPosition.y - CameraHeight);
            transform.position = new Vector3(ClampedX, ClampedY, -10f);
            //原理待反推

            //備案-別刪
            /*//根據所有目標位置中的最小值設為Camera的起點座標
            targetPosition = new Vector3(min.x, min.y, -10f);
            
            //把相機限制在一定範圍內
            targetPosition.x = Math.Clamp(targetPosition.x, minPosition.x, maxPosition.x);
            targetPosition.y = Math.Clamp(targetPosition.y, minPosition.y, maxPosition.y);
            //把相機移動到Player位置
            transform.position = targetPosition;
            _camera.orthographicSize = size;*/
        }
        else
        {
            //搜尋所有Player
            var players = GameObject.FindGameObjectsWithTag("Player");
            foreach (var temp in players)
            {
                player.Add(temp);
            }
        }
    }
}
