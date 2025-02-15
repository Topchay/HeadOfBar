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

    public event Action OnClicked, OnExit;    // �������, ���������� ��� ����� � ������ 

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))      // �������� ������� ����� ������ ����.
            OnClicked?.Invoke();              // ���� ���� ����������, �������� ������� OnClicked.
        if (Input.GetKeyDown(KeyCode.Escape))
            OnExit?.Invoke();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            print(lastPosition);
        }
    }

    // ����� ��� ��������, ��������� �� ��������� ���� ��� ���������� UI.
    public bool IsPointerOverUI() => EventSystem.current.IsPointerOverGameObject();

    // ����� ��� ��������� ������� �� �����, ��������� �����.
    public Vector3 GetSelectedMapPosition()
    {
        Vector3 mousePos = Input.mousePosition;                // �������� ������� ������� ���� �� ������.
        mousePos.z = sceneCamera.nearClipPlane;                // ������������� ���������� �������� ������� ��� ��������.
        Ray ray = sceneCamera.ScreenPointToRay(mousePos);      // ������� ��� �� ������ ����� ������� ������� �� ������.
        RaycastHit hit;

        // ��������� Raycast �, ���� �� ����� � ������, ��������� lastPosition.
        if (Physics.Raycast(ray, out hit, 100, placementLayerMask))
        {
            lastPosition = hit.point;
        }
        return lastPosition;
    }
}
