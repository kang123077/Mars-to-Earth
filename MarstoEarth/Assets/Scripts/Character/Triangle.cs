using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Triangle : MonoBehaviour
{
    [SerializeField] Texture _texture;
    float rot = 0;
    // Start is called before the first frame update
    void Start()
    {
        MakeTriagle();
    }
    void MakeTriagle()
    {

        Vector3[] vertice = new Vector3[]
        {
            new Vector3(.0f,.0f,.0f),
            new Vector3(.0f, 1.0f, .0f),
            new Vector3(1.0f, 1.0f, .0f),
            new Vector3(1.0f, .0f, .0f),
        };
        
        Vector2[] uvs = new Vector2[]
        {
            new Vector2(.0f,.0f),
            new Vector2(.0f, 1.0f),
            new Vector2(1.0f, 1.0f),
            new Vector2(1.0f, .0f),
        };

        //인덱스버퍼: 중복되는 정점을 줄이기 위해 사용
        int[] triagles = new int[] { 0, 1, 2, 0, 2, 3 };

        Mesh mesh = new Mesh();

        mesh.vertices = vertice;
        mesh.uv = uvs;
        mesh.triangles = triagles;

        mesh.RecalculateBounds();//바운딩박스 재구성
        mesh.RecalculateNormals();//법선벡터 재구성

        GetComponent<MeshFilter>().mesh = mesh;

        Material material = new Material(Shader.Find("Standard"));
        material.mainTexture = _texture;

        GetComponent<MeshRenderer>().material = material;
        


    }

    // Update is called once per frame
    void Update()
    {
        
        rot += Time.deltaTime * 100f;
        transform.rotation = Quaternion.Euler(-10f, 0f, rot);
    }
}
