using System.Collections;
using UnityEngine;

public class RightArmMove : MonoBehaviour
{
    public bool enableRotation = false;
    public float moveSpeed = 1f;             // Units per second
    public float waitAfterMove = 0.5f;       // Wait time after each move
    public float waitBetweenRotations = 0.5f; // Wait time between rotation steps
    public float rotationSpeed = 5f;         // For smooth rotation

    public float xStep = 0.05f;
    public int xSteps = 2;

    private Transform startTransform;

    private void Start()
    {
        startTransform = transform;
        StartCoroutine(MoveLoop());
    }

    IEnumerator MoveLoop()
    {
        for (int i = 0; i <= xSteps; i++)
        {
            float currentX = startTransform.position.x - i * xStep;

            Vector3[] moveSequence = new Vector3[]
            {
                new Vector3(currentX, 0.487f, 0.4f),
                new Vector3(currentX, 0.537f, 0.4f),
                new Vector3(currentX, 0.437f, 0.4f),
                new Vector3(currentX, 0.487f, 0.4f),
                new Vector3(currentX, 0.487f, 0.45f),
                new Vector3(currentX, 0.487f, 0.35f),
                new Vector3(currentX, 0.487f, 0.4f),
            };

            foreach (Vector3 targetPos in moveSequence)
            {
                yield return StartCoroutine(SmoothMoveToPosition(targetPos));
                yield return new WaitForSeconds(waitAfterMove);

                if (enableRotation)
                {
                    yield return StartCoroutine(RotateSequence());
                }
            }
        }
    }

    IEnumerator SmoothMoveToPosition(Vector3 target)
    {
        while (Vector3.Distance(transform.position, target) > 0.001f)
        {
            transform.position = Vector3.MoveTowards(transform.position, target, moveSpeed * Time.deltaTime);
            yield return null;
        }

        transform.position = target; // Snap exactly
    }

    IEnumerator RotateSequence()
    {
        yield return RotateTo(Vector3.up, 10f);
        yield return new WaitForSeconds(waitBetweenRotations);

        yield return RotateTo(Vector3.up, -20);
        yield return new WaitForSeconds(waitBetweenRotations);

        yield return RotateTo(Vector3.up, 10f);
        yield return new WaitForSeconds(waitBetweenRotations);

        yield return RotateTo(Vector3.forward, 10f);
        yield return new WaitForSeconds(waitBetweenRotations);

        yield return RotateTo(Vector3.forward, -20);
        yield return new WaitForSeconds(waitBetweenRotations);

        yield return RotateTo(Vector3.forward, 10f);
        yield return new WaitForSeconds(waitBetweenRotations);
    }

    IEnumerator RotateTo(Vector3 axis, float angle)
    {
        Quaternion startRot = transform.rotation;
        Quaternion targetRot = startRot;

        if (axis == Vector3.up)
            targetRot = Quaternion.Euler(startTransform.rotation.eulerAngles.x, startTransform.rotation.eulerAngles.y + angle, startTransform.rotation.eulerAngles.z);
        else if (axis == Vector3.forward)
            targetRot = Quaternion.Euler(startTransform.rotation.eulerAngles.x, startTransform.rotation.eulerAngles.y, startTransform.rotation.eulerAngles.z + angle);

        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime * rotationSpeed;
            transform.rotation = Quaternion.Slerp(startRot, targetRot, t);
            yield return null;
        }

        transform.rotation = targetRot;
    }
}
