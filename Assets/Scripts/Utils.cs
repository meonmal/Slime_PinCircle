using UnityEngine;
using UnityEngine.UIElements;

public static class Utils
{
    /// <summary>
    /// 각도를 기준으로 원의 둘레 위치를 구함
    /// </summary>
    /// <param name="radius">원의 반지름</param>
    /// <param name="angle">각도</param>
    /// <returns>원의 반지름, 각도에 해당하는 둘레 위치</returns>
    public static Vector3 GetPositionFromAngle(float radius, float angle)
    {
        Vector3 position = Vector3.zero;

        angle = DegreeToRadian(angle);

        position.x = Mathf.Cos(angle) * radius;
        position.y = Mathf.Sin(angle) * radius;

        return position;
    }

    /// <summary>
    /// Degree 값을 Radian 값으로 변환
    /// 1도는 "PI/180" Radian
    /// angle도는 "PI/180 * angle" Radian
    /// </summary>
    /// <param name="angle"></param>
    /// <returns></returns>
    public static float DegreeToRadian(float angle)
    {
        return Mathf.PI * angle / 180;
    }

    /// <summary>
    /// Radian 값으 Degree 값으로 변환
    /// 1Radian은 "180/PI" 도
    /// angle Raian은 "180/PI * angle" 도
    /// </summary>
    /// <param name="angle"></param>
    /// <returns></returns>
    public static float RadiusToDegree(float angle)
    {
        return angle * (180 / Mathf.PI);
    }
}
