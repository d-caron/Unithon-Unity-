using System;
using UnityEngine;

public class Deplacer : MonoBehaviour
{
    private static float DELTA_POS = 0.8F;
    public Animator animator;

    // public float x, y, z;
    public Vector3 dest;

    // Start is called before the first frame update
    void Start()
    {
        dest = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
        // On vérifie si on est au bon endroit sinon on se déplace
        if (Math.Abs (transform.position.x - dest.x) > DELTA_POS ||
            Math.Abs (transform.position.z - dest.z) > DELTA_POS)
        {
            animator.Play("Walk");
            deplacer (dest);
        }
        else{
            animator.Play("Idle");
        }
    }

    // fonction de déplacement d'une unité de distance vers la destination voulue
    public void deplacer (Vector3 dest)
    {
        float dist_x = dest.x - transform.position.x;
        float dist_z = dest.z - transform.position.z;

        //regard vers la destination
        //transform.LookAt(dest);

        Vector3 avancement = Vector3.Normalize (new Vector3 (dist_x, 0,dist_z)) * Time.deltaTime * 3;
        transform.position = transform.position + avancement;
    }
}
