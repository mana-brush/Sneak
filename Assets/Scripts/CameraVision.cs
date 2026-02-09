using UnityEngine;
using TMPro;

public class CameraVision : MonoBehaviour
{

    public LayerMask layersToHit;
    public TextMeshProUGUI cameraText; 
    public float detectionRange = 10f;
    public int detectionAngle = 30;
    public GameObject player;
    private bool isInAngle, isInRange, isNotHidden;
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        (isInAngle, isInRange, isNotHidden) = (false, false, false);

        if (Vector3.Distance(transform.position, player.transform.position) < detectionRange)
        {
            isInRange = true;
        }

        if (Physics.Raycast(transform.position, (player.transform.position - transform.position), out var hit, Mathf.Infinity, layersToHit))
        {
            if (hit.transform == player.transform)
            {
                isNotHidden = true;
            }
        }

        Vector3 side1 = player.transform.position - transform.position;
        Vector3 side2 = transform.forward;
        
        float angle = Vector3.SignedAngle(side1, side2, Vector3.up);
        if (angle < detectionAngle && angle > (-1 * detectionAngle))
        {
            isInAngle = true;
        }

        if (isInRange && isInAngle && isNotHidden)
        {
            cameraText.text = "Visible";
            cameraText.color = Color.green;
        }
        else
        {
            cameraText.text = "Not Visible";
            cameraText.color = Color.red;
        }

    }
}
