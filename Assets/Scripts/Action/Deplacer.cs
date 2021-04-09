using System;
using UnityEngine;


public class Deplacer : MonoBehaviour
{
    // public float x, y, z;
    public Vector3 dest;

    // Le CharacterControl permet de lui dire si le personnage est occupé ou non 
    CharacterControl characterControl;
    
    public Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        characterControl = gameObject.GetComponent<CharacterControl>();
    }

    // Update is called once per frame
    void Update()
    {
        // On vérifie si on est au bon endroit sinon on se déplace
        if (!characterControl.IsNextToMe(dest))
        {
            // Si le personnage n'était pas occupé à la dernière itération alors il devient occupé sinon on ne fait rien car il est déjà
            if(!characterControl.GetIsOccupied()){
                characterControl.IsOccupied();
            }
            //désactivation du visuel des objets associés aux Idles randoms
            
            animator.Play("Walk");
            deplacer (dest);
        }
        else{
            // Si le personnage était occupé à la dernière itération alors il ne devient plus occupé sinon on ne fait rien car il est déjà
            if(characterControl.GetIsOccupied()){
                characterControl.IsNotOccupied();
            }
        }
    }

    // fonction de déplacement d'une unité de distance vers la destination voulue
    public void deplacer (Vector3 dest)
    {
        float dist_x = dest.x - transform.position.x;
        float dist_z = dest.z - transform.position.z;

        //regard vers la destination
        transform.LookAt(new Vector3(dest.x, transform.position.y, dest.z));

        Vector3 avancement = Vector3.Normalize (new Vector3 (dist_x, 0,dist_z)) * Time.deltaTime * 3;
        transform.position = transform.position + avancement;
    }

    
}
