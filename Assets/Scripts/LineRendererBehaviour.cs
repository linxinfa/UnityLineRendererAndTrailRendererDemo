using UnityEngine;

public class LineRendererBehaviour : MonoBehaviour
{
    public LineRenderer linerenderer;
    /// <summary>
    /// 线段的z轴坐标
    /// </summary>
    const float LINE_POS_Z = 10;
    /// <summary>
    /// 线段的点的数量
    /// </summary>
    const int LINE_POS_CNT = 10;

    /// <summary>
    /// 坐标点的数组
    /// </summary>
    Vector3[] mLinePosList = new Vector3[LINE_POS_CNT];

    /// <summary>
    /// 鼠标的屏幕坐标
    /// </summary>
    Vector3 mMouseScreenPos;
    /// <summary>
    /// 是否是按下
    /// </summary>
    bool mFire = false;
    /// <summary>
    /// 上一次是否是按下
    /// </summary>
    bool mFirePre = false;
    /// <summary>
    /// 是否是刚按下
    /// </summary>
    bool mFireDown = false;
    /// <summary>
    /// 是否是抬起
    /// </summary>
    bool mFireUp = false;

    /// <summary>
    /// 坐标的起始和终止坐标点
    /// </summary>
    Vector2 mStart, mEnd;


    /// <summary>
    /// 坐标点索引
    /// </summary>
    int mLinePosIndex = 0;

    /// <summary>
    /// 线段的alpha通道值
    /// </summary>
    float mTrailAlpha = 0f;


    void Update()
    {
        // 鼠标的位置
        mMouseScreenPos = Input.mousePosition;
        mFireDown = false;
        mFireUp = false;

        mFire = Input.GetMouseButton(0);
        if (mFire && !mFirePre) mFireDown = true;
        if (!mFire && mFirePre) mFireUp = true;
        mFirePre = mFire;

        // 画线
        DrawLine();
        // 设置线段颜色，主要是设置alpha值，慢慢变淡
        SetLineColor();
    }

    void SetLineColor()
    {
        if (mTrailAlpha > 0)
        {
            // 黄色
            linerenderer.startColor = new Color(1, 1, 0, mTrailAlpha);
            // 红色
            linerenderer.endColor = new Color(1, 0, 0, mTrailAlpha);
            // 慢慢变透明
            mTrailAlpha -= Time.deltaTime * 2;
        }
    }

    /// <summary>
    /// 画线
    /// </summary>
    void DrawLine()
    {
        // 鼠标按下
        if (mFireDown)
        {
            mStart = mMouseScreenPos;
            mEnd = mMouseScreenPos;

            mLinePosIndex = 0;
            mTrailAlpha = 1;
            AddTrailPoint();
        }

        // 鼠标滑动中
        if (mFire)
        {
            mEnd = mMouseScreenPos;
            var pos1 = Camera.main.ScreenToWorldPoint(new Vector3(mStart.x, mStart.y, LINE_POS_Z));
            var pos2 = Camera.main.ScreenToWorldPoint(new Vector3(mEnd.x, mEnd.y, LINE_POS_Z));
            // 滑动距离超过0.1才算作一次有效的滑动
            if (Vector3.Distance(pos1, pos2) > 0.01f)
            {
                mTrailAlpha = 1;
                ++mLinePosIndex;
                // 添加坐标点到数组中
                AddTrailPoint();
            }

            mStart = mMouseScreenPos;
        }

        // 将坐标数组赋值给LineRenderer组件
        SetLineRendererPos();
    }

    /// <summary>
    /// 添加坐标点到数组中
    /// </summary>
    void AddTrailPoint()
    {
        if (mLinePosIndex < LINE_POS_CNT)
        {
            for (int i = mLinePosIndex; i < LINE_POS_CNT; ++i)
            {
                mLinePosList[i] = Camera.main.ScreenToWorldPoint(new Vector3(mEnd.x, mEnd.y, LINE_POS_Z));
            }
        }
        else
        {
            for (int i = 0; i < LINE_POS_CNT - 1; ++i)
            {
                mLinePosList[i] = mLinePosList[i + 1];
            }
            mLinePosList[LINE_POS_CNT - 1] = Camera.main.ScreenToWorldPoint(new Vector3(mEnd.x, mEnd.y, LINE_POS_Z));
        }
    }

    /// <summary>
    /// 将坐标数组赋值给LineRenderer组件
    /// </summary>
    void SetLineRendererPos()
    {
        for (int i = 0; i < LINE_POS_CNT; ++i)
        {
            linerenderer.SetPosition(i, mLinePosList[i]);
        }
    }
}
