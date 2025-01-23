using Inquisitor;
using UnityEditor;
using UnityEngine;

namespace TestEditor
{
    [CustomEditor(typeof(CitizenController))]
    public class CitizenWaypointEditor : Editor
    {
        // Флаг, позволяющий включать/выключать "режим расстановки точек".
        private bool editingPoints = false;

        // Этот метод вызывается при отрисовке Scene View, если объект с данным скриптом выбран
        void OnSceneGUI()
        {
            // Приводим target к типу нашего скрипта
            InquisitorController myScript = (InquisitorController) target;

            // Проверяем, включён ли режим редактирования
            if (!editingPoints) return;

            // Получаем текущее событие
            Event e = Event.current;

            // Если нажата левая кнопка мыши без зажатого Alt (зажатый Alt обычно вращает сцену)
            if (e.type == EventType.MouseDown && e.button == 0 && !e.alt)
            {
                // Бросаем луч в сцену из точки клика
                Ray ray = HandleUtility.GUIPointToWorldRay(e.mousePosition);

                // Если нужно ставить точки только на объекты с коллайдерами:
                if (Physics.Raycast(ray, out RaycastHit hit))
                {
                    // Создаём пустой объект
                    GameObject newPoint = new GameObject("Point");

                    // Фиксируем операцию создания для возможности отката (Undo)
                    Undo.RegisterCreatedObjectUndo(newPoint, "Create point");

                    // Ставим объект в точку пересечения луча и коллайдера
                    newPoint.transform.position = hit.point;

                    // Добавляем его Transform в список
                    Undo.RecordObject(myScript, "Add point to list");
                    myScript.waypoints.Add(newPoint.transform);

                    // Помечаем скрипт как изменённый, чтобы Unity понял, что нужно сохранить эти изменения
                    EditorUtility.SetDirty(myScript);
                }

                // Сообщаем системе, что событие мы «израсходовали»
                e.Use();
            }
        }

        // Отображаем дополнительный интерфейс в инспекторе
        public override void OnInspectorGUI()
        {
            // Отрисовываем стандартные поля (список points и т.д.)
            DrawDefaultInspector();

            // Кнопка (точнее, Toggle) для включения/выключения режима редактирования
            editingPoints = GUILayout.Toggle(editingPoints, "Edit Points in Scene");

            if (editingPoints)
            {
                EditorGUILayout.HelpBox("Нажмите ЛКМ в окне сцены, чтобы добавить новую точку.", MessageType.Info);
            }
        }
    }
}