using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media.Media3D;
using System.Windows.Threading;
using System.Windows;

namespace SnakeGame
{
    public class Snake
    {
        public enum Direction { Up, Down, Left, Right }

        public List<Point> Body { get; private set; }
        private Direction direction;
        private Direction nextDirection;

        public Snake(int startX, int startY)
        {
            Body = new List<Point> { new Point(startX, startY) };
            direction = Direction.Right;
            nextDirection = Direction.Right;
        }

        public void Move()
        {
            direction = nextDirection;
            var head = Body.First();
            var newHead = GetNewHeadPosition(head, direction);
            Body.Insert(0, newHead);
            Body.RemoveAt(Body.Count - 1);
        }

        public void Grow()
        {
            var tail = Body.Last();
            var newTail = GetNewHeadPosition(tail, direction);
            Body.Add(newTail);
        }

        public void ChangeDirection(Direction newDirection)
        {
            if (CanChangeDirection(newDirection))
            {
                nextDirection = newDirection;
            }
        }


        private Point GetNewHeadPosition(Point currentHead, Direction dir)
        {
            switch (dir)
            {
                case Direction.Up:
                    return new Point(currentHead.X, currentHead.Y - 1);
                case Direction.Down:
                    return new Point(currentHead.X, currentHead.Y + 1);
                case Direction.Left:
                    return new Point(currentHead.X - 1, currentHead.Y);
                case Direction.Right:
                    return new Point(currentHead.X + 1, currentHead.Y);
                default:
                    throw new InvalidOperationException("Invalid direction.");
            }
        }

        private bool CanChangeDirection(Direction newDirection)
        {
            return !(newDirection == Direction.Up && direction == Direction.Down ||
                     newDirection == Direction.Down && direction == Direction.Up ||
                     newDirection == Direction.Left && direction == Direction.Right ||
                     newDirection == Direction.Right && direction == Direction.Left);
        }

        public bool CollidesWith(Point point)
        {
            return Body.Any(segment => segment.X == point.X && segment.Y == point.Y);
        }

        public bool CollidesWithItself()
        {
            return Body.Skip(1).Any(segment => segment.X == Body[0].X && segment.Y == Body[0].Y);
        }

        public bool IsOutOfBounds(int width, int height)
        {
            var head = Body.First();
            return head.X < 0 || head.X >= width || head.Y < 0 || head.Y >= height;
        }
    }
}
