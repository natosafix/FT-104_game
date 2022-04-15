using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTracking : MonoBehaviour
{
    public GameObject Player;
    private const float OffsetZ = -10.0f;
    public float Smooth = 5.0f;
    public float ZoomSensitivity = 10.0f;
    
    void Start()
    {
        Player = GameObject.Find("Player");
        var playerPosition = Player.transform.position;
        transform.position = new Vector3(playerPosition.x, playerPosition.y, OffsetZ);
    }

    void Update()
    {
        var delta = Input.GetAxis("Mouse ScrollWheel") * ZoomSensitivity * -1;
        var playerPosition = Player.transform.position;
        var newCameraPosition = new Vector3(playerPosition.x, playerPosition.y, OffsetZ);
        transform.position = Vector3.Lerp(transform.position, newCameraPosition, Time.deltaTime * Smooth);
        if (Camera.main.orthographicSize + delta / 10.0f >= 2f && Camera.main.orthographicSize + delta / 10.0f <= 4.5f)
            Camera.main.orthographicSize += delta / 10.0f;
    }
}
