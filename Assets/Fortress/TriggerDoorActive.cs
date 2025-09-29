using System.Collections.Generic;
using UnityEngine.Rendering;
using UnityEngine;

public class DoorActive : MonoBehaviour {
    public Animator Anim;
    public List<Material> MateList;
    public bool IsActive;

    private void MaterialHitAlpha(bool opaque) {
        foreach (var item in MateList) {
            MaterialFadeAlpha(item, opaque);
            item.color = opaque ? AlphaAdjustment(item) : AlphaAdjustment(item, 0.3f);
        }
    }
    
    private void MaterialFadeAlpha(Material mate, bool opaque) {
        mate.SetFloat("_Mode", opaque ? 0 : 3);
        mate.SetInt("_SrcBlend", (int)(opaque ? BlendMode.One : BlendMode.SrcAlpha));
        mate.SetInt("_DstBlend", (int)(opaque ? BlendMode.Zero : BlendMode.OneMinusSrcAlpha));
        mate.SetInt("_ZWrite", opaque ? 1 : 0);
        mate.DisableKeyword("_ALPHATEST_ON");
        if (opaque) {
            mate.DisableKeyword("_ALPHABLEND_ON");
        } else {
            mate.EnableKeyword("_ALPHABLEND_ON");
        }
        mate.DisableKeyword("_ALPHAPREMULTIPLY_ON");
        mate.renderQueue = opaque ? -1 : 3000;
    }

    private Color AlphaAdjustment(Material mate, float alpha = 1f) {
        return new Color(mate.color.r, mate.color.g, mate.color.b, alpha);
    }

    private void OnTriggerEnter(Collider other) {
        if (Anim) {
            Anim.SetBool("IsOpen", true);
        }
        MaterialHitAlpha(IsActive);
    }

    private void OnTriggerExit(Collider other) {
        if (Anim) {
            Anim.SetBool("IsOpen", false);
        }
        //MaterialHitAlpha(!IsActive);
    }

    private void Start() {
        Anim = GetComponent<Animator>();
    }
}
