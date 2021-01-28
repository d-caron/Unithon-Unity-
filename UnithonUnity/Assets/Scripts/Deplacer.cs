using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deplacer : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        deplacer (new Vector3 (0, 0, 0));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void deplacer (Vector3 dest)
    {
        float dist_x = transform.position.x - dest.x;
        float dist_z = transform.position.z - dest.z;

        while (transform.position != dest)
        {
            transform.position = transform.position + new Vector3 (
                transform.position.x + dist_x / (dist_x + dist_z),
                transform.position.y,
                transform.position.z + dist_z / (dist_x + dist_z)
            ) * Time.deltaTime;
        }      
    }
}
