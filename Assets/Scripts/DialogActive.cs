using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;
using UnityEngine.UI;

[Serializable]
public class Discuss {
    public string Speaker;
    public string Dialog;
    public string Picture;
}

public class DialogActive : MonoBehaviour {
    public GameObject Form;
    public TextMeshProUGUI Speaker;
    public TextMeshProUGUI Dialog;
    public RectTransform Display_L;
    public RectTransform Display_R;

    public List<Discuss> Sentences;
    //public TextAsset ScriptText;

    public bool IsOnceTime;
    public bool IsWriting;
    public bool IsCutscene;

    public int Index = 0;

    public UnityAction OnFinishedAction { get; set; }

    public int EventCutsceneLoop {
        set {
            var director = transform.root.GetComponent<PlayableDirector>();
            if (value != 0) {
                director.time = value;
            } else {
                PlayerActive.Instance.OnInteraction -= NextSentence;
                Destroy(gameObject);
            }
        }
    }

    public void ButtonNext_Handler() {
        NextSentence();
    }

    private bool flag = false;

    private IEnumerator WriteSentence() {
        Speaker.text = Sentences[Index].Speaker;
        foreach (char text in Sentences[Index].Dialog.ToCharArray()) {
            if (Dialog.text != Sentences[Index].Dialog) {
                Dialog.text += text;
                IsWriting = true;
                yield return new WaitForSeconds(0.025f);
            } else {
                break;
            }
        }
        IsWriting = false;
        Index++;
    }

    private void NextSentence() {
        Dialog.text = "";
        if (IsWriting) {
            Dialog.text = Sentences[Index].Dialog;
            IsWriting = false;
        } else {
            var state = Index <= Sentences.Count - 1;
            if (state) {
                var over = Sentences[Index].Speaker.Equals(Speaker.text);
                var texture = Resources.Load<Sprite>("Texture/" + Sentences[Index].Picture);

                Display_L.GetComponent<Image>().sprite = texture;
                Display_R.GetComponent<Image>().sprite = texture;
                if (!over) {
                    flag = !flag;
                    Display_L.gameObject.SetActive(flag);
                    Display_R.gameObject.SetActive(!flag);
                }

                StartCoroutine(WriteSentence());
            } else {
                if (IsOnceTime || IsCutscene) {
                    if (IsCutscene) {
                        EventCutsceneLoop = 0;
                    }
                    PlayerActive.Instance.OnInteraction -= NextSentence;
                    gameObject.SetActive(false);
                }
                Debug.Log("Dialog selesai disini");
                OnFinishedAction?.Invoke();
                Index = 0;
            }
            Form.SetActive(state);
            //PlayerActive.Instance.IsInteract = state;
        }
    }

    private void OnTriggerEnter(Collider collision) {
        //if (IsOnceTime) { // <-- Trigger langsung
        //    // Bisa tambahkan fungsi frezee player
        //    NextSentence();
        //}
        PlayerActive.Instance.OnInteraction += NextSentence;
    }

    private void OnTriggerExit(Collider collision) {
        if (!IsOnceTime) {
            PlayerActive.Instance.OnInteraction -= NextSentence;
            Index = 0;
        }
    }

    private void Start() {
        //Sentences = JsonUtility.FromJson<SerializableList<Discuss>>(ScriptText.text).Collection;
        if (IsCutscene) {
            PlayerActive.Instance.OnInteraction += NextSentence;
            NextSentence();
        }
    }
}