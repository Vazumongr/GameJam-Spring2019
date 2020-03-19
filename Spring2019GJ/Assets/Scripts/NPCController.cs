using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPCController : MonoBehaviour {

    private List<string> greetings;
    private List<string> tasks;

    public TextAsset greetTextAsset;
    public TextAsset tasksTextAsset;
    public TextMesh speechText;

    private string quest;
    private string state;
    private bool engaged;
    private bool taskGiven;
    private bool completed;
    
    private int questID;

    
    public Camera cam;
    public GameObject player;

    // Use this for initialization
    void Start()
    {
        greetings = new List<string>();
        tasks = new List<string>();
        readGreetingsFile();
        readTasksFile();
        engaged = taskGiven = completed = false;
        speechText.GetComponent<Transform>();
        speechText.text = "";


    }//End Start

    // Update is called once per frame
    void Update() {
        
        speechText.GetComponent<Transform>().LookAt(cam.GetComponent<Transform>());
    }//End Update

    private void OnTriggerEnter(Collider other)
    {
        speechText.text = greetings[UnityEngine.Random.Range(0, greetings.Count)];
        if (!completed)
        {
            if (!taskGiven)
                state = "waitingToOfferTask";
            else
                state = "waitingToAsk";
        }
    }

    private void OnTriggerStay(Collider other)
    {
        //If player doesn't have a task
        if (true)
        {
            switch (state)
            {
                case "waitingToOfferTask":
                    if (Input.GetKeyDown(KeyCode.Space))
                    {
                        //Debug.Log(tasks[UnityEngine.Random.Range(0, tasks.Count)]);
                        questID = UnityEngine.Random.Range(0, tasks.Count);
                        while(player.GetComponent<PlayerQuestLog>().checkQuestActive(questID) || player.GetComponent<PlayerQuestLog>().checkQuestCompleted(questID))
                            questID = UnityEngine.Random.Range(0, tasks.Count);

                        if (!player.GetComponent<PlayerQuestLog>().checkQuestActive(questID))
                        {
                            quest = tasks[questID];
                            speechText.text = quest
                                + "\n Y or N";
                            state = "awaitingReply";
                        }
                    }
                    break;
                case "awaitingReply":
                    if (Input.GetKeyDown(KeyCode.Y))
                    {
                        speechText.text = "Thank you so much! Buh Bye!";
                        taskGiven = true;
                        state = "";
                        player.GetComponent<PlayerQuestLog>().setQuestActive(questID);
                        Debug.Log(player.GetComponent<PlayerQuestLog>().checkQuestActive(questID));
                    }
                    else if (Input.GetKeyDown(KeyCode.N))
                    {
                        speechText.text = "It's not like I wanted you to anyways. BAKA!.";
                        taskGiven = false;
                        state = "";
                    }
                    break;
                case "waitingToAsk":
                    if (Input.GetKeyDown(KeyCode.Space))
                    {
                        speechText.text = "Have you finished that task yet? \n Y or N";
                        state = "awaitingToComplete";
                    }
                    break;

                case "awaitingToComplete":
                    if (Input.GetKeyDown(KeyCode.Y))
                    {
                        if (player.GetComponent<PlayerQuestLog>().checkQuestCompleted(questID))
                        {
                            if(questID==1)
                                player.GetComponent<PlayerController>().changeMoney(-8);

                            speechText.text = "Thank you so much! \n Heres your reward!!";
                            taskGiven = false;
                            player.GetComponent<PlayerController>().changeMoney(6);
                            Debug.Log(player.GetComponent<PlayerController>().getPlayerCoins());
                            completed = true;
                            state = "";
                        }
                        else
                            speechText.text = "What the fuck did you just fucking \n " +
                                "say about me, you little bitch? \n  " +
                                "I'll have you know I graduated top of \n " +
                                "my class in the Navy Seals, and I've been \n " +
                                "involved in numerous secret raids on Al-Quaeda, \n " +
                                "and I have over 300 confirmed kills. I am trained \n " +
                                "in gorilla warfare and I'm the top sniper in the \n " +
                                "entire US armed forces. You are nothing to me but \n " +
                                "just another target. I will wipe you the fuck out \n " +
                                "with precision the likes of which has never been seen \n " +
                                "before on this Earth, mark my fucking words. \n " +
                                "You think you can get away with saying that shit \n " +
                                "to me over the Internet? Think again, fucker. As \n " +
                                "we speak I am contacting my secret network of spies \n " +
                                " across the USA and your IP is being traced right \n " +
                                "now so you better prepare for the storm, maggot. \n " +
                                "The storm that wipes out the pathetic little thing \n " +
                                "you call your life. You're fucking dead, kid.";
                        Debug.Log(player.GetComponent<PlayerQuestLog>().checkQuestCompleted(questID));


                    }
                    else if (Input.GetKeyDown(KeyCode.N))
                    {
                        speechText.text = "It's not like I wanted you to anyways. BAKA!.";
                        taskGiven = true;
                        state = "";
                    }
                    break;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        speechText.text = "";
        engaged = false;
        state = "";
        if(questID == 3)
        {
            player.GetComponent<PlayerQuestLog>().setLeaveMe();
        }
    }

    

    void readGreetingsFile()
    {
        string temp = greetTextAsset.ToString();
        string[] remo = temp.Split(new string[] { "\n" }, StringSplitOptions.None);
        for(int i = 0; i < remo.Length; i++)
        {
            greetings.Add(remo[i]);
        }
    }//End readGreetingsFile

    void readTasksFile()
    {
        string temp = tasksTextAsset.ToString();
        string[] remo = temp.Split(new string[] { "\n" }, StringSplitOptions.None);
        for (int i = 0; i < remo.Length; i++)
        {
            if(remo[i].Contains("nn")||true)
            {
                remo[i].Replace("nn", "\n");
            }
            tasks.Add(remo[i].Replace("nn", "\n"));
        }
    }//End readTasksFile
}
