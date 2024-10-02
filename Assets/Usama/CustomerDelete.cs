using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CustomerDelete : MonoBehaviour
{
    public List<GameObject> customerPrefabs;    // List of customer prefabs
    public Transform[] entryPoints;             // Entry points for customers
    public List<Transform> shelvesItem;                 // List of available shelves in the shop
    public float customerSpawnDelay = 5f;       // Time delay between spawning customers
    public List<AnimationClip> AnimClip = new List<AnimationClip>();
    private void Start()
    {
        StartCoroutine(SpawnCustomer());
    }
    
    private IEnumerator SpawnCustomer()
    {
        while (true)
        {
            // Check if any shelf has an available space
            Transform availableShelf = GetAvailableItem();

            if (availableShelf != null && customerPrefabs.Count > 0 && entryPoints.Length > 0)
            {
                // Randomly select a customer prefab
                GameObject customerPrefab = customerPrefabs[Random.Range(0, customerPrefabs.Count)];

                // Randomly select one of the entry points
                Transform entryPoint = entryPoints[Random.Range(0, entryPoints.Length)];

                // Instantiate the customer at the random entry point
                GameObject customer = Instantiate(customerPrefab, entryPoint.position, Quaternion.identity);

                if (availableShelf.position != Vector3.zero)
                {
                    // Move the customer to the assigned shelf position using NavMesh
                    NavMeshAgent agent = customer.GetComponent<NavMeshAgent>();
                    if (agent != null)
                    {
                        agent.enabled = true;
                        agent.SetDestination(availableShelf.position);
                        agent.stoppingDistance = 1f;
                        agent.speed = 1.5f;
                        StartCoroutine(WaitForCustomerToReach(agent, availableShelf, customer));
                    }
                }
            }
            // Wait before spawning the next customer
            yield return new WaitForSeconds(customerSpawnDelay);
        }
    }
    private Transform GetAvailableItem()
    {
        if (shelvesItem.Count == 0)
            return null; // No items available
        // Try to find an unreserved item
        for (int i = 0; i < shelvesItem.Count; i++)
        {
            int randomIndex = Random.Range(0, shelvesItem.Count); // Get a random index
            item itemData = shelvesItem[randomIndex].GetComponent<item>();
            if (itemData != null && !itemData.Reserved) // Check if the item is not reserved
            {
                itemData.Reserved = true;
                return shelvesItem[randomIndex]; // Return the available item
            }
        }
        return null; // No unreserved items found
    }


    private IEnumerator WaitForCustomerToReach(NavMeshAgent agent, Transform item, GameObject customer)
    {
        // Wait until the customer reaches the destination
        while (agent.pathPending || agent.remainingDistance > agent.stoppingDistance)
        {
            yield return null;
        }

        // Stop the customer and occupy the shelf
        agent.enabled = false;

        // Rotate customer towards the item
        StartCoroutine(RotateCustomerTowardsItem(agent.transform, item));

        Transform objChild = customer.transform.GetChild(1).transform;
        // Calculate the direction vector from the child's position to the item to Play Animation According to it 
        Vector3 directionToItem = item.position - objChild.position;
        float yDifference = directionToItem.y; // Check the vertical distance (up or down)
        float zDifference = directionToItem.z; // Check the forward distance
        float threshold = 0.1f; // Adjust this as needed

        // Play different animations based on the relative position of the item
        Animator animator = customer.GetComponent<Animator>(); // Assuming customer has an Animator component
        AnimationClip anim=null;
        if (animator != null)
        {
            if (yDifference > threshold) // Item is above the character
            {
                animator.Play("Grab"); // Play up animation
                anim = AnimClip[0];
            }
            else if (yDifference < -threshold) // Item is below the character
            {
                animator.Play("Kneeling"); // Play down animation
                anim = AnimClip[2];
            }
            else if (Mathf.Abs(zDifference) > threshold) // Item is in front of the character
            {
                animator.Play("Grab2"); // Play front animation
                anim = AnimClip[1];
            }
            else
            {
                anim = AnimClip[0];
            }
        }
        yield return new WaitForSeconds(anim.length);
        item.gameObject.SetActive(false);
        shelvesItem.Remove(item);
        GameObject itemtohold = Instantiate(item.gameObject);
        Destroy(item.gameObject);
        itemtohold.SetActive(false);
        animator.Play("Idle"); // Play Idle animation
        yield return new WaitForSeconds(1f);
        CharcaterBehaviour charcater = customer.GetComponent<CharcaterBehaviour>();
        itemtohold.transform.parent = transform;
        charcater.SelectedItem++;
        charcater.CharcaterHoldingItem.Add(itemtohold);
        if (charcater.SelectedItem < charcater.ItemToPurchase)
        {
            // After finishing the action, check for another available shelf
            Transform nextShelf = GetAvailableItem();
            if (nextShelf != null)
            {
                Vector3 nextPosition = nextShelf.position;
                // Move to the next available shelf
                agent.enabled = true;
                agent.SetDestination(nextPosition);
                animator.Play("Walk"); // Play Idle animation
                // Wait until the customer reaches the next shelf
                yield return WaitForCustomerToReach(agent, nextShelf, customer);
            }
            else
            {
                yield return null;
            }
        }

        customer.GetComponent<CharcaterBehaviour>().FindCashier();
    }

    private IEnumerator RotateCustomerTowardsItem(Transform customerTransform, Transform itemTransform)
    {
        Vector3 directionToItem = itemTransform.position - customerTransform.position;
        directionToItem.y = 0; // Ignore vertical difference

        Quaternion targetRotation = Quaternion.LookRotation(directionToItem);

        while (Quaternion.Angle(customerTransform.rotation, targetRotation) > 0.01f)
        {
            customerTransform.rotation = Quaternion.Slerp(customerTransform.rotation, targetRotation, Time.deltaTime * 2f);
            yield return null; // Wait for the next frame
        }
    }
}
