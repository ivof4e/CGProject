using Draw.src.Model;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace Draw
{
	/// <summary>
	/// Класът, който ще бъде използван при управляване на диалога.
	/// </summary>
	public class DialogProcessor : DisplayProcessor
	{
		#region Constructor
		
		public DialogProcessor()
		{
		}
		
		#endregion
		
		#region Properties
		
		/// <summary>
		/// Избран елемент.
		/// </summary>
		private List<Shape> selection = new List<Shape>();
		public List<Shape> Selection {
			get { return selection; }
			set { selection = value; }
		}
		
		/// <summary>
		/// Дали в момента диалога е в състояние на "влачене" на избрания елемент.
		/// </summary>
		private bool isDragging;
		public bool IsDragging
        {
			get { return isDragging; }
			set { isDragging = value; }
		}
		
		/// <summary>
		/// Последна позиция на мишката при "влачене".
		/// Използва се за определяне на вектора на транслация.
		/// </summary>
		private PointF lastLocation;
		public PointF LastLocation
        {
			get { return lastLocation; }
			set { lastLocation = value; }
		}
		
		#endregion
		
		/// <summary>
		/// Проверява дали дадена точка е в елемента.
		/// Обхожда в ред обратен на визуализацията с цел намиране на
		/// "най-горния" елемент т.е. този който виждаме под мишката.
		/// </summary>
		/// <param name="point">Указана точка</param>
		/// <returns>Елемента на изображението, на който принадлежи дадената точка.</returns>
		public Shape ContainsPoint(PointF point)
		{
			for(int i = ShapeList.Count - 1; i >= 0; i--){
				if (ShapeList[i].Contains(point)){
					//ShapeList[i].FillColor = Color.White;
						
					return ShapeList[i];
				}	
			}
			return null;
		}
		
		/// <summary>
		/// Транслация на избраният елемент на вектор определен от <paramref name="p>p</paramref>
		/// </summary>
		/// <param name="p">Вектор на транслация.</param>
		public void TranslateTo(PointF p)
		{
			foreach(var item in Selection) {
                item.Location = new PointF(item.Location.X + p.X - lastLocation.X, item.Location.Y + p.Y - lastLocation.Y);	
			}
            lastLocation = p;
        }
        /// <summary>
		/// Добавя примитив - правоъгълник на произволно място върху клиентската област.
		/// </summary>
		public void AddRandomRectangle()
        {
            Random rnd = new Random();
            int x = rnd.Next(100, 1000);
            int y = rnd.Next(100, 600);

            RectangleShape rect = new RectangleShape(new Rectangle(x, y, 100, 200));
            rect.FillColor = Color.White;
            rect.BorderColor = Color.Black;

            ShapeList.Add(rect);
        }
        // Добавяне на рандом Квадрат
        public void AddRandomSquare()
        {
            Random rnd = new Random();
            int x = rnd.Next(100, 1000);
            int y = rnd.Next(100, 600);

            SquareShape square = new SquareShape(new Rectangle(x, y, 100, 100));
            square.FillColor = Color.White;
            square.BorderColor = Color.HotPink;

            ShapeList.Add(square);
        }

        // Добавяне на рандом Триъгълник
        public void AddRandomTriangle()
        {
            Random rnd = new Random();
            int x = rnd.Next(100, 1000);
            int y = rnd.Next(100, 600);

            TriangleShape triangle = new TriangleShape(new Rectangle(x, y, 100, 100));
            triangle.FillColor = Color.White;
            triangle.BorderColor = Color.Blue;

            ShapeList.Add(triangle);
        }

        // Добавяне на рандом Елипса
        public void AddRandomEllipse()
        {
            Random rnd = new Random();
            int x = rnd.Next(100, 1000);
            int y = rnd.Next(100, 600);

            EllipseShape ellipse = new EllipseShape(new Rectangle(x, y, 200, 100));
            ellipse.FillColor = Color.White;
            ellipse.BorderColor = Color.Green;

            ShapeList.Add(ellipse);
        }

        // Добавяне на рандом Кръг
        public void AddRandomCircle()
        {
            Random rnd = new Random();
            int x = rnd.Next(100, 1000);
            int y = rnd.Next(100, 600);

            CircleShape circle = new CircleShape(new Rectangle(x, y, 100, 100));
            circle.FillColor = Color.White;
            circle.BorderColor = Color.Purple;

            ShapeList.Add(circle);
        }

        public void SetFillColor(Color color)
        {
            foreach (var item in Selection)
            {
                item.FillColor = color;
            }
        }

        public override void Draw(Graphics grfx)
        {
            base.Draw(grfx);
            foreach (var item in Selection)
            {
                grfx.DrawRectangle(Pens.Black, item.Location.X - 3, item.Location.Y - 3, item.Width + 6, item.Height + 6);
            }
        }

        internal void SetFillColor(object color)
        {
            throw new NotImplementedException();
        }

        public void Group()
        {
            if (Selection.Count < 2) return;

            float minX = float.PositiveInfinity;
            float minY = float.PositiveInfinity;
            float maxX = float.NegativeInfinity;
            float maxY = float.NegativeInfinity;
            foreach (var item in Selection)
            {
                if (minX > item.Location.X) minX = item.Location.X;
                if (minY > item.Location.Y) minY = item.Location.Y;
                if (maxX < item.Location.X + item.Width) maxX = item.Location.X + item.Width;
                if (maxY < item.Location.Y + item.Height) maxY = item.Location.Y + item.Height;
            }

            var group = new GroupShape(new RectangleF(minX, minY, maxX - minX, maxY - minY));
            group.SubItems = Selection;

            foreach (var item in Selection)
            {
                ShapeList.Remove(item);
            }

            Selection = new List<Shape>();
            Selection.Add(group);

            ShapeList.Add(group);

        }
    }
}