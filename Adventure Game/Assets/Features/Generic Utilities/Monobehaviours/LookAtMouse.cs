using UnityEngine;

// attach this to any object that needs to point towards the mouse by rotating
public class LookAtMouse : MonoBehaviour
{
    // instead of allowing a custom camera reference, use the main camera instead, which assumes the main camera is being used.
    // uncommenting the following to lines will break scene transitions.
    //[Tooltip("The current camera. Used for converting between pixel coordinates and world coordinates.")]
    //public Camera cam;
    
    // code inspired by Indie Nuggets and Brackeys on YouTube
    private void Update()
    {
        // vector subtraction gets a vector pointing from the first vector to the second.
        Vector3 lookDir = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position; // math done in world coordinates. Pixel coordinates are also possible, as long as it's consistent.

        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg; // get the angle to point towards.
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward); // Vector3.forward as the axis because that represents the z axis (which is typically for rotating in 2D)
    }


}
