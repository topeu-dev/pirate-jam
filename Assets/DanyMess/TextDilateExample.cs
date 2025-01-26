using TMPro;
using UnityEngine;

public class TextDilateExample : MonoBehaviour
{
    public TextMeshProUGUI textMeshPro1;  // Первый текст
    public TextMeshProUGUI textMeshPro2;  // Второй текст

    // Публичные переменные для скорости и силы изменений
    public float dilateSpeed = 1.0f;  // Скорость изменения Dilate
    public float dilateStrength = 0.2f;  // Сила изменения Dilate

    // Публичные переменные для новых значений Dilate
    public float newDilateValue1 = 0.5f;  // Новое фиксированное значение для первого текста
    public float newDilateValue2 = 1.0f;  // Новое фиксированное значение для второго текста

    private float targetDilate1;  // Цель для Dilate первого текста
    private float targetDilate2;  // Цель для Dilate второго текста
    private float currentDilate1;  // Текущий параметр Dilate первого текста
    private float currentDilate2;  // Текущий параметр Dilate второго текста

    void Start()
    {
        // Инициализация начальных значений для Dilate
        currentDilate1 = textMeshPro1.fontMaterial.GetFloat(ShaderUtilities.ID_FaceDilate);
        currentDilate2 = textMeshPro2.fontMaterial.GetFloat(ShaderUtilities.ID_FaceDilate);

        targetDilate1 = dilateStrength;  // Установим начальную цель для первого текста
        targetDilate2 = dilateStrength;  // Установим начальную цель для второго текста
    }

    void Update()
    {
        // Плавно меняем параметр Dilate с заданной скоростью для первого текста
        currentDilate1 = Mathf.MoveTowards(currentDilate1, targetDilate1, dilateSpeed * Time.deltaTime);
        currentDilate2 = Mathf.MoveTowards(currentDilate2, targetDilate2, dilateSpeed * Time.deltaTime);

        // Обновляем значение Dilate для обоих текстов
        UpdateTextDilate(textMeshPro1, currentDilate1);
        UpdateTextDilate(textMeshPro2, currentDilate2);

        // После достижения цели, меняем цель на новую фиксированную из паблика
        if (Mathf.Approximately(currentDilate1, targetDilate1))
        {
            targetDilate1 = newDilateValue1;  // Задаем новое фиксированное значение для первого текста
        }
        if (Mathf.Approximately(currentDilate2, targetDilate2))
        {
            targetDilate2 = newDilateValue2;  // Задаем новое фиксированное значение для второго текста
        }
    }

    // Функция для обновления значения Dilate для текста
    void UpdateTextDilate(TextMeshProUGUI textMeshPro, float dilateValue)
    {
        // Получаем материал текста
        Material material = textMeshPro.fontMaterial;

        // Изменяем параметр Dilate
        material.SetFloat(ShaderUtilities.ID_FaceDilate, dilateValue);
    }
}
