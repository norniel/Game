using System.Linq;
using Engine.Objects.Animals;
using Engine.Tools;

namespace Engine.States
{
    class Fleeing : IState
    {
        private readonly Animal _animal;
        private int _fleeingInvisible;
        private Vector _vector;

        public Fleeing(Animal animal)
        {
            _animal = animal;
        }

        public void Act()
        {
            GetFleeVector2();
        }

        private void GetFleeVector2()
        {
            var enemies = _animal.GetEnemies()?
                .Distinct().Select(mo => new
                {
                    Mo = mo,
                    Distance = Point.Distance(mo.Position, _animal.Position)
                }).ToList();

            if (enemies == null || !enemies.Any())
            {
                if (_fleeingInvisible > 5 || _vector == null)
                {
                    _animal.StateEvent.FireEvent();
                    return;
                }

                _fleeingInvisible++;

                CalculateFinalVectorAndPosition(_vector);

                return;
            }

            _fleeingInvisible = 0;

            var maxDistance = enemies.Max(e => e.Distance);
            var antiDistanceSum = enemies.Sum(e => maxDistance + 1 - e.Distance);

            if (maxDistance < 0.0001)
            {
                CalculateFinalVectorAndPosition(_vector ?? new Vector(1.0, 1.0).Normalize());
                return;
            }

            var newVector = new Vector();

            foreach (var enemy in enemies)
            {
                var tVector = Vector.FromPoints(enemy.Mo.Position, _animal.Position).Normalize();
                newVector += tVector.Multiply((maxDistance + 1 - enemy.Distance) / antiDistanceSum);
            }

            CalculateFinalVectorAndPosition(newVector.Normalize());
        }

        private void CalculateFinalVectorAndPosition(Vector newVector)
        {
            var vectorForFree = _animal.GetNearestPassibleVector(newVector);

            if (_vector != null)
            {
                vectorForFree += _vector;
                vectorForFree = vectorForFree.Normalize();
            }

            _animal.Angle = vectorForFree.Angle();

            var x = (int) (vectorForFree.X * _animal.Speed / 10 + _animal.Position.X);
            var y = (int) (vectorForFree.Y * _animal.Speed / 10 + _animal.Position.Y);

            _animal.Position = new Point(x < 0 ? 0 : x >= Map.MAP_WIDTH ? Map.MAP_WIDTH - 1 : x,
                y < 0 ? 0 : y >= Map.MAP_HEIGHT ? Map.MAP_HEIGHT - 1 : y);

            _vector = vectorForFree;
        }

        private void GetFleeVector()
        {
            var enemies = _animal.GetEnemies()?
                .Distinct().Select(mo => new
                {
                    Mo = mo,
                    Distance = Point.Distance(mo.Position, _animal.Position)
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
                        var distance = Point.Distance(enemies[i].Mo.Position, enemies[j].Mo.Position);

                        if (maxDistance < distance)
                        {
                            maxI = i;
                            maxJ = j;
                            maxDistance = distance;
                        }
                    }
                }

                endPoint = new Point((enemies[maxI].Mo.Position.X + enemies[maxJ].Mo.Position.X) / 2,
                    (enemies[maxI].Mo.Position.Y + enemies[maxJ].Mo.Position.Y) / 2);
            }

            var distanceToEnemies = Point.Distance(_animal.Position, endPoint);

            if (distanceToEnemies >= 0.00001)
            {
                var dx = -((double) endPoint.X - _animal.Position.X) / distanceToEnemies;
                var dy = -((double) endPoint.Y - _animal.Position.Y) / distanceToEnemies;

                var vectorForFree = _animal.GetNearestPassibleVector(new Vector(dx, dy));

                _animal.Angle = vectorForFree.Angle();
                /*
                                if (Math.Abs(dx) >= 0.0001)
                                    _animal.Angle = (180 * Math.Atan(dy / dx) / Math.PI) + (dx > 0 ? 180 : 0);
                                else
                                    _animal.Angle = (dy < 0) ? 90 : 270;
                                */
                var x = (int) (vectorForFree.X * _animal.Speed / 10 + _animal.Position.X);
                var y = (int) (vectorForFree.Y * _animal.Speed / 10 + _animal.Position.Y);

                _animal.Position = new Point(x < 0 ? 0 : x >= Map.MAP_WIDTH ? Map.MAP_WIDTH - 1 : x,
                    y < 0 ? 0 : y >= Map.MAP_HEIGHT ? Map.MAP_HEIGHT - 1 : y);
            }
        }

        public bool ShowActing => false;
    }
}