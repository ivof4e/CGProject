﻿using System;
using System.Collections.Generic;
using System.Drawing;

namespace Draw
{
    /// <summary>
    /// Класът правоъгълник е основен примитив, който е наследник на базовия Shape.
    /// </summary>
    [Serializable]
    public class GroupShape : Shape
    {
        #region Constructor

        public GroupShape(RectangleF rect) : base(rect)
        {
        }

        public GroupShape(RectangleShape rectangle) : base(rectangle)
        {
        }

        public List<Shape> SubItems { get; set; }

        public override Color FillColor
        {
            set
            {
                foreach (var item in SubItems)
                {
                    item.FillColor = value;
                }
            }
        }

        #endregion

        public override PointF Location
        {
            set {
                float dx = value.X - Location.X;
                float dy = value.Y - Location.Y;
                base.Location = value;
                foreach (var item in SubItems)
                {
                    item.Location = new PointF(item.Location.X + dx, item.Location.Y + dy);
                }
            }
        }

        /// <summary>
        /// Проверка за принадлежност на точка point към правоъгълника.
        /// В случая на правоъгълник този метод може да не бъде пренаписван, защото
        /// Реализацията съвпада с тази на абстрактния клас Shape, който проверява
        /// дали точката е в обхващащия правоъгълник на елемента (а той съвпада с
        /// елемента в този случай).
        /// </summary>
        public override bool Contains(PointF point)
        {
            if (base.Contains(point))
            {
                // Проверка дали е в обекта само, ако точката е в обхващащия правоъгълник.
                // В случая на правоъгълник - директно връщаме true

                foreach (var item in SubItems)
                {
                    if (item.Contains(point)) return true;
                }
                return false;
        }
            else
                // Ако не е в обхващащия правоъгълник, то неможе да е в обекта и => false
                return false;
        }

        /// <summary>
        /// Частта, визуализираща конкретния примитив.
        /// </summary>
        public override void DrawSelf(Graphics grfx)
        {
            base.DrawSelf(grfx);

            foreach (var item in SubItems)
            {
                item.DrawSelf(grfx);
            }
        }
    }
}
