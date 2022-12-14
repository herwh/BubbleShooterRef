using UnityEngine;
using UnityEngine.EventSystems;

public class InputController : MonoBehaviour
{
    [SerializeField] private GameObject _arrow;
    [SerializeField] private ShootBubbleSpawner _shootBubbleSpawner;
    [SerializeField] private float _throwForce;

    private void Update()
    {
#if UNITY_EDITOR
       if (EventSystem.current.IsPointerOverGameObject())
       {
           return;
       } 
#else
       foreach (Touch touch in Input.touches)
       {
           int id = touch.fingerId;
           if (EventSystem.current.IsPointerOverGameObject(id))
           {
               return;
           }
       }
#endif
        
        if (Input.GetMouseButton(0))
        {
            RotateArrow();
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (_shootBubbleSpawner.HasBubble)
            {
                ThrowBubble(_shootBubbleSpawner.PopBubble());
            }
        }
    }

    private void RotateArrow()
    {
        var mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0;
        var mouseDirection = mousePosition - _arrow.transform.position;
        var lookRotation = Quaternion.LookRotation(Vector3.forward, mouseDirection);
        var euler = lookRotation.eulerAngles;
        var zAngle = euler.z;

        if (euler.z > 180)
        {
            zAngle -= 360;
        }

        var rotationLimit = Mathf.Clamp(zAngle, -80f, 80f);
        _arrow.transform.rotation = Quaternion.Euler(0, 0, rotationLimit);
    }

    private void ThrowBubble(Bubble bubble)
    {
        var direction = _arrow.transform.up;
        bubble.MoveOnDirection(direction, _throwForce);
    }
}