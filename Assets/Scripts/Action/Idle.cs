using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Idle : MonoBehaviour
{

    public float timerMaxRandomIdle = 5f;

    private float timerRandomIdle;
    public GameObject phone;
    public GameObject micro;
    public Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        //désactivation du visuel des objets associés aux Idles randoms
        micro.SetActive(false);
        phone.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(timerRandomIdle <=0){
            RunRandomIdle();
            resetTimer();
        }
        else{
            String currentAnimation = animator.GetCurrentAnimatorClipInfo(0)[0].clip.name;
            if(currentAnimation != "Idle_Generic" && currentAnimation != "Walking" && currentAnimation != "Ninja_Run"){
                resetTimer();
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

    public void resetTimer(){
        timerRandomIdle = timerMaxRandomIdle;
    }
}
