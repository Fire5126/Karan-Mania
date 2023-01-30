using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class IntroManager : MonoBehaviour
{
    [SerializeField] private GameObject KarenSpeechBubble;
    [SerializeField] private GameObject SupervisorSpeechBubble;
    [SerializeField] private GameObject Karen;
    [SerializeField] private GameObject Supervisor;

    // Start is called before the first frame update
    void Start()
    {
        //Karen.GetComponent<Animator>().Play("");
        //Supervisor.GetComponentInChildren<Animator>().Play("");
        //KarenSpeechBubble.SetActive(true);
        //SupervisorSpeechBubble.SetActive(true);
    }

    public void GameStartSequence()
    {
        Karen.GetComponent<Animator>().Play("");
        Supervisor.GetComponent<Animator>().Play("");
        KarenSpeechBubble.SetActive(false);
        SupervisorSpeechBubble.SetActive(false);
        Quaternion rot = Quaternion.Euler(0, 0, 90);
        Karen.GetComponent<PlayerAttack>().ToiletPaper(rot);
        Karen.GetComponent<Animator>().Play("");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
