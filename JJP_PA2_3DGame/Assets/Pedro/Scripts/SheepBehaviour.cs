using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SheepBehaviour : MonoBehaviour
{
    [SerializeField] private float mSpeed = 20f;
    [SerializeField] private float rSpeed = 100f;
    [SerializeField] private int startHP = 100;
    private bool isFinding = false;
    private bool rotatingLeft = false;
    private bool rotatingRight = false;
    private bool isWalking = false;

    Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (startHP <= 0) Destroy(gameObject);

        if (isFinding == false) StartCoroutine(FindPath());

        if(rotatingRight == true) transform.Rotate(transform.up * Time.deltaTime * rSpeed);

        if(rotatingLeft == true) transform.Rotate(transform.up * Time.deltaTime * -rSpeed);

        if (isWalking == true) rb.AddForce(-transform.forward * mSpeed);
    }

    IEnumerator FindPath()
    {
        int rotationTime = Random.Range(1, 3);
        int rotateWait = Random.Range(1, 3);
        int rotateDirection = Random.Range(1, 2);
        int walkWait = Random.Range(1, 3);
        int walkTime = Random.Range(1, 3);

        isFinding = true;

        yield return new WaitForSeconds(walkWait);

        isWalking = true;

        yield return new WaitForSeconds(walkTime);

        isWalking = false;

        yield return new WaitForSeconds(rotateWait);

        if (rotateDirection == 1)
        {
            rotatingLeft = true;
            yield return new WaitForSeconds(rotationTime);
            rotatingLeft = false;
        }
        if (rotateDirection == 2)
        {
            rotatingRight = true;
            yield return new WaitForSeconds(rotationTime);
            rotatingRight = false;
        }

        isFinding = false;
    }

    public void TakeDamage(int hp)
    {
        startHP -= hp;
    }
}
