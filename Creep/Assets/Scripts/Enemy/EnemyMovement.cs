using UnityEngine;
using System.Collections;

public class EnemyMovement : MonoBehaviour
{
    public float moveSpeed = 0.5f;
    public float turnInterval = 5f;
    public float turnDuration = 1f;
    public float lookDuration = 1.5f;
    public AudioSource footstepAudio;
    public Transform player;

    private bool isTurning = false;

    void Start()
    {
        // automatically find the player if not assigned
        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player")?.transform;

        // start footsteps
        if (footstepAudio != null)
        {
            footstepAudio.loop = true;
            footstepAudio.Play();
        }

        // start coroutines for moving and looking
        StartCoroutine(MoveContinuously());
        StartCoroutine(LookRoutine());
    }

    IEnumerator MoveContinuously()
    {
        while (true)
        {
            if (!isTurning && player != null)
            {
                // calculate flat direction away from player
                Vector3 dir = (transform.position - player.position);
                dir.y = 0f; // keep horizontal
                dir.Normalize();

                // move smoothly away
                transform.position += dir * moveSpeed * Time.deltaTime;

                // face the direction of movement smoothly
                Quaternion targetRot = Quaternion.LookRotation(dir);
                transform.rotation = Quaternion.Lerp(transform.rotation, targetRot, Time.deltaTime * 5f);

                // keep upright
                transform.rotation = Quaternion.Euler(0, transform.eulerAngles.y, 0);

                // make sure footsteps are playing while moving
                if (footstepAudio != null && !footstepAudio.isPlaying)
                    footstepAudio.Play();
            }
            else if (isTurning && footstepAudio != null && footstepAudio.isPlaying)
            {
                // pause footsteps while turning
                footstepAudio.Pause();
            }

            yield return null;
        }
    }

    IEnumerator LookRoutine()
    {
        while (true)
        {
            // wait for next look event
            yield return new WaitForSeconds(turnInterval);

            if (player == null) yield break;

            // start turning toward player
            isTurning = true;

            // turn smoothly toward player
            yield return StartCoroutine(TurnToFace(player.position));

            // trigger detection when facing player
            GetComponent<EnemyDetection>()?.CheckForPlayer();

            // hold the stare
            yield return new WaitForSeconds(lookDuration);

            // turn back to walk away
            Vector3 awayTarget = transform.position + (transform.position - player.position).normalized;
            yield return StartCoroutine(TurnToFace(awayTarget));

            // resume moving & footsteps
            isTurning = false;
        }
    }

    IEnumerator TurnToFace(Vector3 targetPos)
    {
        // flatten rotation target
        Vector3 flatTarget = targetPos;
        flatTarget.y = transform.position.y;

        Quaternion startRot = transform.rotation;
        Quaternion targetRot = Quaternion.LookRotation(flatTarget - transform.position);
        float elapsed = 0f;

        while (elapsed < turnDuration)
        {
            transform.rotation = Quaternion.Slerp(startRot, targetRot, elapsed / turnDuration);
            transform.rotation = Quaternion.Euler(0, transform.eulerAngles.y, 0);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.rotation = targetRot;
        transform.rotation = Quaternion.Euler(0, transform.eulerAngles.y, 0);
    }
}