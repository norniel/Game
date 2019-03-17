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
            GetFleeVector();
        }

        private void GetFleeVector()
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

            if (x < 0)
            {
                x = 0;
            }

            if (y < 0)
            {
                y = 0;
            }

            if (x >= Map.MAP_WIDTH)
            {
                x = Map.MAP_WIDTH - 1;
            }

            if (y >= Map.MAP_HEIGHT)
            {
                y = Map.MAP_HEIGHT - 1;
            }

           _animal.Position = new Point(x, y);

            _vector = vectorForFree;
        }

        public bool ShowActing => false;
    }
}