using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Talk : MonoBehaviour
{
    public Animator animator;
    public GameObject partner;
    private bool discussionNotStarted = true;
    public int isTalking = 0;
    private bool noTalk = true;

    private CharacterControl characterControl;

    // Start is called before the first frame update
    void Start()
    {
        //partner = null;
        isTalking = 0;

        this.characterControl = GetComponent<CharacterControl>();
    }

    // Update is called once per frame
    void Update()
    {
        if(partner != null){
                if(discussionNotStarted){
                    transform.LookAt(new Vector3(partner.transform.position.x, transform.position.y, partner.transform.position.z));
                    partner.transform.LookAt(new Vector3(transform.position.x, partner.transform.position.y, transform.position.z));
                    isTalking = WhoStartDiscussion();
                    discussionNotStarted = false;
                }
                
                // 0 = ne parle pas ; 1 = va/entrain de parler ; 2 = a fini de parler
                switch(partner.GetComponent<Talk>().isTalking, isTalking){
                    case (0,1) :
                        if(noTalk){
                            animator.Play("Talk");
                            noTalk = false;
                        }
                        else{
                            if(animator.GetCurrentAnimatorClipInfo(0)[0].clip.name == "Idle_Generic"){
                                isTalking = 2;
                            }
                        }
                        break;
                    case (0,2) :
                        partner.GetComponent<Talk>().isTalking = 1;
                        isTalking = 0;
                        noTalk = true;
                        break;
                    case (1,0) : 
                        animator.Play("Idle");
                        break;
                    case (2,0) : 
                        animator.Play("Idle");
                        break;
                    // Si on est dans un cas impossible, on force un cas possible, corrige les problèmes d'états
                    default:
                        partner.GetComponent<Talk>().isTalking = 0;
                        isTalking = 1;
                        break;
                }
        }
    }

    public void IsFinish(){
        if (partner != null) {
            GameObject partnerSave = partner;
            partner = null;
            partnerSave.GetComponent<Talk>().IsFinish();
        }
        if(characterControl.isOccupied && characterControl.GetCurrentCommandAction().Equals("discuter")) {
            characterControl.IsNotOccupied();
        }
        
        isTalking = 0;
        noTalk = true;
        discussionNotStarted = true;
    }

    private int WhoStartDiscussion(){
        if(string.Compare(partner.name, name)>0){
            return 0;
        }
        else{
            return 1;
        }
    }
}
