using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeType1 : MonoBehaviour
{
    public Sprite[] cubeSprite;

    public float moveSpeed=3;
    //这个是图片渲染的结果️
    private SpriteRenderer sr;
    // Start is called before the first frame update
    private void Move()
    {
        float v = Input.GetAxisRaw("Vertical");
        transform.Translate(v*moveSpeed*Time.fixedDeltaTime*Vector3.up,Space.World);
        if (v < 0&&v > 0)
        {
            sr.sprite = cubeSprite[0];
        }
            
    }
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }
    
    
    
    // Update is called once per frame
    void Update()
    {
        
    }
}
