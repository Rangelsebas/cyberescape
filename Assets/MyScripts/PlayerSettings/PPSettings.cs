using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class PPSettings : MonoBehaviour
{
    public static PPSettings Instance { get; set; }

    public PostProcessProfile pp;
    private ChromaticAberration ch;
    private ColorGrading cg;
    private Vignette vg;

    private float speed = 2f;
    private float sch, stemperaturecg, stintcg, ssaturationcg, sintensityvg;

    private float temperaturecg = 57f, tintcg = -6.5f, saturationcg = 16f, /*intensityvg = 0.55f,*/ intensitych = 1.0f;

    private void Awake()
    {
        Instance = this;

        ch = pp.GetSetting<ChromaticAberration>();
        cg = pp.GetSetting<ColorGrading>();
        vg = pp.GetSetting<Vignette>();

        ValuesSetUp();
        StartValuesSetup();
    }

    private void StartValuesSetup()
    {
        sch = ch.intensity.value;
        stemperaturecg = cg.temperature;
        stintcg = cg.tint;
        ssaturationcg = cg.saturation;
        sintensityvg = vg.intensity;
    }

    private void ValuesSetUp()
    {
        ch.intensity.value = 0.2f;
        cg.temperature.value = 0f;
        cg.tint.value = 0f;
        cg.saturation.value = 0f;
        vg.intensity.value = 0f;
        vg.color.value = Color.black;
    }

    private void Update()
    {

    }

    public void PulseAbberation()
    {
        ch.intensity.value = Mathf.Lerp(ch.intensity.value, intensitych, Time.time * speed);
        Invoke(nameof(StopChromatic), 0.35f);
    }

    private void StopChromatic()
    {
        ch.intensity.value = Mathf.Lerp(ch.intensity.value, sch, Time.time * speed);
    }

    public void PulseSlowmotion()
    {
        cg.temperature.value = Mathf.Lerp(cg.temperature.value, temperaturecg, Time.time * speed);
        cg.tint.value = Mathf.Lerp(cg.tint.value, tintcg, Time.time * speed);
        cg.saturation.value = Mathf.Lerp(cg.saturation.value, saturationcg, Time.time * speed);
    }

    public void StopSlowmotion()
    {
        cg.temperature.value = Mathf.Lerp(cg.temperature.value, stemperaturecg, Time.time * speed);
        cg.tint.value = Mathf.Lerp(cg.tint.value, stintcg, Time.time * speed);
        cg.saturation.value = Mathf.Lerp(cg.saturation.value, ssaturationcg, Time.time * speed);
    }

    /*public void PulseVignette()
    {
        vg.intensity.value = Mathf.Lerp(vg.intensity.value, intensityvg, Time.time * speed);
        Invoke(nameof(StopVignette), 0.15f);
    }

    private void StopVignette()
    {
        vg.intensity.value = Mathf.Lerp(vg.intensity.value, sintensityvg, Time.time * speed);
    }*/

    /*public void StartLowHealthDisplayPingPong()
    {
        vg.color.value = Color.red;
        vg.intensity.value = Mathf.PingPong(Time.time, 0.55f);
    }

    public void StopLowHealthDisplayPingPong()
    {
        vg.color.value = Color.black;
        vg.intensity.value = sintensityvg;
    }*/
}
