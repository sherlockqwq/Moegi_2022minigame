using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moveTrap : MonoBehaviour
{
    public Transform pointA;
    public Transform pointB;
    private Vector3 PositionA;
    private Vector3 PositionB;
    private Vector3 pointStart;
    private Vector3 pointDestination;


    public float moveSpeed;//0到1之间
    private void Start()
    {
        PositionA = pointA.position;
        PositionB = pointB.position;
        transform.position = Vector3.Lerp(transform.position, PositionA, moveSpeed * Time.deltaTime);
        pointStart = PositionA;
        pointDestination = PositionB;
        StartCoroutine(MoveBetweenAB());
    }
    /*void MoveTowardsPoint(Vector3 pointStart, Vector3 pointDestination)//放到update里面
    {

        transform.position = Vector3.Lerp(pointStart, pointDestination, moveSpeed * Time.deltaTime);

    }*/
    IEnumerator MoveBetweenAB()
    {
        Vector3 destination = PositionA;
        while ((transform.position - pointDestination).sqrMagnitude > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, pointDestination, moveSpeed * Time.deltaTime);
            yield return 1;

        }
        yield return 1;
    }
    private void Update()
    {

        if ((transform.position - PositionA).sqrMagnitude < 0.1f)
        {
            pointDestination = PositionB;


        }
        else if ((transform.position - PositionB).sqrMagnitude < 0.1f)
        {
            pointDestination = PositionA;

        }
    }

}
