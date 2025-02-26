using UnityEngine;
using System.Collections.Generic;
using NUnit.Framework;
public class EnemyManager : MonoBehaviour
{
    public List<Enemy> enemies = new List<Enemy>();

    private void Update()
    {
        foreach (Enemy enemy in enemies)
        {
            if (GameManager.Instance.gameState == GameState.Dead)
            { if (enemy.GetType() != typeof(EnemyBoss) && !enemy.dead) { enemy.dead = true; } }
            else
            {
                if (!enemy.gameObject.activeSelf && !enemy.dead && enemy.transform.position.x < 11 && enemy.transform.position.x > -11)
                { 
                    enemy.gameObject.SetActive(true);
                    if (enemy.stats.Type == EntityType.Boss) { Map.Instance.reachedBoss = true; }
                }
                if (enemy.stats.Type == EntityType.Boss && enemy.dead)
                {
                    var b = enemy as EnemyBoss;
                    if (b.winSawRef != null)
                    {
                        var win = Instantiate(b.winSawRef);
                        win.transform.position = b.transform.position;
                        b.winSawRef = null;
                    }
                }
            }
        }
    }

}
