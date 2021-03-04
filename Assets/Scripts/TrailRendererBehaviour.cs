using UnityEngine;

public class TrailRendererBehaviour : MonoBehaviour
{
    public TrailRenderer trailrenderer;
    Transform mSelfTrans;

    /// <summary>
    /// 线段的z轴坐标
    /// </summary>
    const float LINE_POS_Z = 10;

    void Awake()
    {
        mSelfTrans = transform;
    }


    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            // 停止划线，防止坐标瞬移导致出现一个拖尾
            trailrenderer.emitting = false;
            var mousPos = Input.mousePosition;
            mSelfTrans.position = Camera.main.ScreenToWorldPoint(new Vector3(mousPos.x, mousPos.y, LINE_POS_Z));
            return;
        }
        
        
        if(Input.GetMouseButton(0))
        {
            trailrenderer.emitting = true;
            var mousPos = Input.mousePosition;
            mSelfTrans.position = Camera.main.ScreenToWorldPoint(new Vector3(mousPos.x, mousPos.y, LINE_POS_Z));
        }
    }
}
