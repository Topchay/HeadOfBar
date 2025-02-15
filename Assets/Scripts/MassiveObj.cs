using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MassiveObj : MonoBehaviour
{
    [SerializeField] private Transform tr;
    [SerializeField] private GameObject[] gameObj;
    [SerializeField] private List<GameObject> listObj = new List<GameObject>();

    private void dopM()
    {
        int count = tr.childCount;
        gameObj = new GameObject[count];
        for (int i = 0; i < count; i++)
        {
            print(tr.GetChild(i).gameObject.name);
            gameObj[i] = tr.GetChild(i).gameObject;
            listObj.Add(tr.GetChild(i).gameObject);
        }
    }

    private void MultyMassive()
    {
        int[][] multM = new int[3][];
        multM[0] = new int[5];
        multM[1] = new int[6];
        multM[2] = new int[4];

        List<int>[] multL = new List<int>[4];
        for (int i = 0; i < multL.Length; i++) multL[i] = new List<int>();

        object[] objM = new object[4];
        objM[0] = 7;
        objM[1] = "Текст";
        objM[2] = true;
        objM[3] = 7.6f;

        print(objM[0]);
        objM[1] += 7.ToString();
    }

    private void Start()
    {
        List<Student> students = new List<Student>();

        for (int i = 0; i < 5; i++)
        {
            Student s1 = new Student(i + "nameS1");
            s1.ID = students.Count;
            students.Add(s1);
        }

        for (int i = 0; i < students.Count; i++)
        {
            print(students[i].StudentInfo());
        }

        students[0].Data = DateTime.Now;
    }
}

public class Student
{
    private int id;
    private string name;
    private DateTime data;

    public int Age { get; set; }

    public int ID
    {
        get { return id; } 
        set { id = value+1; }
    }

    public DateTime Data
    {
        get { return data; }
        set { }
    }

    public Student(string name)
    {
        this.name = name;
        id = 1;

        data = DateTime.Now;
    }

    public string StudentInfo()
    {
        return $"{id}\n{name}\n{data}";
    }
}
