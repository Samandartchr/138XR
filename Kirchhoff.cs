using System;

public class Kirchhoff
{
    /*
    public static void Main(string[] args)
    {
        foreach (var v in result){
            Console.WriteLine(v);
        }
    }
    */
    
    public static float[,] A = new float[,]
    {
        {1,1,0,1,0,0},
        {-1,0,1,0,0,1},
        {0,-1,-1,0,1,0},
        {0,0,0,-1,-1,-1},
        {0,8,-6,0,0,0},
        {0,8,0,0,1,-3},
        {0,0,0,10,0,-3},
        {0,0,-6,10,-1,0},
    };
    public static float[] C = new float[] {0,0,0,0,20,20,20,20};
    public static float[] result = SolveLeastSquares(A, C);

    public static float[] SolveLeastSquares(float[,] A, float[] C)
    {
        int m = A.GetLength(0); // Rows
        int n = A.GetLength(1); // Columns
        float[,] ATA = new float[n, n];
        for (int i = 0; i < n; i++)
            for (int j = 0; j < n; j++)
                for (int k = 0; k < m; k++)
                    ATA[i, j] += A[k, i] * A[k, j];
        float[] ATC = new float[n];
        for (int i = 0; i < n; i++)
            for (int k = 0; k < m; k++)
                ATC[i] += A[k, i] * C[k];
        return SolveGaussian(ATA, ATC);
    }
    public static float[] SolveGaussian(float[,] A, float[] b)
    {
        int n = A.GetLength(0);
        float[,] M = (float[,])A.Clone();
        float[] x = (float[])b.Clone();
        for (int i = 0; i < n; i++)
        {
            if (Math.Abs(M[i, i]) < 1e-6f)
                throw new Exception("Zero pivot");
            for (int j = i + 1; j < n; j++)
            {
                float factor = M[j, i] / M[i, i];
                for (int k = i; k < n; k++)
                    M[j, k] -= factor * M[i, k];
                x[j] -= factor * x[i];
            }
        }
        float[] result = new float[n];
        for (int i = n - 1; i >= 0; i--)
        {
            float sum = x[i];
            for (int j = i + 1; j < n; j++)
                sum -= M[i, j] * result[j];
            result[i] = sum / M[i, i];
        }
        return result;
    }
}