using UnityEngine;

public class CameraActive : MonoBehaviour {
    public Transform Player;
    public Transform[] Cameras;
    public ModuleInputPlay InputPlay;
    public float Speed;
    public bool isometric;

    private Quaternion rotation;

    /// <summary>
    /// Action method is handler from input system event 
    /// </summary>
    /// <param name="State">Type of state (enumeration)</param>
    private void Action(ActionState State) {
        if (State != ActionState.Camera) return;
        isometric = !isometric;         // Pergantian kamera
        transform.rotation = Quaternion.identity;

        // Setingan kamera isometrik
        Cameras[0].gameObject.SetActive(isometric);
        Cameras[0].SetLocalPositionAndRotation(new Vector3(5, 5, -5), Quaternion.Euler(30, -45, 0));
        // Setingan kamera third person
        Cameras[1].gameObject.SetActive(!isometric);
        Cameras[1].SetLocalPositionAndRotation(new Vector3(0, 3, -4), Quaternion.Euler(15, 0, 0));
    }

    private void Start() {
        // Reset status
        rotation = Quaternion.identity;
        InputPlay.OnCamera = Action;
    }

    private void LateUpdate() {
        // Rotasi kamera hanya diizinkan untuk third person camera
        if (Cameras[1].gameObject.activeInHierarchy) {
            // Dapatkan inputan dari player
            var vector = InputPlay ? InputPlay.LookHandler.normalized : Vector3.zero;
            // Penyesuaian rotasi pada kamera
            rotation.x += vector.y;
            rotation.y += vector.x;
            rotation.x = Mathf.Clamp(rotation.x, -15f, 45f);
            // Memutar kamera
            transform.localRotation = Quaternion.Euler(rotation.x, rotation.y, rotation.z);
        }
        // Kamera mengikuti player
        transform.position = Vector3.Slerp(transform.position, Player.position, Speed * Time.deltaTime);
    }
}
