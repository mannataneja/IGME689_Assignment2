using UnityEngine;

public class Fire : MonoBehaviour
{
    public FireSimulation fireSimulation;

    private void Start()
    {
        fireSimulation = FindFirstObjectByType<FireSimulation>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Boundary")
        {
            Debug.Log("Entering fire district");
            fireSimulation.slow = true;
        }
    }
}
