using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : AbstractLevelController
{
    public override IEnumerator LevelLoopRoutine()
    {
        while(!isLevelSuccess)
        {
            m_BallController = Instantiate(m_BallPrefab, m_SpawnPoint.position, Quaternion.identity);

            while(!m_BallController.IsPlayerDied)
            {
                if(isLevelSuccess)
                {
                    break;
                }

                yield return null;
            }
        }

        m_BallController.PlayerControl = false;

        //Konfetileri göster

        //Konfetilerin bitmesini bekle

        //Dön
    }

    public override void Triggered()
    {
        isLevelSuccess = true;
    }
}
