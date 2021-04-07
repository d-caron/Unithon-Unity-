using System;
using UnityEngine;


public class Deplacer : MonoBehaviour
{
    private static float DELTA_POS = 1.0F;
    public Animator animator;
    public bool isTalking;
    public float timerRandomIdle = 5.0f;
    public GameObject phone;
    public GameObject micro;

    // public float x, y, z;
    public Vector3 dest;

    // Le CharacterControl permet de lui dire si le personnage est occupé ou non 
    CharacterControl characterControl;

    // Start is called before the first frame update
    void Start()
    {
        characterControl = gameObject.GetComponent<CharacterControl>();
        dest = transform.position;
        isTalking = false;
        //désactivation du visuel des objets associés aux Idles randoms
        micro.SetActive(false);
        phone.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        // On vérifie si on est au bon endroit sinon on se déplace
        if (!IsNextToMe(dest))
        {
            // Si le personnage n'était pas occupé à la dernière itération alors il devient occupé sinon on ne fait rien car il est déjà
            if(!characterControl.GetIsOccupied()){
                characterControl.IsOccupied();
            }
            timerRandomIdle = 5.0f;
            //désactivation du visuel des objets associés aux Idles randoms
            micro.SetActive(false);
            phone.SetActive(false);
            animator.Play("Walk");
            deplacer (dest);
        }
        else{
            // Si le personnage était occupé à la dernière itération alors il ne devient plus occupé sinon on ne fait rien car il est déjà
            if(characterControl.GetIsOccupied()){
                characterControl.IsNotOccupied();
            }
            if(!isTalking){
                if(timerRandomIdle <=0){
                    RunRandomIdle();
                    timerRandomIdle = 5.0f;
                }
                else{
                    String currentAnimation = animator.GetCurrentAnimatorClipInfo(0)[0].clip.name;
                    if(currentAnimation != "Idle_Generic" && currentAnimation != "Walking" && currentAnimation != "Ninja_Run"){
                        timerRandomIdle = 5.0f;
                    }
                    else{
                        //désactivation du visuel des objets associés aux Idles randoms
                        micro.SetActive(false);
                        phone.SetActive(false);
                        animator.Play("Idle");
                        timerRandomIdle -= Time.deltaTime;
                    }
                }
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

    public bool IsNextToMe(Vector3 to){
        return !((Math.Abs (transform.position.x - to.x) > DELTA_POS) ||
        (Math.Abs (transform.position.z - to.z) > DELTA_POS));
    }

    /*
    args : rien
    do : lance un Ilde aléatoire parmi ceux disponibles
    return : rien*/
    private void RunRandomIdle()
    {
        String[] randomIdles = {"Boxing","Kicking","Jumping","Dancing","Vomiting","Singing","Calling"};
        String choice = randomIdles[UnityEngine.Random.Range(0, randomIdles.Length)];
        animator.Play(choice);
        //active le micro si l'action est de chanter
        if(choice == "Singing"){
            micro.SetActive(true);
        }
        //active le téléphone si l'action est de passer un appel
        if(choice == "Calling"){
            phone.SetActive(true);
        }
    }
}
