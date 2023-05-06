using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleGenerator : MonoBehaviour
{
    public GameObject wallSegment;
    public void MakeObstacle(Layout layout, int offset)
    {
        for( int i = 0; i < layout.TopLayer.Length; i++)
        {
            if (layout.TopLayer[i])
            {
                Debug.Log(layout.TopLayer[i]);
                Instantiate(wallSegment, new Vector3(-12 + 6 * i, 9.5f, offset), Quaternion.identity);
            }
            if (layout.BotLayer[i])
            {
                Instantiate(wallSegment, new Vector3(-12 + 6 * i, 3.5f, offset), Quaternion.identity);
            }
        }
        
    }
}
