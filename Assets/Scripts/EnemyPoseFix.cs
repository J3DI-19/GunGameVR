using UnityEngine;

public class EnemyPoseFix : MonoBehaviour
{
    public Transform leftUpperArm;
    public Transform leftLowerArm;

    public Transform rightUpperArm;
    public Transform rightLowerArm;

    void Start()
    {
        ApplyPose();
    }

    void ApplyPose()
    {
        // LEFT ARM
        if (leftUpperArm != null)
            leftUpperArm.localRotation = Quaternion.Euler(-32.56f, -51.02f, 64f);

        if (leftLowerArm != null)
            leftLowerArm.localRotation = Quaternion.Euler(-7.03f, 24.98f, 14.73f);

        // RIGHT ARM
        if (rightUpperArm != null)
            rightUpperArm.localRotation = Quaternion.Euler(-92.63f, -0.1f, -81.27f);

        if (rightLowerArm != null)
            rightLowerArm.localRotation = Quaternion.Euler(-17.42f, -1.72f, -2f);
    }
}