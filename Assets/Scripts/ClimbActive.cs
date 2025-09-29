using UnityEngine;

public class ClimbActive : MonoBehaviour {
    public bool IsFront;

    private float delay;

    private void OnTriggerStay(Collider other) {
        if (other.CompareTag("Player")) {
            var vector = IsFront ? transform.forward * -1f : transform.forward;
            var player = PlayerActive.Instance;
            if (player.IsClimb != 2) {
                player.IsClimb = 1;
            }
            player.CliffFace = Quaternion.LookRotation(vector);
            player.IsFall = player.IsJump = player.IsIdle = false;
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.CompareTag("Player")) {
            PlayerActive.Instance.CliffFace = Quaternion.identity;
            PlayerActive.Instance.IsJump = false;
            delay = 1f;
        }
    }

    private void Update() {
        if (delay > 0) {
            delay -= Time.deltaTime;
        }

        if (delay < 0) {
            PlayerActive.Instance.IsClimb = 0;
            PlayerActive.Instance.IsFall = true;
            delay = 0;
        }
    }
}
