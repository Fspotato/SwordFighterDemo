using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
public class TileLoader : MonoBehaviour
{
    public string tilemapName; //設置tilemap的名稱
    Tilemap tmp;
    void Start()
    {
        tilemapName = "Level1";
        LoadTile();
    }
    void LoadTile()
    {
        tmp = LoadTilemap(tilemapName);
        if (tmp != null)
        {
            Tilemap map = Instantiate(tmp, transform); //生成Tilemap
            map.transform.position = Vector3.zero; //把Tilemap放到原點上
            map.GetComponent<TilemapRenderer>().sortingLayerName = "Ground"; // 設置Layer
        }
        else
        {
            Debug.Log("Tilemap加載失敗");
        }
    }
    private Tilemap LoadTilemap(string name)
    {
        return Resources.Load<Tilemap>("Maps/" + name); //Resources讀取 放置方式：Assets/Resources/Maps/TilemapNama
    }

}
