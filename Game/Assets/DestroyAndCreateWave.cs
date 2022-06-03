using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAndCreateWave : StateMachineBehaviour
{
    public GameObject wavePrefab;
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        var transform = animator.gameObject.transform.position;
        Destroy(animator.gameObject, stateInfo.length);
        GameObject wave = Instantiate(wavePrefab) as GameObject;
        wave.transform.position = animator.gameObject.transform.position;
    }
}
