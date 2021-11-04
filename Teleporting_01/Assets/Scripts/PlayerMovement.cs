using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;
    public Animator characterAnimator, teleportingAnimator;
    public List<Transform> targets;
    
    public float speed = 3f, gravity = -9.81f;
    int index;
    bool teleport, walkable;

    Vector3 velocity;

    // Start is called before the first frame update
    void Start()
    {
        teleportingAnimator = GetComponent<Animator>();
        index = 1;
        teleport = false;
        walkable = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (walkable)
        {
            if (Input.GetKey(KeyCode.UpArrow))
            {
                characterAnimator.SetBool("IsWalking", true);
                float z = Input.GetAxis("Vertical");

                Vector3 move = /* transform.right * x +*/ transform.forward * z;
                controller.Move(move * speed * Time.deltaTime);
            }
            else if (Input.GetKeyUp(KeyCode.UpArrow))
            {
                characterAnimator.SetBool("IsWalking", false);
            }

            velocity.y += gravity * Time.deltaTime;
            controller.Move(velocity * Time.deltaTime);
        }

        if(Input.GetKeyDown(KeyCode.Space) && teleport)
        {
            StartCoroutine(Teleport());
            IEnumerator Teleport()
            {
                if (index == 6) index = 1;
                teleportingAnimator.SetInteger("Status", index % 5 + 1);
                walkable = false;
                yield return new WaitForSeconds(3f);
                controller.transform.position = targets[index % 5].position;
                index++;
                teleportingAnimator.SetTrigger("Located");
                yield return new WaitForSeconds(1f);
                walkable = true;
                teleportingAnimator.SetInteger("Status", 0);
            }
        }
    }

    /*
    void ShrinkBeforeTeleport()
    {
        mode = TELEPORT;
        if (index == 4) index = 0;
        teleportingAnimator.SetInteger("Status", ++index);
    } */

    void OnTriggerEnter(Collider other)
    {
        teleport = true;
    }

    void OnTriggerExit(Collider other)
    {
        teleport = false;
    }
}
