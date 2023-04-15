using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeCreator : MonoBehaviour
{
    [SerializeField] private Texture _texture;


    private float _rot = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        MakeCube();
    }

    void MakeCube()
    {
        // 정점 버퍼
        Vector3[] vertice = new Vector3[]
        {
            // 앞면
            new Vector3(-1.0f, -1.0f, -1.0f), // 0
            new Vector3(-1.0f, 1.0f, -1.0f), // 1
            new Vector3(1.0f, 1.0f, -1.0f), // 2
            new Vector3(1.0f, -1.0f, -1.0f), // 3
                                              
            // 윗면
            new Vector3(-1.0f, 1.0f, -1.0f), // 4
            new Vector3(-1.0f, 1.0f, 1.0f), // 5
            new Vector3(1.0f, 1.0f, 1.0f), // 6
            new Vector3(1.0f, 1.0f, -1.0f), // 7

            // 뒷면
            new Vector3(-1.0f, 1.0f, 1.0f), // 8
            new Vector3(-1.0f, -1.0f, 1.0f), // 9
            new Vector3(1.0f, -1.0f, 1.0f),  // 10
            new Vector3(1.0f, 1.0f, 1.0f), // 11

            // 밑변
            new Vector3( -1.0f, -1.0f, 1.0f), // 12
            new Vector3(-1.0f, -1.0f, -1.0f), // 13
            new Vector3(1.0f, -1.0f, -1.0f), // 14
            new Vector3(1.0f, -1.0f, 1.0f), // 15

            // 좌측면
            new Vector3(-1.0f, -1.0f, 1.0f), // 16
            new Vector3(-1.0f, 1.0f, 1.0f), // 17
            new Vector3(-1.0f, 1.0f, -1.0f), // 18
            new Vector3(-1.0f, -1.0f, -1.0f), // 19

            // 우측면
            new Vector3(1.0f, -1.0f, -1.0f), // 20
            new Vector3(1.0f, 1.0f, -1.0f), // 21
            new Vector3(1.0f, 1.0f, 1.0f),  // 22
            new Vector3(1.0f, -1.0f, 1.0f) // 23

        };

        // 인덱스버퍼
        int[] triangles = new int[]
        {
            0, 1, 2, // 앞면
            0, 2, 3,

            4, 5, 6, // 윗면
            4, 6, 7,

            8, 9, 10, // 뒷면
            8, 10, 11,

            12, 13, 14, // 밑면
            12, 14, 15,

            16, 17, 18, // 좌측면
            16, 18, 19,

            20, 21, 22, // 우측면
            20, 22, 23

        };


        // uv 좌표
        Vector2[] uvs = new Vector2[]
        {
            new Vector2(0.0f, 0.66f),
            new Vector2(0.0f, 1f),
            new Vector2(0.5f, 1f),
            new Vector2(0.5f, 0.66f),

           new Vector2(0.0f, 0.66f),
            new Vector2(0.0f, 1f),
            new Vector2(0.5f, 1f),
            new Vector2(0.5f, 0.66f),

            new Vector2(0.0f, 0.66f),
            new Vector2(0.0f, 1f),
            new Vector2(0.5f, 1f),
            new Vector2(0.5f, 0.66f),

            new Vector2(0.0f, 0.66f),
            new Vector2(0.0f, 1f),
            new Vector2(0.5f, 1f),
            new Vector2(0.5f, 0.66f),

            new Vector2(0.0f, 0.66f),
            new Vector2(0.0f, 1f),
            new Vector2(0.5f, 1f),
            new Vector2(0.5f, 0.66f),

            new Vector2(0.0f, 0.66f),
            new Vector2(0.0f, 1f),
            new Vector2(0.5f, 1f),
            new Vector2(0.5f, 0.66f),

        };



        Mesh mesh = new Mesh(); // 메쉬 객체 생성

        mesh.vertices = vertice;
        mesh.triangles = triangles;
        mesh.uv = uvs;

        mesh.RecalculateBounds();   // 바운딩 박스 재구성
        mesh.RecalculateNormals();  // 법석벡터를 재구성

        GetComponent<MeshFilter>().mesh = mesh; // mesh 데이타 전달(mesh 데이타 관리)

        Material material = new Material(Shader.Find("Standard"));
        material.SetTexture("_MainTex", _texture);

        GetComponent<MeshRenderer>().material = material;
    }

    // Update is called once per frame
    void Update()
    {
        _rot += Time.deltaTime * 30.0f;

        transform.rotation = Quaternion.Euler(10.0f, 0.0f, _rot);

    }
}