using TMPro;
using UnityEngine;

public class TextDilateExample : MonoBehaviour
{
    public TextMeshProUGUI textMeshPro1;  // ������ �����
    public TextMeshProUGUI textMeshPro2;  // ������ �����

    // ��������� ���������� ��� �������� � ���� ���������
    public float dilateSpeed = 1.0f;  // �������� ��������� Dilate
    public float dilateStrength = 0.2f;  // ���� ��������� Dilate

    // ��������� ���������� ��� ����� �������� Dilate
    public float newDilateValue1 = 0.5f;  // ����� ������������� �������� ��� ������� ������
    public float newDilateValue2 = 1.0f;  // ����� ������������� �������� ��� ������� ������

    private float targetDilate1;  // ���� ��� Dilate ������� ������
    private float targetDilate2;  // ���� ��� Dilate ������� ������
    private float currentDilate1;  // ������� �������� Dilate ������� ������
    private float currentDilate2;  // ������� �������� Dilate ������� ������

    void Start()
    {
        // ������������� ��������� �������� ��� Dilate
        currentDilate1 = textMeshPro1.fontMaterial.GetFloat(ShaderUtilities.ID_FaceDilate);
        currentDilate2 = textMeshPro2.fontMaterial.GetFloat(ShaderUtilities.ID_FaceDilate);

        targetDilate1 = dilateStrength;  // ��������� ��������� ���� ��� ������� ������
        targetDilate2 = dilateStrength;  // ��������� ��������� ���� ��� ������� ������
    }

    void Update()
    {
        // ������ ������ �������� Dilate � �������� ��������� ��� ������� ������
        currentDilate1 = Mathf.MoveTowards(currentDilate1, targetDilate1, dilateSpeed * Time.deltaTime);
        currentDilate2 = Mathf.MoveTowards(currentDilate2, targetDilate2, dilateSpeed * Time.deltaTime);

        // ��������� �������� Dilate ��� ����� �������
        UpdateTextDilate(textMeshPro1, currentDilate1);
        UpdateTextDilate(textMeshPro2, currentDilate2);

        // ����� ���������� ����, ������ ���� �� ����� ������������� �� �������
        if (Mathf.Approximately(currentDilate1, targetDilate1))
        {
            targetDilate1 = newDilateValue1;  // ������ ����� ������������� �������� ��� ������� ������
        }
        if (Mathf.Approximately(currentDilate2, targetDilate2))
        {
            targetDilate2 = newDilateValue2;  // ������ ����� ������������� �������� ��� ������� ������
        }
    }

    // ������� ��� ���������� �������� Dilate ��� ������
    void UpdateTextDilate(TextMeshProUGUI textMeshPro, float dilateValue)
    {
        // �������� �������� ������
        Material material = textMeshPro.fontMaterial;

        // �������� �������� Dilate
        material.SetFloat(ShaderUtilities.ID_FaceDilate, dilateValue);
    }
}
