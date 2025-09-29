using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActive : MonoBehaviour {
    // Player input system
    [Header("Character Module")]
    public ModuleInputPlay InputPlay;

    #region Action Attribute
    [Header("Character Equip")]
    // Equipment on the right hand
    public bool IsRightArmed;
    public Transform Weapon;
    public Transform SlotWeapon;
    public Transform SlotPrimary;
    // Equipment on the left hand
    public bool IsLeftArmed;
    public Transform Shield;
    public Transform SlotShield;
    public Transform SlotSecond;

    // Atribut pendeteksi musuh
    [Header("Character Target")]
    public Transform EnemyTarget;
    public List<Transform> EnemiesList;
    public GameObject[] EnemiesAll;
    public float EnemyRange;
    public int EnemyIndex;
    #endregion

    // Locomotion state
    [Header("Character State")]
    public bool IsIdle;
    public bool IsFall;
    public bool IsJump;
    public bool IsDash;
    public bool IsStance;
    public bool IsAttack;
    public int IsClimb;

    public Quaternion CliffFace { get; set; }

    // Player singleton
    public static PlayerActive Instance { get; private set; }

    // Player attribute
    private Animator anim;
    private CharacterController body;
    private Vector3 moveupdate;
    private float movespeed;
    private float delay;
    private bool dashnext;

    /// <summary>
    /// Method used to activate equipment on the player.
    /// By moving an item from the primary slot to the right-hand slot.
    /// </summary>
    private void EquipMainSwitch() {
        // Verify that the player has the required equipment and battle suit.
        if (Weapon) {
            IsRightArmed = !IsRightArmed;                                           // Equipment activation
            Weapon.SetParent(IsRightArmed ? SlotWeapon : SlotPrimary);              // Moving equipment
            Weapon.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);  // Reset position and rotation equipment
            // In the stance condition, when storing equipment, it will directly switch to the idle position
            if (!IsRightArmed && IsStance) {
                IsStance = false;
            }
        }
        // Deactivate attack mode if it is currently active
        IsAttack = false;
    }

    /// <summary>
    /// Method used to activate equipment on the player.
    /// By moving an item from the secondary slot to the left-hand slot.
    /// </summary>
    private void EquipSubSwitch() {
        // Verify that the player has the required equipment and battle suit.
        if (Shield) {
            IsLeftArmed = !IsLeftArmed;                                             // Equipment activation
            Shield.SetParent(IsLeftArmed ? SlotShield : SlotSecond);                // Moving equipment
            Shield.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);  // Reset position and rotation equipment
            // In the stance condition, when storing equipment, it will directly switch to the idle position
            if (!IsLeftArmed && IsStance) {
                IsStance = false;
            }
        }
        // Deactivate attack mode if it is currently active
        IsAttack = false;
    }

    /// <summary>
    /// Method for the player to perform an attack action,
    /// usable only when a weapon is equipped and active.
    /// </summary>
    private void Attack() {
        // The player must be in an active equipment state or not resting state
        if (IsAttack) {
            // The enemy must be locked on
            if (EnemyTarget) {
                // Calculate the enemy’s distance from the player
                var dist = Vector3.Distance(EnemyTarget.position, transform.position);
                // If the distance is greater than 5f and less than 35f, then the player will move closer to the enemy
                if (dist > 5f && dist < 35f && EnemyTarget) { 
                    Movement(EnemyTarget, dist); // Move toward the target
                    return;                      // Function terminates
                }
            }
            // Start attacking if the attack delay is 0, The delay is determined by the player’s attack speed
            if (delay == 0 && IsIdle) {
                anim.SetFloat("Combat", 0);      // Set animation style to basic attack
                anim.SetTrigger("Attack");       // Play attack animation
                delay = 0.5f;                    // Set delay to match the attack speed, or default to 0.5f
            }
        }
    }

    /// <summary>
    /// Coroutine Method for the player to perform a jump action, either from an idle position or while moving
    /// </summary>
    /// <returns>Wait for 0.5 seconds</returns>
    private IEnumerator JumpCoroutine() {
        // Do while character not in jump state
        if (!IsJump && !IsStance) {
            IsFall = false;            // Deactivate fall system
            IsJump = true;             // Set state to jump
            anim.SetFloat("Move", 4f); // Change animation to jump action
            if (IsIdle) {
                // Jump action for player in idle state
                moveupdate.y += Mathf.Sqrt(270f); // (1f * -3 * gravity = -30f)
            } else {
                // Jump action for player in moving state
                moveupdate.y++;
                yield return new WaitForSeconds(0.5f);
            }
            IsFall = true;             // Activate fall system
        }
    }

    /// <summary>
    /// Coroutine Method for the player to perform a dash action
    /// </summary>
    /// <returns>Wait for 0.25 seconds</returns>
    private IEnumerator DashCoroutine() {
        movespeed = 20f;             // Boost speed up character
        anim.SetFloat("Move", 3f);   // Change animation to dash
        // Wait for 0.25s is value of time delay to character dash
        yield return new WaitForSeconds(0.25f);
        IsDash = false;              // Deactivate dash system
    }

    /// <summary>
    /// Metode untuk mengaktifkan sistem gravitasi pada player
    /// </summary>
    private void Fall() {
        // Sistem akan berjalan jika status IsFall aktif
        if (IsFall) {
            IsClimb = 0;                            // Reset IsClimb ke angka 0 untuk mencegah bug
            moveupdate.y += -30f * Time.deltaTime;  // Set gravitas 30f kearah bawah atau minus
            body.Move(moveupdate * Time.deltaTime); // Down force movement
            anim.SetFloat("Move", 4f);              // Mainkan animasi jump
            // Checking player grounded
            if (body.isGrounded) {                  // Lakukan pengecekan apakah player sudah jejak ketanah
                // Reset atttribute
                moveupdate = Vector3.zero;
                anim.SetFloat("Move", 0f);
                IsJump = false;
            } 
        }
    }

    /// <summary>
    /// Movement method to procces locomotion on player
    /// </summary>
    private void Movement() {
        Fall();                                             // Aktifkan fungsi fall 
        var vector = InputPlay ? InputPlay.MoveHandler.normalized : Vector3.zero; // Terima inputan
        IsIdle = (vector.x, vector.z) == (0, 0);            // Idle adalah kondisi dimana tidak terdapat inputan
        anim.SetFloat("AxisX", IsStance ? vector.x : 0f);   // Nilai axis kiri kanan yang digunakan untuk strafing
        anim.SetFloat("AxisZ", IsStance ? vector.z : 0f);   // Nilai axis depan belakang yang digunakan untuk strafing
        // Statement condition between idle and move
        if (IsIdle) {
            // // Reset speed dan animasi menjadi 0 ketika sedang tidak lompat
            if (!IsJump) { 
                movespeed = 0;
                anim.SetFloat("Move", IsStance ? 1f : 0f);
            }
            // Fungsi Attack hanya berlaku untuk kondisi idle 
            Attack();
        } else {
            // Pergerakan tidak boleh dilakukan ketika lompat atau sedang memanjat
            if (!IsJump && IsClimb < 2) {
                movespeed = 6f;                                            // Set kecepatan gerak
                anim.SetFloat("Move", IsStance ? 1f : 2f);                  // Jalankan animasi bergerak
                moveupdate = new Vector3(vector.x, moveupdate.y, vector.z); // Simpan nilai terakhir pergerakan
                // Aksi pergerakan dash
                if (IsDash && !IsStance && !IsJump) {
                    StartCoroutine(DashCoroutine());
                }

                // Setup camera relatif movement
                var forward = Camera.main.transform.forward;
                var right = Camera.main.transform.right;
                forward.y = right.y = 0;
                moveupdate = (vector.z * forward.normalized) + (vector.x * right.normalized);
                // Rotasi akan dinon aktifkan untuk mode "Stance"
                if (!IsStance) {
                    // Setup rotation of character
                    var rotate = Quaternion.LookRotation(moveupdate);
                    transform.rotation = Quaternion.Slerp(rotate, transform.rotation, Time.deltaTime);
                } else { 
                    movespeed = 3f;   // Penurunan sementara kecepatan gerak ketika mode stance
                }
            } 
        }
        // Gerakkan karakter
        body.Move(movespeed * Time.deltaTime * moveupdate);
    }

    /// <summary>
    /// Metode overload untuk Movement, metode ini akan menggerakkan player menuju ke target yang diinginkan
    /// </summary>
    /// <param name="target">Transform target yang dituju</param>
    /// <param name="distance">Jarak target yang diizinkan untuk melakukan pergerakkan</param>
    private void Movement(Transform target, float distance) {
        // Jika jarak melebihi 25f maka player akan melakukan dash di akhir
        if (distance > 15f) {
            dashnext = true;
        }
        // Player moving to enemy
        movespeed = 6;                                   // Atur kecepatan gerak player = 13f
        moveupdate = target.position - transform.position; // Atur posisi vector3 player
        moveupdate.Normalize();                            // Vector3 di normalisasi
        // Pengaturan rotasi player
        var rotate = Quaternion.LookRotation(moveupdate);  
        transform.rotation = Quaternion.Slerp(rotate, transform.rotation, Time.deltaTime);
        anim.SetFloat("Move", 2f);                         // Atur animasi untuk berlari
        // Cast dash jika jarak sudah kurang dari 20f
        if (distance < 7f && dashnext) {                
            movespeed = 20f;                               // Percepat laju pergerakan sampai 3x
            anim.SetFloat("Move", 3f);                     // Dash pose or animation
        }
        // Gerakkan karakter
        body.Move(movespeed * Time.deltaTime * moveupdate);
    }

    /// <summary>
    /// Action method is handler from input system event 
    /// </summary>
    /// <param name="State">Type of state (enumeration)</param>
    private void Action(ActionState State) {
        switch (State) {
            case ActionState.Jump:
                if (IsClimb == 1 && !IsRightArmed) {
                    // Match rotation between player and climb target
                    transform.SetPositionAndRotation(transform.position, CliffFace);
                    IsClimb = 2;
                    // Climbing
                    anim.SetTrigger("ClimbUp");
                } else {
                    // Jumping
                    StartCoroutine(JumpCoroutine());
                }
                break;
            case ActionState.Stance:
                if (IsRightArmed) {
                    IsStance = !IsStance;
                }
                break;
            case ActionState.Equip1:
                // Harus dicek tipe item yang berada di slot equip
                anim.SetTrigger("Draw1");
                break;
            case ActionState.Equip2:
                // Harus dicek tipe item yang berada di slot equip
                anim.SetTrigger("Draw2");
                break;
            case ActionState.Rest:
                //anim.SetBool("IsRest", !anim.GetBool("IsRest"));
                break;
            case ActionState.Objective:
                IsDash = true;
                break;
            case ActionState.Attack:
                if (IsRightArmed) {
                    IsAttack = true;
                }
                break;
            case ActionState.Special:
                break;
        }
    }

    private void Start() {
        // Setup singleton player
        Instance = this;
        // Setup components
        body = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();

        // Send player action to input system 
        InputPlay.OnAction = Action;
    }

    private void Update() {
        // Update movement
        Movement();

        // Cooldown progress
        if (delay > 0) {
            delay -= Time.deltaTime;
        }

        if (delay < 0) {
            delay = 0;
            IsAttack = dashnext = false;
        }
    }
}