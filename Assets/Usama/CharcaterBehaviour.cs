using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CharcaterBehaviour : MonoBehaviour
{
    public List<GameObject> CharcaterHoldingItem = new List<GameObject>();
    private NavMeshAgent agent;
    public int ItemToPurchase = 0;
    public int SelectedItem = 0;
    private Animator CharcaterAnimator;
    private Transform assignedCashierPosition;
    CashMachine nearestCashier;
    private bool hasCoroutineStarted = false;
    public Vector3 initialPosition;
    private bool waitingForTurn = false;
    public GameObject Player;
    public GameObject Money;
    private PedesterienIKA IKAAnim;
    private void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        ItemToPurchase = Random.Range(1, 6);
        SelectedItem = 0;
        IKAAnim = GetComponent<PedesterienIKA>();
        CharcaterAnimator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        initialPosition = transform.position;
        Money.SetActive(false);
    }

    public void FindCashier()
    {
        StartCoroutine(HandleCustomerPayments());
    }

    private bool WaitToPayCash = true;
    public IEnumerator HandleCustomerPayments()
    {
        while (WaitToPayCash)
        {
            nearestCashier = FindObjectOfType<CashMachine>();

            if (nearestCashier != null)
            {
                Transform availablePosition = nearestCashier.GetAvailablePosition();
                if (availablePosition != null)
                {
                    if (!nearestCashier.waitingCustomers.Contains(this))
                    {
                        nearestCashier.waitingCustomers.Add(this);
                    }
                    WaitToPayCash = false;

                    // Move to the assigned cashier position
                    assignedCashierPosition = availablePosition;
                    agent.enabled = true;
                    MoveToPosition(assignedCashierPosition,false);
                }
                else
                {
                    // If no position available, wander around randomly in the store
                    StartCoroutine(WanderInStore());
                }
            }
            else
            {
                Debug.Log("No cashier found.");
            }

            yield return new WaitForSeconds(1f);
        }
    }

    public void MoveToPosition(Transform position, bool  bill)
    {
        if (agent != null && position != null)
        {
            if (bill == true)
            {
                agent.speed = 0.8f;
            }
            else
            {
                agent.speed =1.5f;
            }
            agent.enabled = true;
            agent.stoppingDistance = 0.15f;
            agent.SetDestination(position.position);
            CharcaterAnimator.Play("Walk_Bag");
            StartCoroutine(WaitForCustomerToReach(agent, position));
        }
    }

    private IEnumerator WaitForCustomerToReach(NavMeshAgent agent, Transform TargetPosition)
    {
        while (agent.pathPending || agent.remainingDistance > agent.stoppingDistance)
        {
            yield return null;
        }

        CharcaterAnimator.Play("Idle_Bag");
        agent.transform.position = TargetPosition.position;
        agent.enabled = false;
        if (Player != null)
        {
           // StartCoroutine(RotateCustomerTowardsCasher(Player.transform));
        }
        // Only the first customer in line can pay
        if (nearestCashier.waitingCustomers[0].transform == transform)
        {
            waitingForTurn = true;
            CompletePurchase();
        }
    }

    public void CompletePurchase()
    {
        for (int a = 0; a < CharcaterHoldingItem.Count; a++)
        {
            CharcaterHoldingItem[a].transform.position = nearestCashier.Itemplacement[a].transform.position;
            CharcaterHoldingItem[a].gameObject.GetComponent<BoxCollider>().enabled = true;
            CharcaterHoldingItem[a].SetActive(true);
        }
        if (!hasCoroutineStarted)
        {
            StartCoroutine(WaitForBill());
        }
    }

    private IEnumerator WaitForBill()
    {
        hasCoroutineStarted = true;

        while (CharcaterHoldingItem.Count > 0)
        {
            yield return null;
        }
        if (Player != null)
        {
            //StartCoroutine(RotateCustomerTowardsCasher(Player.transform));
        }
        yield return new WaitForSeconds(0.5f);
        Debug.Log("Play Animation Pay bill");
        Money.SetActive(true);
        CharcaterAnimator.Play("Paying");
        yield return new WaitForSeconds(0.2f);
        IKAAnim.rightHandObj = transform.GetChild(2).transform;
        Money.SetActive(true);
    }

    public void MoveBack()
    {
        // After paying, move away
        nearestCashier.MoveCharacterForward();
        hasCoroutineStarted = false;
        agent.enabled = true;
        agent.SetDestination(initialPosition);
        StartCoroutine(MoveCustomerBack(agent));
        CharcaterAnimator.Play("Walk_Bag");
    }
    private IEnumerator MoveCustomerBack(NavMeshAgent agent)
    {
        while (agent.pathPending || agent.remainingDistance > agent.stoppingDistance)
        {
            yield return null;
        }
        Destroy(gameObject);
    }
    private IEnumerator RotateCustomerTowardsCasher(Transform target)
    {
        // Calculate direction to the target
        Vector3 directionToTarget = target.position - transform.position;
        // directionToTarget.y = 0; // Ignore vertical difference for smoother rotation

        // Get the target rotation based on the direction
        Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);

        float timeElapsed = 0f;  // Track elapsed time
        float maxDuration = 2f;  // Stop the coroutine after 5 seconds

        // Rotate until the character is facing the target or time runs out
        while (Quaternion.Angle(transform.rotation, targetRotation) > 0.1f && timeElapsed < maxDuration)
        {
            // Smoothly rotate the character towards the target
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f); // Adjust the speed factor (5f) for faster rotation

            // Update the elapsed time
            timeElapsed += Time.deltaTime;

            // Wait for the next frame before continuing the rotation
            yield return null;
        }

        // Ensure that the rotation is set exactly to the target rotation at the end if time didn't run out
        if (timeElapsed < maxDuration)
        {
            GetComponent<NavMeshAgent>().enabled = true;
        }
    }


    // Wander randomly if no cashier available
    private IEnumerator WanderInStore()
    {
        while (WaitToPayCash)
        {
            Vector3 randomDirection = Random.insideUnitSphere * 5f;
            randomDirection += transform.position;
            NavMeshHit hit;
            NavMesh.SamplePosition(randomDirection, out hit, 5f, 1);
            agent.enabled = true;
            agent.SetDestination(hit.position);
            CharcaterAnimator.Play("Walk_Bag");

            yield return new WaitForSeconds(5f); // Wander for a bit before checking for cashier again
        }
    }
}
