using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTracking : MonoBehaviour
{
    public GameObject Player;
    private Rigidbody2D playerRB;
    private const float OffsetZ = -10.0f;
    public float Smooth = 5.0f;
    public float ZoomSensitivity = 10.0f;
    
    void Start()
    {
        Player = GameObject.Find("Player");
        playerRB = Player.GetComponent<Rigidbody2D>();
        var playerPosition = Player.transform.position;
        transform.position = new Vector3(playerPosition.x, playerPosition.y, OffsetZ);
        Camera.main.orthographicSize += 2;
    }

    void Update()
    {
        var playerPosition = playerRB.position + Vector2.up.Rotate(playerRB.rotation);
        var newCameraPosition = new Vector3(playerPosition.x, playerPosition.y, OffsetZ);
        transform.position = Vector3.Lerp(transform.position, newCameraPosition, Time.deltaTime * Smooth);
        /*var delta = Input.GetAxis("Mouse ScrollWheel") * ZoomSensitivity * -1;
        if (Camera.main.orthographicSize + delta / 10.0f >= 0.01f && Camera.main.orthographicSize + delta / 10.0f <= 30f)
            Camera.main.orthographicSize += delta / 10.0f;*/
    }
}