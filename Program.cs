using System;
using System.Linq;

namespace Arrays
{
    class Program
    {
        public static void Main(string[] args)
        {
            
            bool flag = true;
            int n = -1;
            while (flag)
            {
                try
                {
                    Console.WriteLine("Input matrix dimension: ");
                    n = int.Parse(Console.ReadLine());
                    flag = false;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Program need to take integer number 1 - 1000");
                }
            }

            
            if (n == 1)
            {
                Console.WriteLine(1);
            }
            else if (n == 2) 
            {
                Console.WriteLine("Sorry, there is no magic square for 2");
            }
            else if (n > 1000 || n <= 0)
            {
                Console.WriteLine("incorrect number");
            }
            else
            {
                int[][] matrix;
                if (n % 2 == 1)
                {
                    matrix = create_odd_matrix(n);
                }
                else if (n % 2 == 0 && n % 4 != 0)
                {
                    matrix = create_even_not_4_matrix(n);
                }
                else
                {
                    matrix = create_even_4_matrix(n);
                }
                show_matrix(matrix);
                if (check_matrix_on_magic(matrix))
                    Console.WriteLine("It's magic square");
                else
                {
                    Console.WriteLine("It's not magic square");
                }
            }
            
        }

        public static int[][] create_odd_matrix(int n, int count = 1)
        {
            int[][] matrix = create_square_matrix(n);
            int y = 0; int x = n / 2;
            int t_x = x; int t_y = y;
            while (count <= Math.Pow(n, 2))
            {
                matrix[y][x] = count++;

                if (((y == 0) && (x >= n - 1)) && (matrix[n - 1][0] != 0))
                {
                    y++;
                }
                else
                {
                    y--;
                    if (y < 0)
                    {
                        y = n - 1;
                    }
                    x++;
                    if (x == n)
                    {
                        x = 0;
                    }
                    if (matrix[y][x] != 0)
                    {
                        y += 2;
                        x--;
                    }
                }
            }

            return matrix;
        }

        public static int[][] create_even_not_4_matrix(int n)
        {
            int[][] result = create_square_matrix(n);

            int local_mat_n = n / 2;
            int otstup = local_mat_n - (local_mat_n / 2) - 2;
            int[][] m00 = create_odd_matrix(local_mat_n);
            int[][] m10 = create_square_matrix(local_mat_n);
            int[][] m01 = create_square_matrix(local_mat_n);
            int[][] m11 = create_square_matrix(local_mat_n);
            for (int i = 0; i < local_mat_n; i++)
            {
                for (int j = 0; j < local_mat_n; j++)
                {
                    m10[i][j] = m00[i][j] + (local_mat_n * local_mat_n)*3;
                    m01[i][j] = m00[i][j] + (local_mat_n * local_mat_n)*2;
                    m11[i][j] = m00[i][j] + (local_mat_n * local_mat_n);
                }
            }

            int t_val = m00[0][0];
            m00[0][0] = m10[0][0];
            m10[0][0] = t_val;

            t_val = m00[local_mat_n-1][0];
            m00[local_mat_n-1][0] = m10[local_mat_n-1][0];
            m10[local_mat_n-1][0] = t_val;

            for (int i = 1; i < local_mat_n-1; i++)
            {
                t_val = m00[i][1];
                m00[i][1] = m10[i][1];
                m10[i][1] = t_val;
            }

            for (int i = 0; i < otstup; i++)
            {
                for (int j = 0; j < local_mat_n; j++)
                {
                    t_val = m00[j][local_mat_n - 1 - i];
                    m00[j][local_mat_n - 1 - i] = m10[j][local_mat_n - 1 - i];
                    m10[j][local_mat_n - 1 - i] = t_val;
                }
                
                for (int j = 0; j < local_mat_n; j++)
                {
                    t_val = m01[j][i];
                    m01[j][i] = m11[j][i];
                    m11[j][i] = t_val;
                }
            }

            for (int i = 0; i < local_mat_n; i++)
            {
                for (int j = 0; j < local_mat_n; j++)
                {
                    result[i][j] = m00[i][j];
                    result[i][j + local_mat_n] = m01[i][j];
                    result[i + local_mat_n][j] = m10[i][j];
                    result[i + local_mat_n][j + local_mat_n] = m11[i][j];
                }
            }
            return result;
        }

        public static int[][] create_even_4_matrix(int n)
        {
            int[][] result = create_square_matrix(n);
            int[][] reverse_mat = create_square_matrix(n);
            int s_size = n / 4; // size of small squares
            int center_size = n / 2;

            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    result[i][j] = j + i * n + 1;
                }
                for (int k = 0; k < n; k++)
                {
                    reverse_mat[i][k] = result[i][n-k-1];
                }
            }
            Array.Reverse(reverse_mat);

            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    if ((i >= s_size && i < n - s_size && (j < s_size || j > n - s_size - 1)) || ((i < s_size || i > n - s_size -1) && j >= s_size && j < n - s_size))
                    {
                        result[i][j] = reverse_mat[i][j];
                    }
                }
            }

            return result;
        }

        public static bool check_matrix_on_magic(int[][] matrix)
        {
            int sm = matrix[0].Sum();
            for (int i = 0; i < matrix.Length; i++)
            {
                if (matrix[i].Sum() != sm)
                {
                    Console.WriteLine("Not magic by string");
                    return false;
                }
            }

            int diag = 0;
            int side_diag = 0;

            for (int i = 0; i < matrix.Length; i++)
            {
                int column = 0;
                for (int j = 0; j < matrix[i].Length; j++)
                {
                    if (i == j)
                        diag += matrix[i][j];

                    column += matrix[j][i];
                }

                side_diag += matrix[i][matrix.Length - i - 1];
                if (column != sm)
                {
                    Console.WriteLine("Not magic by columns");
                    return false;
                }

            }
            if (sm != diag || sm != side_diag)
            {
                Console.WriteLine("Not magic by diag");
                return false;
            }
            return true;
        }
        public static void show_matrix(int[][] matrix)
        {
            int ln  = Convert.ToString(matrix.Length * matrix.Length).Length;
            Console.WriteLine();
            for (int i = 0; i < matrix.Length; i++)
            {
                Console.Write("  ");
                for (int j = 0; j < matrix[i].Length; j++)
                {
                    string gap = " ";
                    for (int k = 0; k < ln - Convert.ToString(matrix[i][j]).Length; k++)
                        gap += " ";
                    Console.Write(matrix[i][j] + gap);
                }
                Console.WriteLine();
            }
            Console.WriteLine();

        }

        public static int[][] create_square_matrix(int n)
        {
            int[][] matrix = new int[n][];
            for (int i = 0; i < n; i++)
            {
                matrix[i] = new int[n];
            }
            return matrix;
        }
    }
}