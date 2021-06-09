using System.Drawing;
using System.Windows.Forms;

namespace ListViewCustomReorder
{
    /// <summary>
    /// A slightly extended version of the standard ListView.
    /// It has two additional properties to draw an insertion line before or after an item.
    /// </summary>
    public class ListViewEx : ListView
    {
        // from WinUser.h
        private const int WM_PAINT = 0xF;

        public ListViewEx()
        {
            // Reduce flicker
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
        }

        /// <summary>
        /// If set to a value >= 0, an insertion line is painted before the item with the given index.
        /// </summary>
        public int LineBefore { get; set; } = -1;

        /// <summary>
        /// If set to a value >= 0, an insertion line is painted after the item with the given index.
        /// </summary>
        public int LineAfter { get; set; } = -1;

        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);

            // We have to take this way (instead of overriding OnPaint()) because the ListView is just a wrapper
            // around the common control ListView and unfortunately does not call the OnPaint overrides.
            if (m.Msg == WM_PAINT)
            {
                if (LineBefore >= 0 && LineBefore < Items.Count)
                {
                    Rectangle rc = Items[LineBefore].GetBounds(ItemBoundsPortion.Entire);
                    DrawInsertionLine(rc.Left, rc.Right, rc.Top);
                }
                if (LineAfter >= 0 && LineBefore < Items.Count)
                {
                    Rectangle rc = Items[LineAfter].GetBounds(ItemBoundsPortion.Entire);
                    DrawInsertionLine(rc.Left, rc.Right, rc.Bottom);
                }
            }
        }

        /// <summary>
        /// Draw a line with insertion marks at each end
        /// </summary>
        /// <param name="X1">Starting position (X) of the line</param>
        /// <param name="X2">Ending position (X) of the line</param>
        /// <param name="Y">Position (Y) of the line</param>
        private void DrawInsertionLine(int X1, int X2, int Y)
        {
            using (Graphics g = CreateGraphics())
            {
                g.DrawLine(Pens.DodgerBlue, X1, Y, X2 - 1, Y);
                Point[] leftTriangle = new[]
                {

                    new Point(X1,     Y - 8),
                    new Point(X1 + 1, Y - 7),
                    new Point(X1 + 2, Y - 6),
                    new Point(X1 + 3, Y - 5),
                    new Point(X1 + 4, Y - 4),
                    new Point(X1 + 5, Y - 3),
                    new Point(X1 + 6, Y - 2),
                    new Point(X1 + 7, Y - 1),
                    new Point(X1 + 8, Y),
                    new Point(X1 + 7, Y + 1),
                    new Point(X1 + 6, Y + 2),
                    new Point(X1 + 5, Y + 3),
                    new Point(X1 + 4, Y + 4),
                    new Point(X1 + 3, Y + 5),
                    new Point(X1 + 2, Y + 6),
                    new Point(X1 + 1, Y + 7),
                    new Point(X1,     Y + 8)
                };
                Point[] rightTriangle = new[]
                {
                    new Point(X2,     Y - 8),
                    new Point(X2 - 1, Y - 7),
                    new Point(X2 - 2, Y - 6),
                    new Point(X2 - 3, Y - 5),
                    new Point(X2 - 4, Y - 4),
                    new Point(X2 - 5, Y - 3),
                    new Point(X2 - 6, Y - 2),
                    new Point(X2 - 7, Y - 1),
                    new Point(X2 - 9, Y),
                    new Point(X2 - 7, Y + 1),
                    new Point(X2 - 6, Y + 2),
                    new Point(X2 - 5, Y + 3),
                    new Point(X2 - 4, Y + 4),
                    new Point(X2 - 3, Y + 5),
                    new Point(X2 - 2, Y + 6),
                    new Point(X2 - 1, Y + 7),
                    new Point(X2,     Y + 8)
                };
                g.FillPolygon(Brushes.DodgerBlue, leftTriangle);
                g.FillPolygon(Brushes.DodgerBlue, rightTriangle);
            }
        }
    }
}
