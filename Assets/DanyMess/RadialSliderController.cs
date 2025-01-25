using UnityEngine;

public class RadialSliderController : MonoBehaviour
{
    public Material radialMaterial; // �������� � ��������

    [Range(0, 1)]
    public float fillAmount = 1; // �������� �������

    void Update()
    {
        if (radialMaterial != null)
        {
            radialMaterial.SetFloat("_FillAmount", fillAmount);
        }
    }

    public void SetFillAmount(float value)
    {
        fillAmount = Mathf.Clamp01(value);
    }
}
