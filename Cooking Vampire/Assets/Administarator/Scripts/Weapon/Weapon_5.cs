using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_5 : Weapon
{
    [Title("�߻簢")]
    public float coneAngle;
    public int maxCount;

    public override IEnumerator Active()
    {
        int projectileCount = gm.stat.Get_COUNT(stat.count);

        while(projectileCount > 0)
        {
            ShotGun(Mathf.Min(maxCount, projectileCount));
            projectileCount -= maxCount;
            yield return new WaitForSeconds(0.5f);
        }
    }

    protected override void MaxLevel()
    {
    }

    private void ShotGun(int projectileCount)
    {
        Vector2 direction = player.moveController.inputLook;
        if (direction == Vector2.zero)
            direction = RanPos().normalized;

        for(int i = 0; i < projectileCount; i++)
        {
            float angleOffset = (i - (projectileCount - 1) / 2f) * (coneAngle / (projectileCount - 1));
            Quaternion rotation = Quaternion.AngleAxis(angleOffset, Vector3.forward);
            Vector3 coneDirection = rotation * direction;

            FireDir(coneDirection);
        }
    }
}
