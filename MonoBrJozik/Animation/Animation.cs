using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;

namespace MonoBrJozik.Animation
{
    public class Animation
    {
        readonly List<AnimationFrame> _frames = new List<AnimationFrame>();
        TimeSpan _timeIntoAnimation;

        private TimeSpan Duration { get; set; }

        public void AddFrame(Rectangle rectangle, TimeSpan duration)
        {
            AnimationFrame newFrame = new AnimationFrame()
            {
                SourceRectangle = rectangle,
                Duration = duration
            };

            Duration = TimeSpan.FromSeconds(_frames.Sum(frame => frame.Duration.TotalSeconds));

            _frames.Add(newFrame);
        }

        public Rectangle? CurrentRectangle(int tick)
        {
            if (!_frames.Any())
                return null;

            var index = tick /10%_frames.Count;
            return _frames[index].SourceRectangle;
        }

        public void Update(GameTime gameTime)
        {
            var secondsIntoAnimation =
                _timeIntoAnimation.TotalSeconds + gameTime.ElapsedGameTime.TotalSeconds;

            var remainder = secondsIntoAnimation % Duration.TotalSeconds;

            _timeIntoAnimation = TimeSpan.FromSeconds(remainder);
        }
    }
}
