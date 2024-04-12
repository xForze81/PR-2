using PR_2;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace SnakeGame
{
    public partial class MainWindow : Window
    {
        private const int Width = 49;
        private const int Height = 32;
        private const int Size = 20;
        private Snake Snake;
        private Food food;
        private int score;

        public MainWindow()
        {
            InitializeComponent();
            Snake = new Snake(Width / 2, Height / 2);
            food = new Food(Width, Height);
            score = 0;
            DrawGame();
            CompositionTarget.Rendering += UpdateGame;
        }
        private int SnakeSpeed = 300; // Задержка в миллисекундах между обновлениями

        private void UpdateGame(object sender, EventArgs e)
        {
            Snake.Move();
            if (Snake.CollidesWith(food.Position))
            {
                Snake.Grow();
                food.GenerateNewPosition(Width, Height);
                score++;
                scoreText.Text = $"Score: {score}";
            }

            if (Snake.IsOutOfBounds(Width, Height) || Snake.CollidesWithItself())
            {
                MessageBox.Show("Game Over! Collision with yourself or wall.");
                Close();
            }

            DrawGame();

            // Устанавливаем задержку перед следующим обновлением
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(SnakeSpeed);
            timer.Tick += UpdateGame;
            timer.Start();
        }



        private void DrawGame()
        {
            gameCanvas.Children.Clear();
            DrawBorder(); // Рисуем границы игрового поля
            DrawSnake();  // Рисуем змейку
            DrawFood();   // Рисуем еду
        }
        private void DrawBorder()
        {
            // Рисуем верхнюю границу
            DrawRectangle(0, 0, Width * Size, 3, Brushes.Red);
            // Рисуем нижнюю границу
            DrawRectangle(0, Height * Size - 3, Width * Size, 3, Brushes.Red);
            // Рисуем левую границу
            DrawRectangle(0, 0, 3, Height * Size, Brushes.Red);
            // Рисуем правую границу
            DrawRectangle(Width * Size - 3, 0, 3, Height * Size, Brushes.Red);
        }
        private void DrawSnake()
        {
            foreach (var segment in Snake.Body)
            {
                DrawRectangle(segment.X * Size, segment.Y * Size, Size, Size, Brushes.Green);
            }
        }

        private void DrawFood()
        {
            DrawText(food.Position.X * Size, food.Position.Y * Size, "f", Brushes.Red);
        }

        private void DrawText(int x, int y, string text, Brush color)
        {
            TextBlock tb = new TextBlock();
            tb.Text = text;
            tb.Foreground = color;
            tb.FontSize = Size;
            Canvas.SetLeft(tb, x);
            Canvas.SetTop(tb, y);
            gameCanvas.Children.Add(tb);
        }

        private void DrawRectangle(int x, int y, int width, int height, Brush color)
        {
            Rectangle rect = new Rectangle();
            rect.Fill = color;
            rect.Width = width;
            rect.Height = height;
            Canvas.SetLeft(rect, x);
            Canvas.SetTop(rect, y);
            gameCanvas.Children.Add(rect);
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.W:
                    Snake.ChangeDirection(Snake.Direction.Up);
                    break;
                case Key.S:
                    Snake.ChangeDirection(Snake.Direction.Down);
                    break;
                case Key.A:
                    Snake.ChangeDirection(Snake.Direction.Left);
                    break;
                case Key.D:
                    Snake.ChangeDirection(Snake.Direction.Right);
                    break;
            }
        }
    }
}
