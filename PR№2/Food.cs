using System;

namespace SnakeGame
{
    public class Food
    {
        public Point Position { get; private set; }

        public Food(int width, int height)
        {
            GenerateNewPosition(width, height);
        }

        public void GenerateNewPosition(int width, int height)
        {
            var random = new Random();
            Position = new Point(random.Next(width), random.Next(height));
        }
    }
}
