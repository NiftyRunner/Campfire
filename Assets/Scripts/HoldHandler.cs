using UnityEngine;

[RequireComponent (typeof(Rigidbody))]
public class HoldHandler : MonoBehaviour
{
    [SerializeField] private float followSpeed = 10.0f; 
    
    private bool isHolding;
    private Transform trackingPoint;
    private Rigidbody rb;

    public void SetHolding(bool state) => isHolding = state;
    public void SetTrackingTransform(Transform _trackingPoint) => trackingPoint = _trackingPoint;

    bool isParentSet = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (isHolding)
        {
            //For physics based - use fixed update(jittery)
            //Vector3 newPos = Vector3.Lerp(rb.position, trackingPoint.position, followSpeed * Time.fixedDeltaTime);
            //rb.MovePosition(newPos);

            //For normal movement - change to lateUpdate
            transform.position = Vector3.Lerp(transform.position, trackingPoint.position, followSpeed * Time.deltaTime);


            //For normal movement
            //isParentSet = true;
            //transform.SetParent(trackingPoint);
            //transform.localPosition = Vector3.zero;
            //transform.localRotation = Quaternion.identity;
        }

        //if (!isHolding && isParentSet) {
        //    transform.SetParent(null);
        //    isParentSet= false;
        //}

    }

}
