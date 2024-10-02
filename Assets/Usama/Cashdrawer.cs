using System.Collections;
using UnityEngine;

public class Cashdrawer : MonoBehaviour
{
    private float openPositionX = -0.283f;
    private float closedPositionX = 0.028f;
    private float moveDuration = 0.5f; // Time it takes to open/close the drawer
    private Vector3 initialPosition;

    private void Start()
    {
        // Store the initial local position of the drawer (this is the closed position)
        initialPosition = transform.localPosition;
    }
    public void Draweropen()
    {
        StartCoroutine(OpenDrawer());
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            Draweropen();
        }  if (Input.GetKeyDown(KeyCode.C))
        {
            DrawerClose();
        }
    }
    public void DrawerClose()
    {
        StartCoroutine(CloseDrawer());
    }
    // Coroutine to smoothly open the drawer
    private IEnumerator OpenDrawer()
    {
        Vector3 targetPosition = new Vector3(openPositionX, transform.localPosition.y, transform.localPosition.z);
        float elapsedTime = 0f;

        while (elapsedTime < moveDuration)
        {
            transform.localPosition = Vector3.Lerp(initialPosition, targetPosition, elapsedTime / moveDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure the drawer reaches the exact open position at the end
        transform.localPosition = targetPosition;
        gameObject.GetComponent<BoxCollider>().enabled = false;
    }

    // Coroutine to smoothly close the drawer
    private IEnumerator CloseDrawer()
    {
        gameObject.GetComponent<BoxCollider>().enabled = true;
        Vector3 targetPosition = new Vector3(closedPositionX, transform.localPosition.y, transform.localPosition.z);
        float elapsedTime = 0f;

        while (elapsedTime < moveDuration)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, targetPosition, elapsedTime / moveDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure the drawer reaches the exact closed position at the end
        transform.localPosition = targetPosition;
    }


}
