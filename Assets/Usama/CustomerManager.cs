using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class CustomerManager : MonoBehaviour
{
    public List<GameObject> customerPrefabs;    // List of customer prefabs
    public Transform[] entryPoints;             // Entry points for customers
    public List<Transform> shelvesItem;                 // List of available shelves in the shop
    public List<Transform> shelves = new List<Transform>();   // List of available shelves in the shop
    public float customerSpawnDelay = 5f;       // Time delay between spawning customers
    public List<AnimationClip> AnimClip = new List<AnimationClip>();
    public Transform Destination;
    public DayTimeManager DayTime;
    public bool ShopOpen = false;
    public List<GameObject> InstantiatedObj= new List<GameObject>();
    private Coroutine spawnCustomerCoroutine;
    public GameObject ShowMessage;
    public Text ShowMessageTEXT;
    private void Start()
    {
        // Find all objects of type ShelfParent
        ShelfParent[] shelfParents = GameObject.FindObjectsOfType<ShelfParent>();

        // Populate the shelves list with their Transforms
        foreach (ShelfParent shelf in shelfParents)
        {
            shelves.Add(shelf.transform);
        }
        StartCoroutine(StartSpawnCharacterCoroutine());
    }

    private IEnumerator StartSpawnCharacterCoroutine()
    {
        while (true)
        {
            // If the shop is open but the coroutine for spawning hasn't started, start it
            if (DayTime.open && !ShopOpen)
            {
                ShopOpen = true;
                spawnCustomerCoroutine = StartCoroutine(SpawnCustomer());
            }
            // If the shop is closed and customers are being spawned, stop the coroutine
            else if (!DayTime.open && ShopOpen)
            {
                ShopOpen = false;
                if (spawnCustomerCoroutine != null)
                {
                    StopCoroutine(spawnCustomerCoroutine);
                    spawnCustomerCoroutine = null;
                }
            }

            // Check again every 5 seconds
            yield return new WaitForSeconds(5f);
        }
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
                InstantiatedObj.Add(customer);
                // Move the customer to the assigned shelf position using NavMesh
                NavMeshAgent agent = customer.GetComponent<NavMeshAgent>();
                if (agent != null)
                {
                    agent.enabled = true;
                    agent.SetDestination(availableShelf.position);
                    agent.stoppingDistance = 1f;
                    agent.speed = 1.5f;
                    
                }
                if (shelvesItem.Contains(availableShelf))
                {
                    StartCoroutine(WaitForCustomerToReach(agent, availableShelf, customer));
                }
                else
                {

                    StartCoroutine(CustomerCoroutine(agent, Destination.position));

                }
            }
        
            // Wait before spawning the next customer
            yield return new WaitForSeconds(customerSpawnDelay);
        }
    }
    
    
    // Coroutine for customer behavior if there is no item in the shop 
    public IEnumerator CustomerCoroutine(NavMeshAgent agent, Vector3 destinationPosition)
    {
        // Store the initial position
        Vector3 initialPosition = agent.transform.position;
        //agent.stoppingDistance = 1.3f;
        // Wait until the agent reaches the destination
        bool _isMoving = true;
        while (_isMoving)
        {
            if (agent.hasPath && agent.remainingDistance < 1)
            { _isMoving = false; //print("  " + agent.remainingDistance);
             }
            yield return null;  // Wait for the next frame
        }
    // "Dismissing"
        // Once at the destination, wander around for 15 seconds
        // yield return WanderForSeconds(4,agent);
        Animator animator = agent.gameObject.GetComponent<Animator>(); // Assuming customer has an Animator component
        animator.Play("Dismissing"); // Play Idle animation
        yield return new WaitForSeconds(1.5f);

        // Return to the initial position
        agent.stoppingDistance = 1f;
        agent.SetDestination(initialPosition);
        yield return  new WaitForSeconds(0.1f);
        InstantiatedObj.Remove(agent.gameObject);
        animator.Play("Walk"); // Play Idle animation
        yield return  new WaitForSeconds(0.5f);
        // Wait until the agent returns to the initial position
        while (!agent.pathPending && agent.remainingDistance > agent.stoppingDistance)
        {
            yield return null;
        }

        // Destroy the agent after reaching the initial position
        Destroy(agent.gameObject);
    }

    private IEnumerator WanderForSeconds(float seconds ,NavMeshAgent agent)
    {
        float timeWandering = 0f;

        while (timeWandering < seconds)
        {
            // Generate a random position within a certain range for wandering
            Vector3 wanderPosition = GetRandomPointInStore(agent);

            // Move the agent to the random position
            agent.SetDestination(wanderPosition);
            yield return new WaitForSeconds(1);
            // Wait until the agent reaches the wander position or for a second to simulate wandering
            while (!agent.pathPending && agent.remainingDistance > agent.stoppingDistance)
            {
                yield return null;  // Wait for next frame
            }

            // Accumulate wandering time
            timeWandering += 1f;  // Adjust how often the agent moves to new positions
        }
    }
    // Method to get a random position in the store for wandering
    private Vector3 GetRandomPointInStore( NavMeshAgent agent)
    {
        // Define a range or bounds for the store area
        float range = 10f;
        Vector3 randomPoint = new Vector3(
            Random.Range(-range, range),
            0,
            Random.Range(-range, range)
        );

        // Adjust the point to be relative to the store area, e.g., add offsets if necessary
        return randomPoint + agent.transform.position;
    }

    private Transform GetAvailableItem()
    {
        if (InstantiatedObj.Count <4)
        {

            if (shelvesItem.Count == 0 && shelves.Count != 0)
            {
                int shelvess = Random.Range(0, shelves.Count);
                return shelves[shelvess];
            }

            if (shelvesItem.Count == 0)
            {
                return Destination;
            }

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
        yield return new WaitForSeconds(0.5f);
        item unitPrice = item.GetComponent<item>();
       // ShelfPlacement GrossPrice = item.transform.parent.transform.parent.GetComponent<ShelfPlacement>();
        Animator animator = customer.GetComponent<Animator>(); // Assuming customer has an Animator component
        if (unitPrice.Unit_Price != unitPrice.Gross_Price)
        {
            Transform objChild = customer.transform.GetChild(1).transform;
            // Calculate the direction vector from the child's position to the item to Play Animation According to it 
            Vector3 directionToItem = item.position - objChild.position;
            float yDifference = directionToItem.y; // Check the vertical distance (up or down)
            float zDifference = directionToItem.z; // Check the forward distance
            float threshold = 0.1f; // Adjust this as needed
            AnimationClip anim = null;
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
            animator.Play("Idle"); // Play Idle animation
            yield return new WaitForSeconds(1f);
            CharcaterBehaviour charcater = customer.GetComponent<CharcaterBehaviour>();
            item.gameObject.SetActive(false);
            shelvesItem.Remove(item);
            GameObject itemtohold = Instantiate(item.gameObject);
            Destroy(item.gameObject);
            itemtohold.SetActive(false);
            itemtohold.tag = "Item";
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
                    yield return new WaitForFixedUpdate();
                    // Wait until the customer reaches the next shelf
                    yield return WaitForCustomerToReach(agent, nextShelf, customer);
                }
                else
                {
                    yield return null;
                }
            }
            //StopCoroutine(RotateCustomerTowardsItem(agent.transform, item));
            customer.GetComponent<CharcaterBehaviour>().FindCashier();
        }
        else
        {
            agent.enabled = false;
            animator.Play("Dismissing");
            yield return new WaitForSeconds(0.5f);
            ShowMessageTEXT.text = "Attention:  Set the price for" + unitPrice.Name+ ",\n customers can't buy without it!";
            ShowMessage.SetActive(true);
            yield return new WaitForSeconds(1.5f);
            agent.enabled = true;
            agent.SetDestination(agent.GetComponent<CharcaterBehaviour>().initialPosition);
            animator.Play("Walk");
            StartCoroutine(MOveplayerbackToInitialPosition(agent));
        }
    }

    public IEnumerator MOveplayerbackToInitialPosition(NavMeshAgent agent)
    {
        InstantiatedObj.Remove(agent.gameObject);
        //agent.stoppingDistance = 1f;
        // Wait until the agent reaches the destination
        bool _isMoving = true;
        while (_isMoving)
        {
            if (agent.hasPath && agent.remainingDistance < 1)
            {
                _isMoving = false; //print("  " + agent.remainingDistance);
            }
            yield return null;  // Wait for the next frame
        } 
           
        // Destroy the agent after reaching the initial position
        Destroy(agent.gameObject);
    }

    private IEnumerator RotateCustomerTowardsItem(Transform customerTransform, Transform itemTransform)
    {
        Vector3 directionToItem = itemTransform.position - customerTransform.position;
        directionToItem.y = 0; // Ignore vertical difference

        Quaternion targetRotation = Quaternion.LookRotation(directionToItem);

        float timeElapsed = 0f;
        float maxDuration = 2f;  // Maximum duration of 5 seconds

        while (Quaternion.Angle(customerTransform.rotation, targetRotation) > 0.01f && timeElapsed < maxDuration)
        {
            customerTransform.rotation = Quaternion.Slerp(customerTransform.rotation, targetRotation, Time.deltaTime * 2f);
            timeElapsed += Time.deltaTime;  // Track the time elapsed
            yield return null; // Wait for the next frame
        }
    }


}
