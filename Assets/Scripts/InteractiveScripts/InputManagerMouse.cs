using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputManagerMouse : MonoBehaviour
{
    [SerializeField] private Camera sceneCamera;
    [SerializeField] private Vector3 lastPosition;
    [SerializeField] private LayerMask placementLayerMask;

    public event Action OnClicked, OnExit;    // Событие, вызываемое при клике и выходе 

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))      // Проверка нажатия левой кнопки мыши.
            OnClicked?.Invoke();              // Если есть подписчики, вызывает событие OnClicked.
        if (Input.GetKeyDown(KeyCode.Escape))
            OnExit?.Invoke();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            print(lastPosition);
        }
    }

    // Метод для проверки, находится ли указатель мыши над элементами UI.
    public bool IsPointerOverUI() => EventSystem.current.IsPointerOverGameObject();

    // Метод для получения позиции на карте, выбранной мышью.
    public Vector3 GetSelectedMapPosition()
    {
        Vector3 mousePos = Input.mousePosition;                // Получает текущую позицию мыши на экране.
        mousePos.z = sceneCamera.nearClipPlane;                // Устанавливает корректное значение глубины для проекции.
        Ray ray = sceneCamera.ScreenPointToRay(mousePos);      // Создает луч из камеры через позицию курсора на экране.
        RaycastHit hit;

        // Выполняет Raycast и, если он попал в объект, обновляет lastPosition.
        if (Physics.Raycast(ray, out hit, 100, placementLayerMask))
        {
            lastPosition = hit.point;
        }
        return lastPosition;
    }
}
