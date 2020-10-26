using System.Collections.Generic;
using UnityEngine;

namespace Completed
{
    public class EnemyManager
    {
        public List<Enemy> Enemies { get; }

        public EnemyManager(IEnumerable<GameObject> enemies)
        {
            if (Enemies == null)
            {
                Enemies = new List<Enemy>();
            }
            Enemies.Clear();

            foreach (var enemyGameObject in enemies)
            {
                var enemy = enemyGameObject.GetComponent<Enemy>();
                RegisterEnemy(enemy);
                enemy.Init();
            }
        }

        private void RegisterEnemy(Enemy enemy)
        {
            Enemies.Add(enemy);
        }

        public bool HasEnemies()
        {
            return Enemies.Count == 0;
        }
    }
}