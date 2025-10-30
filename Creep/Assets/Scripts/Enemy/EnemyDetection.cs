using UnityEngine;

public class EnemyDetection : MonoBehaviour
{
    public Transform player;
    public float detectionRange = 10f;
    public float detectionAngle = 45f;

    public void CheckForPlayer()
    {
        Vector3 dirToPlayer = player.position - transform.position;
        float angle = Vector3.Angle(transform.forward, dirToPlayer);

        if (angle < detectionAngle)
        {
            if (Physics.Raycast(transform.position, dirToPlayer.normalized, out RaycastHit hit, detectionRange))
            {
                if (hit.transform.CompareTag("Player"))
                {
                    //Debug.Log("Player spotted!");
                    GameManager.Instance?.PlayerCaught();
                }
            }
        }
    }
}