using System;
using UnityEngine;

public class Deplacer : MonoBehaviour
{
    private static float DELTA_POS = 0.01F;

    public float x, y, z;
    private Vector3 dest;

    // Start is called before the first frame update
    void Start()
    {
        dest = new Vector3 (x, y, z);
    }

    // Update is called once per frame
    void Update()
    {
        // Deplacement
        if (Math.Abs (transform.position.x - dest.x) > DELTA_POS ||
            Math.Abs (transform.position.z - dest.z) > DELTA_POS)
        {
            deplacer (dest);
        }
    }

    public void deplacer (Vector3 dest)
    {
        float dist_x = dest.x - transform.position.x;
        float dist_z = dest.z - transform.position.z;

        transform.Translate (new Vector3 (
            dist_x / Math.Abs (dist_x + dist_z),
            0,
            dist_z / Math.Abs (dist_x + dist_z)
        ) * Time.deltaTime * 4);
    }
}
