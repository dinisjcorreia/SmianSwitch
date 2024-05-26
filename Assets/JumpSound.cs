using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpSound : StateMachineBehaviour
{
    private bool hasPlayedEnterSound = false;
    private bool hasPlayedExitSound = false;

    // Reference to the AudioManager
    private AudioManager audioManager;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!hasPlayedEnterSound)
        {
            if (audioManager == null)
            {
                audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
            }

            audioManager?.PlaySound(audioManager.jumpSound);
            hasPlayedEnterSound = true;
            hasPlayedExitSound = false; // Reset exit flag for the next exit transition
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!hasPlayedExitSound)
        {
            if (audioManager == null)
            {
                audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
            }

            audioManager?.PlaySound(audioManager.hitSound);
            hasPlayedExitSound = true;
            hasPlayedEnterSound = false; // Reset enter flag for the next enter transition
        }
    }
}
