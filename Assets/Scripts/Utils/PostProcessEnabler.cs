using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PostProcessEnabler : MonoBehaviour
{
    public void Bloom(bool enable)
    {
        var profile = GetComponent<Volume>();
        profile.profile.TryGet(out Bloom bloom);
        bloom.active = enable;
    }

    public void Chromatic(bool enable)
    {
        var profile = GetComponent<Volume>();
        profile.profile.TryGet(out ChromaticAberration bloom);
        bloom.active = enable;
    }
    public void Color(bool enable)
    {
        var profile = GetComponent<Volume>();
        profile.profile.TryGet(out ColorAdjustments bloom);
        bloom.active = enable;
    }
}
