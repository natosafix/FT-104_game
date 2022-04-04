using UnityEngine;

public class CameraTracking : MonoBehaviour
{
    const float Smooth = 7.5f;
    const float ZoomSensitivity = 10.0f;
    
    public GameObject Player;
    
    void Start()
    {
        Player = GameObject.Find("Cowboy");
        transform.position = new Vector3(Player.transform.position.x, Player.transform.position.y, -10);
    }
    
    void Update()
    {
        var delta = -1 * Input.GetAxis("Mouse ScrollWheel") * ZoomSensitivity;
        var newCameraPos = new Vector3(Player.transform.position.x, Player.transform.position.y, -10);
        
        transform.position = Vector3.Lerp(transform.position, newCameraPos, Time.deltaTime * Smooth);
        Camera.main.orthographicSize += delta / 5.0f;
        Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize, 2f, 4.5f);
    }
}
