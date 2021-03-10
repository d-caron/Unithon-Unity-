using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Talk : MonoBehaviour
{
    public Animator animator;
    public GameObject partner;
    private bool discussionNotStarted;
    public int isTalking;
    private bool noTalk;
    // Start is called before the first frame update
    void Start()
    {
        //partner = null;
        discussionNotStarted = true;
        isTalking = 0;
        noTalk = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(partner != null){
            if (GetComponent<Deplacer>().IsNextToMe(partner.transform.position)){
                if(discussionNotStarted){
                    isTalking = WhoStartDiscussion();
                    discussionNotStarted = false;
                }
                GetComponent<Deplacer>().isTalking = true;
                // 0 = ne parle pas ; 1 = va/entrain de parler ; 2 = a fini de parler
                switch(partner.GetComponent<Talk>().isTalking, isTalking){
                    case (0,0) : //impossible
                        break; 
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
                    case (1,1) : //impossible
                        break;
                    case (1,2) : //impossible
                        break;
                    case (2,0) : 
                        animator.Play("Idle");
                        break;
                    case (2,1) : //impossible
                        break;
                    case (2,2) : //impossible
                        break;
                }
            }
            else
            {
                GetComponent<Deplacer>().dest = partner.transform.position;
            }
        }
    }

    public void EndDiscussion(){
        partner.GetComponent<Deplacer>().isTalking = false;
        GetComponent<Deplacer>().isTalking = false;
        partner.GetComponent<Talk>().partner = null;
        partner = null;
        discussionNotStarted = true;
        isTalking = 0;
        noTalk = true;
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
