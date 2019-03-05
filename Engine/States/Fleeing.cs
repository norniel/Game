﻿using System;
using System.Linq;
using Engine.Objects.Animals;
using MoreLinq;

namespace Engine.States
{
    class Fleeing :IState
    {
        private readonly Animal _animal;

        public Fleeing(Animal animal)
        {
            _animal = animal;
        }

        public void Act()
        {
            var enemies = _animal.GetEnemies()?
                .Distinct().Select(mo => new
                {
                    Mo = mo,
                    Distance = Math.Sqrt((mo.Position.X - _animal.Position.X) * (mo.Position.X - _animal.Position.X) +
                                         (mo.Position.Y - _animal.Position.Y) * (mo.Position.Y - _animal.Position.Y))
                }).GroupBy(mo => mo.Distance).OrderByDescending(gr => gr.Key).FirstOrDefault()?.ToList();

            if (enemies == null || !enemies.Any())
            {
                _animal.StateEvent.FireEvent();
                return;
            }

            var endPoint = enemies.First().Mo.Position;

            if (enemies.Count > 1)
            {
                var maxDistance = 0.0;
                var maxI = 0;
                var maxJ = 0;
                for (int i = 0; i < enemies.Count - 1; i++)
                {
                    for (int j = i + 1; j < enemies.Count; j++)
                    {
                        var distance = Math.Sqrt(
                            (enemies[i].Mo.Position.X - enemies[j].Mo.Position.X) *
                            (enemies[i].Mo.Position.X - enemies[j].Mo.Position.X) +
                            (enemies[i].Mo.Position.Y - enemies[j].Mo.Position.Y) *
                            (enemies[i].Mo.Position.Y - enemies[j].Mo.Position.Y));

                        if (maxDistance < distance)
                        {
                            maxI = i;
                            maxJ = j;
                            maxDistance = distance;
                        }
                    }
                }

                endPoint = new Point((enemies[maxI].Mo.Position.X + enemies[maxJ].Mo.Position.X)/2, (enemies[maxI].Mo.Position.Y + enemies[maxJ].Mo.Position.Y) / 2);
            }

            var distanceToEnemies = Math.Sqrt((_animal.Position.X - endPoint.X) * (_animal.Position.X - endPoint.X) + (_animal.Position.Y - endPoint.Y) * (_animal.Position.Y - endPoint.Y));

            if (distanceToEnemies >= 0.00001)
            {
                var dx = -(_animal.Position.X - (double)endPoint.X) / distanceToEnemies;
                var dy = -(_animal.Position.Y - (double)endPoint.Y) / distanceToEnemies;

                if (Math.Abs(dx) >= 0.0001)
                    _animal.Angle = (180 * Math.Atan(dy / dx) / Math.PI) + (dx > 0 ? 180 : 0);
                else
                    _animal.Angle = (dy < 0) ? 90 : 270;

                var x = (int) (_animal.Position.X - dx * _animal.Speed / 10);
                var y = (int) (_animal.Position.Y - dy * _animal.Speed / 10);

                _animal.Position = new Point( x < 0 ? 0: x >= Map.MAP_WIDTH ? Map.MAP_WIDTH - 1 : x , y < 0 ? 0 : y >= Map.MAP_HEIGHT ? Map.MAP_HEIGHT - 1 : y);
            }
        }

        public bool ShowActing { get; }
    }
}
