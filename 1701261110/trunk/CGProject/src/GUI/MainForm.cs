using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace Draw
{
    /// <summary>
    /// Върху главната форма е поставен потребителски контрол,
    /// в който се осъществява визуализацията
    /// </summary>
    public partial class MainForm : Form
    {
        /// <summary>
        /// Агрегирания диалогов процесор във формата улеснява манипулацията на модела.
        /// </summary>
        private DialogProcessor dialogProcessor = new DialogProcessor();

        public MainForm()
        {
            //
            // The InitializeComponent() call is required for Windows Forms designer support.
            //
            InitializeComponent();

            //
            // TODO: Add constructor code after the InitializeComponent() call.
            //
        }

        /// <summary>
        /// Изход от програмата. Затваря главната форма, а с това и програмата.
        /// </summary>
        void ExitToolStripMenuItemClick(object sender, EventArgs e)
        {
            Close();
        }

        /// <summary>
        /// Събитието, което се прихваща, за да се превизуализира при изменение на модела.
        /// </summary>
        void ViewPortPaint(object sender, PaintEventArgs e)
        {
            dialogProcessor.ReDraw(sender, e);
        }

        /// <summary>
        /// Прихващане на координатите при натискането на бутон на мишката и проверка (в обратен ред) дали не е
        /// щракнато върху елемент. Ако е така то той се отбелязва като селектиран и започва процес на "влачене".
        /// Промяна на статуса и инвалидиране на контрола, в който визуализираме.
        /// Реализацията се диалогът с потребителя, при който се избира "най-горния" елемент от екрана.
        /// </summary>
        void ViewPortMouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (pickUpSpeedButton.Checked) {
                var sel = dialogProcessor.ContainsPoint(e.Location);
                if (sel != null) {
                    if (dialogProcessor.Selection.Contains(sel))
                        dialogProcessor.Selection.Remove(sel);
                    else
                        dialogProcessor.Selection.Add(sel);

                    statusBar.Items[0].Text = "Последно действие: Селекция на примитив";
                    dialogProcessor.IsDragging = true;
                    dialogProcessor.LastLocation = e.Location;
                    viewPort.Invalidate();
                }
            }
        }

        /// <summary>
        /// Прихващане на преместването на мишката.
        /// Ако сме в режм на "влачене", то избрания елемент се транслира.
        /// </summary>
        void ViewPortMouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (dialogProcessor.IsDragging) {
                if (dialogProcessor.Selection != null) statusBar.Items[0].Text = "Последно действие: Влачене";
                dialogProcessor.TranslateTo(e.Location);
                viewPort.Invalidate();
            }
        }

        /// <summary>
        /// Прихващане на отпускането на бутона на мишката.
        /// Излизаме от режим "влачене".
        /// </summary>
        void ViewPortMouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            dialogProcessor.IsDragging = false;
        }

        /// <summary>
        /// Бутон, който поставя на произволно място кръг със зададените размери.
        /// Променя се лентата със състоянието и се инвалидира контрола, в който визуализираме.
        /// </summary>
        private void CircleButtonClick(object sender, EventArgs e)
        {
            dialogProcessor.AddRandomCircle();
            statusBar.Items[0].Text = "Последно действие: Рисуване на Кръг";
            viewPort.Invalidate();
        }

        /// <summary>
        /// Бутон, който поставя на произволно място елипса със зададените размери.
        /// Променя се лентата със състоянието и се инвалидира контрола, в който визуализираме.
        /// </summary>
        private void EllipseButtonClick(object sender, EventArgs e)
        {
            dialogProcessor.AddRandomEllipse();
            statusBar.Items[0].Text = "Последно действие: Рисуване на Елипса";
            viewPort.Invalidate();
        }

        /// <summary>
        /// Бутон, който поставя на произволно място квадрат със зададените размери.
        /// Променя се лентата със състоянието и се инвалидира контрола, в който визуализираме.
        /// </summary>
        private void SquareButtonClick(object sender, EventArgs e)
        {
            dialogProcessor.AddRandomSquare();
            statusBar.Items[0].Text = "Последно действие: Рисуване на Квадрат";
            viewPort.Invalidate();
        }

        /// <summary>
		/// Бутон, който поставя на произволно място правоъгълник със зададените размери.
		/// Променя се лентата със състоянието и се инвалидира контрола, в който визуализираме.
		/// </summary>
		void RectangleButtonClick(object sender, EventArgs e)
        {
            dialogProcessor.AddRandomRectangle();
            statusBar.Items[0].Text = "Последно действие: Рисуване на Правоъгълник";
            viewPort.Invalidate();
        }

        /// <summary>
        /// Бутон, който поставя на произволно място триъгълник със зададените размери.
        /// Променя се лентата със състоянието и се инвалидира контрола, в който визуализираме.
        /// </summary>
        private void TriangleButtonClick(object sender, EventArgs e)
        {
            dialogProcessor.AddRandomTriangle();
            statusBar.Items[0].Text = "Последно действие: Рисуване на Триъгълник";
            viewPort.Invalidate();
        }

        private void pickUpSpeedButton_Click(object sender, EventArgs e)
        {

        }

        private void speedMenu_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        /// <summary>
        /// Бутон, който групира формите.
        /// </summary>
        private void GroupShapesButtonClick(object sender, EventArgs e)
        {
            dialogProcessor.Group();
            viewPort.Invalidate();
        }

        /// <summary>
        /// Бутон, който разгрупира формите.
        /// </summary>
        private void UnGroupShapesButtonClick(object sender, EventArgs e)
        {
            dialogProcessor.UnGroup();
            viewPort.Invalidate();
        }

        /// <summary>
        /// Бутон, който отваря прозорец със цветове.
        /// </summary>
        private void ColorDialogClick(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                dialogProcessor.SetFillColor(colorDialog1.Color);
                viewPort.Invalidate();
            }
        }

        /// <summary>
        /// Бутон от падащото меню, който трие форми.
        /// </summary>
        private void DeleteToolStripMenuItemClick(object sender, EventArgs e)
        {
            dialogProcessor.Delete();
            viewPort.Invalidate();
        }

        /// <summary>
        /// Бутон от падащото меню, който избира форми.
        /// </summary>
        private void SelectAllToolStripMenuItemClick(object sender, EventArgs e)
        {
            dialogProcessor.SelectAll();
            viewPort.Invalidate();
        }

        /// <summary>
        /// Бутон от падащото меню, който отваря файлове.
        /// </summary>
        private void OpenToolStripMenuItemClick(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                dialogProcessor.OpenFile(openFileDialog1.FileName);
                viewPort.Invalidate();
            }
        }

        /// <summary>
        /// Бутон от падащото меню, който записва файлове.
        /// </summary>
        private void SaveAsFileToolStripMenuItemClick(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                dialogProcessor.SaveAs(saveFileDialog1.FileName);
            }
        }

        /// <summary>
        /// Бутон от падащото меню, който записва изображения във формати ".jpg", ".png", ".bmp".
        /// </summary>
        private void SaveAsPictureToolStripMenuItemClick(object sender, EventArgs e)
        {
            Bitmap bmp = new Bitmap(width: viewPort.Width, height: viewPort.Height);
            Graphics g = Graphics.FromImage(bmp);
            Rectangle rect = viewPort.RectangleToScreen(viewPort.ClientRectangle);
            g.CopyFromScreen(rect.Location, Point.Empty, viewPort.Size);
            g.Dispose();
            SaveFileDialog s = new SaveFileDialog();
            s.Filter = "png files|*.png|jpeg files|*.jpg|bitmaps files|*.bmp";
            if (s.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                if (File.Exists(s.FileName))
                {
                    File.Delete(s.FileName);
                }
                if (s.FileName.Contains(".jpg"))
                {
                    bmp.Save(s.FileName, ImageFormat.Jpeg);
                }
                else if (s.FileName.Contains(".png"))
                {
                    bmp.Save(s.FileName, ImageFormat.Png);
                }
                else if (s.FileName.Contains(".bmp"))
                {
                    bmp.Save(s.FileName, ImageFormat.Bmp);
                }
            }
        }

        /// <summary>
        /// Бутон от падащото меню, който копира форми.
        /// </summary>
        private void CopyToolStripMenuItemClick(object sender, EventArgs e)
        {
            dialogProcessor.Copy();
        }

        private void EditToolStripMenuItemClick(object sender, EventArgs e)
        {

        }
    }
}