package com.Main;


import java.io.*;

/**
 * Created by SA on 01.08.2016.
 */
public class Solution {
    // В качестве условной бесконечности
    // выберем достаточно большое число 10^9
    private static final int INF = 1000 * 1000 * 1000;

    public static void main(String[] args) throws IOException {
        Solution solution = new Solution();

        // Вызываем процедуру решения задачи
        solution.solve();
    }

    private void solve() throws IOException {
        FileInputStream fis = new FileInputStream("D:\\Graphs\\1.txt");
        BufferedReader br = new BufferedReader(new InputStreamReader(fis));
        String lineOfFile;
        int vertexCount = Integer.parseInt(br.readLine());

        int[] rowsOfMatrix = new int[vertexCount * vertexCount];
        for (int i = 0; i < rowsOfMatrix.length; i++) {
            rowsOfMatrix[i] = 0;
        }
        int k = 0;
        int number1;
        int number2;
        int number3;
        System.out.println("size: " + vertexCount);
        while ((lineOfFile = br.readLine()) != null) {
            String[] strs = lineOfFile.split(" ");
            number1 = Integer.parseInt(strs[0]);
            number2 = Integer.parseInt(strs[1]);
            number3 = Integer.parseInt(strs[2]);
            if (k != 10) {
                System.out.println(number1 + " " + number2 + " " + number3);
                k++;
            }
            rowsOfMatrix[number2 + number1 * vertexCount] = number3;
            rowsOfMatrix[number1 + number2 * vertexCount] = number3;
        }
        for (int i = 0; i < vertexCount * vertexCount; i++) {
            if (rowsOfMatrix[i] == 0) {
                rowsOfMatrix[i] = INF;
            }
        }

        br.close();
        fis.close();

        int[][] g = new int[vertexCount][vertexCount];
        int index = 0;
        for (int i = 0; i < vertexCount; i++) {
            for (int j = 0; j < vertexCount; j++) {
                g[i][j] = rowsOfMatrix[index];
                if (i == j) {
                    g[i][j] = 0;
                }
                if (g[i][j] == 0) {
                    g[i][j] = INF;
                }
                rowsOfMatrix[index] = g[i][j];
                index++;
            }
        }


        // Согласно алгоритму будем обновлять
        // ответ для каждой пары вершин i и j,
        // перебирая промежуточную вершину k
        for (k = 0; k < vertexCount; k++) {
            for (int i = 0; i < vertexCount; i++) {
                for (int j = 0; j < vertexCount; j++) {
                    g[i][j] = Math.min(g[i][j], g[i][k] + g[k][j]);
                }
            }
        }

        // Для каждой пары вершин выведем величину
        // кратчайшего пути от i до j, или 0,
        // если j не достижима из i

        File file = new File("12.txt");
        PrintWriter out = new PrintWriter(new BufferedWriter(new FileWriter(file)));
        for (int i = 0; i < vertexCount; i++) {
            for (int j = 0; j < vertexCount; j++) {
                if (g[i][j] == INF) {
                    out.print(0 + " ");
                } else {
                    out.print(g[i][j] + " ");
                }
            }
            out.println();
        }
        out.flush();

    }
}
