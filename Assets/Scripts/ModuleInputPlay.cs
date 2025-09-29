using UnityEngine.Events;
using UnityEngine;

public enum ActionState {
    Interaction, Stance, Camera,
    Dash, Jump, Death, Rest,
    Equip1, Equip2, Attack, Special,
    Objective,
}

[CreateAssetMenu(fileName = "ModuleInputPlay", menuName = "AddOn Module/Input Play", order = 1)]
public class ModuleInputPlay : ScriptableObject {
    public UnityAction<ActionState> OnAction { get; set; }
    public UnityAction<ActionState> OnCamera { get; set; }

    /// <summary>
    /// Metode input khusus untuk pergerakan player, 
    /// akan di inisiasi langsung ketika game sedang berjalan
    /// </summary>
    public Vector3 MoveHandler {
        get {
            var axisX = input.GamePlay.Movement.ReadValue<Vector2>().x;
            var axisZ = input.GamePlay.Movement.ReadValue<Vector2>().y;
            return new Vector3(axisX, 0, axisZ);
        }
    }

    /// <summary>
    /// Metode input khusus untuk rotasi kamera third person, 
    /// akan di inisiasi langsung ketika game sedang berjalan
    /// dan kamera third person diaktifkan
    /// </summary>
    public Vector3 LookHandler {
        get {
            var axisX = input.GamePlay.Look.ReadValue<Vector2>().x;
            var axisY = input.GamePlay.Look.ReadValue<Vector2>().y;
            return new Vector3(axisX, axisY, 0);
        }
    }

    private InputMap input;

    /// <summary>
    /// Metode input selain pergerakan yang aktif ketika input sistem dipanggil,
    /// metode ini akan diinisiasi melewati fungsi start
    /// </summary>
    private void ActionAwake() {
        // Stance Action 
        // Keyboard = Key Tilde
        // Gamepad  = Left Stick Button (PS, Xbox = L3) 
        input.GamePlay.Stance.performed += (e) => {
            OnAction?.Invoke(ActionState.Stance);
        };

        // Stance Action 
        // Keyboard = Key Control/Command
        // Gamepad  = Right Stick Button (PS, Xbox = R3) 
        input.GamePlay.Camera.performed += (e) => {
            OnCamera?.Invoke(ActionState.Camera);
        };

        // Objective Action 
        // Keyboard = Key Tab
        // Gamepad  = Right Shoulder Button (PS = R1, Xbox = RS) 
        input.GamePlay.Objective.performed += (e) => {
            OnAction?.Invoke(ActionState.Objective);
        };

        // Rest/Dash Action 
        // Keyboard = Key Shift 
        // Gamepad  = Left Shoulder Button (PS = L1, Xbox = LS) 
        input.GamePlay.Rest.performed += (e) => {
            OnAction?.Invoke(ActionState.Rest);
        };

        // Interaction Action 
        // Keyboard = Key Enter
        // Gamepad  = South Button (PS = Circle, Xbox = A) 
        input.GamePlay.Interaction.performed += (e) => {
            OnAction?.Invoke(ActionState.Interaction);
        };

        // Jump Action 
        // Keyboard = Key Space 
        // Gamepad  = East Button (PS = Round, Xbox = B) 
        input.GamePlay.Jump.performed += (e) => {
            OnAction?.Invoke(ActionState.Jump);
        };

        // Equip Slot 1 Action 
        // Keyboard = Key E
        // Gamepad  = West Button (PS = Square, Xbox = X) 
        input.GamePlay.Equip1.performed += (e) => {
            OnAction?.Invoke(ActionState.Equip1);
        };

        // Equip Slot 2 Action 
        // Keyboard = Key R
        // Gamepad  = North Button (PS = Triangle, Xbox = Y) 
        input.GamePlay.Equip2.performed += (e) => {
            OnAction?.Invoke(ActionState.Equip2);
        };

        // Special/Skill Attack Action 
        // Keyboard = Key Q 
        // Gamepad  = Left Trigger Button (PS = L2, Xbox = LT)
        input.GamePlay.Special.performed += (e) => {
            OnAction?.Invoke(ActionState.Special);
        };

        // Basic Attack Action 
        // Keyboard = Right Mouse
        // Gamepad  = Right Trigger Button (PS = R2, Xbox = RT) 
        input.GamePlay.Attack.performed += (e) => {
            OnAction?.Invoke(ActionState.Attack);
        };
    }

    private void OnEnable() {
        input = new();
        input.Enable();
        ActionAwake();   // Aktivasi input selain pergerakan
    }

    private void OnDisable() {
        input.Disable();
    }
}
