package com.Main;

import java.io.*;

/**
 * Created by SA on 01.08.2016.
 */
public class Main {
    public static void main(String[] args) throws IOException {
        FileInputStream fis = new FileInputStream("D:\\Graphs\\graph1.txt");
        BufferedReader br = new BufferedReader(new InputStreamReader(fis));
        String lineOfFile;
        int verticesNumber = Integer.parseInt(br.readLine());
        int[] rowsOfMatrix = new int[verticesNumber*verticesNumber];
        int number1;
        int number2;
        int number3;
        while ((lineOfFile = br.readLine()) != null) {
            String[] strs = lineOfFile.split(" ");
            number1 = Integer.parseInt(strs[0]);
            number2 = Integer.parseInt(strs[1]);
            number3 = Integer.parseInt(strs[2]);
            rowsOfMatrix[number1 + number2 * verticesNumber] = number3;
        }
        for (int i = 0; i < verticesNumber * verticesNumber; i++) {
            if (rowsOfMatrix[i] == 0) {
                rowsOfMatrix[i] = 9999999;
            }
        }
        br.close();
        fis.close();


        File file = new File("matrix.txt");
        PrintWriter out = new PrintWriter(new BufferedWriter(new FileWriter(file)));
        int allSize = verticesNumber * verticesNumber;
        int i = 0;
        while (i < allSize) {
            for (int j = 0; j < verticesNumber; j++) {
                out.print(rowsOfMatrix[i] + " ");
                i++;
            }
            out.println();
        }
        out.flush();
    }


}
