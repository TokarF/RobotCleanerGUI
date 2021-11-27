using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotCleanerGUI
{
    public class RobotCleaner
    {
        private Random rnd = new Random();
        private int ROW { get; set; }
        private int COL { get; set; }
        private bool[,] vis { get; set; }
        private DataGridView dgvBoard { get; set; }

        static int[] dRow = { 0, 1, 0, -1 };
        static int[] dCol = { -1, 0, 1, 0 };

        public RobotCleaner(Form frm, int row, int col)
        {
            ROW = row;
            COL = col;
            vis = new bool[ROW, COL];

            for (int i = 0; i < ROW; i++)
            {
                for (int j = 0; j < COL; j++)
                {
                    vis[i, j] = false;
                }
            }

            dgvBoard = new DataGridView();


            dgvBoard.RowCount = row;
            dgvBoard.ColumnCount = col;
            dgvBoard.RowHeadersVisible = false;
            dgvBoard.ColumnHeadersVisible = false;
            dgvBoard.ScrollBars = ScrollBars.None;
            dgvBoard.AllowUserToResizeRows = false;
            dgvBoard.AllowUserToResizeColumns = false;


            for (int i = 0; i < row; i++)
            {
                dgvBoard.Rows[i].Height = 50;
                for (int j = 0; j < col; j++)
                {
                    dgvBoard.Columns[j].Width = 50;
                    dgvBoard.Rows[i].Cells[j].Value = $"";
                    dgvBoard.Rows[i].Cells[j].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    dgvBoard.Rows[i].Cells[j].Style.Font = new Font("Tahoma", 20);
                }
            }

            dgvBoard.ClientSize = new Size(dgvBoard.ColumnCount * 50, dgvBoard.RowCount * 50);

            frm.Size = new Size(dgvBoard.Width, dgvBoard.Height);
            frm.Controls.Add(dgvBoard);
            dgvBoard.ClearSelection();
            DFS(rnd.Next(0, ROW), rnd.Next(0, COL));

        }

        private void SetBoard(Tuple<int, int> cleanerPosition)
        {
            Bitmap bmp = (Bitmap)Image.FromFile("dust.png");


            for (int i = 0; i < ROW; i++)
            {
                for (int j = 0; j < COL; j++)
                {
                    if (i == cleanerPosition.Item1 && j == cleanerPosition.Item2)
                    {
                        dgvBoard.Rows[i].Cells[j].Value = $"⭕";

                    }
                    else if (!vis[i, j])
                    {
                        dgvBoard.Rows[i].Cells[j].Style.BackColor = Color.SandyBrown;
                    }
                    else
                    {
                        dgvBoard.Rows[i].Cells[j].Value = $"";
                        dgvBoard.Rows[i].Cells[j].Style.BackColor = Color.Green;
                    }
                }
            }
        }

        private bool isValid(int row, int col)
        {

            // If cell is out of bounds
            if (row < 0 || col < 0 ||
                row >= ROW || col >= COL)
                return false;

            // If the cell is already visited
            if (vis[row, col])
                return false;

            // Otherwise, it can be visited
            return true;
        }

        public async void DFS(int row, int col)
        {

            // Initialize a stack of pairs and
            // push the starting cell into it
            Stack st = new Stack();
            st.Push(new Tuple<int, int>(row, col));
            Tuple<int, int> curr = new Tuple<int, int>(row, col);
            // Iterate until the
            // stack is not empty
            while (st.Count > 0)
            {

                // Pop the top pair
                curr = (Tuple<int, int>)st.Peek();
                st.Pop();

                row = curr.Item1;
                col = curr.Item2;

                // Check if the current popped
                // cell is a valid cell or not
                if (!isValid(row, col))
                    continue;

                // Mark the current
                // cell as visited
                vis[row, col] = true;

                // Print the element at
                // the current top cell
                SetBoard(Tuple.Create(curr.Item1, curr.Item2));
                await Task.Delay(1000);
                //Thread.Sleep(500);
                //Console.Write(board[row, col] + " ");

                // Push all the adjacent cells
                for (int i = 0; i < 4; i++)
                {
                    int adjx = row + dRow[i];
                    int adjy = col + dCol[i];
                    st.Push(new Tuple<int, int>(adjx, adjy));
                }
            }
            SetBoard(Tuple.Create(curr.Item1, curr.Item2));
            MessageBox.Show("The room has been cleaned!", "Done", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }


    }
}
