/**
 * Created by Katrin on 25.07.2016.
 */

import mpi.MPI;

import java.io.*;
import java.util.Random;

public class Main {

    private static final int INF = -1;
    private static final int VERTICESNUMBER = 1000;
    private static int currentProcessRank;
    private static int commSize;

    /*
    rowsOfMatrix - все рёбра матрицы
    matrix - сама матрица смежности
    size - размерность матрицы
    vacantLines - незанятые строки
    linesNum - количество строк на текущем процессе
    rowsOfCurrentProcess - ребра текущего процесса
    */
    public static void main(String[] args) throws IOException {

        int[] rowsOfMatrix = new int[VERTICESNUMBER * VERTICESNUMBER];
        int size = VERTICESNUMBER;

        MPI.Init(args);
        currentProcessRank = MPI.COMM_WORLD.Rank();
        commSize = MPI.COMM_WORLD.Size();

        arrayInitialization(rowsOfMatrix);

        if (currentProcessRank == 0) {
            printInFile(rowsOfMatrix, size, "matrix1.txt");
        }

        int linesNum = size / commSize;

        int[] rowsOfCurrentProcess = new int[linesNum * size];

        distributionData(rowsOfMatrix, rowsOfCurrentProcess, size);
        parallelFloyd(rowsOfCurrentProcess, size);
        normalisationLines(rowsOfCurrentProcess, size);

        MPI.Finalize();

    }

    private static void arrayInitialization(int[] rowsOfMatrix) {

        int[][] matrix = new int[VERTICESNUMBER][VERTICESNUMBER];
        Random random = new Random();

        if (currentProcessRank == 0) {
            for (int i = 0; i < rowsOfMatrix.length; i++) {
                rowsOfMatrix[i] = random.nextInt(50);
            }
            for (int i = 1; i < commSize; i++) {
                MPI.COMM_WORLD.Send(rowsOfMatrix, 0, rowsOfMatrix.length, MPI.INT, i, 0);
            }
        } else {
            MPI.COMM_WORLD.Recv(rowsOfMatrix, 0, rowsOfMatrix.length, MPI.INT, 0, 0);
        }

        int index = 0;
        for (int i = 0; i < VERTICESNUMBER; i++) {
            for (int j = 0; j < VERTICESNUMBER; j++) {
                matrix[i][j] = rowsOfMatrix[index];
                if (i == j) {
                    matrix[i][j] = 0;
                }
                if (matrix[i][j] == 0) {
                    matrix[i][j] = INF;
                }
                rowsOfMatrix[index] = matrix[i][j];
                index++;
            }
        }
    }

    private static void printInFile(int[] arrayOfRibs, int size, String path) throws IOException {
        if (currentProcessRank == 0) {
            File file = new File(path);
            PrintWriter out = new PrintWriter(new BufferedWriter(new FileWriter(file)));
            int allSize = VERTICESNUMBER * VERTICESNUMBER;
            int i = 0;
            while (i < allSize) {
                for (int j = 0; j < size; j++) {
                    out.print(arrayOfRibs[i] + " ");
                    i++;
                }
                out.println();
            }
            out.flush();
        }
    }

    /*
    sendCount - количество отправляемых элементов
    sendIndex - индекс начала отправления данных
    vocationLines - свободные строки
    */
    static void distributionData(int[] rowsOfMatrix, int[] rowsOfCurrentProcess, int size) {
        int[] sendCount = new int[commSize];
        int[] sendIndex = new int[commSize];
        int vocationLines = size;

        int linesNum = (size / commSize);
        sendCount[0] = linesNum * size;
        sendIndex[0] = 0;
        for (int i = 1; i < commSize; i++) {
            vocationLines -= linesNum;
            linesNum = vocationLines / (commSize - i);
            sendCount[i] = linesNum * size;
            sendIndex[i] = sendIndex[i - 1] + sendCount[i - 1];
        }

        MPI.COMM_WORLD.Scatterv(rowsOfMatrix, 0, sendCount, sendIndex, MPI.INT, rowsOfCurrentProcess, 0,
                sendCount[currentProcessRank], MPI.INT, 0);
    }

    private static void parallelFloyd(int[] rowsOfCurrentProcess, int size) {
        int rowsCount = VERTICESNUMBER / commSize;
        int[] lines = new int[size];
        int option1, option2;
        for (int lineNumber = 0; lineNumber < size; lineNumber++) {
            linesDistribution(rowsOfCurrentProcess, size, lineNumber, lines);
            for (int i = 0; i < rowsCount; i++)
                for (int j = 0; j < size; j++)
                    if ((rowsOfCurrentProcess[i * size + lineNumber] != INF) && (lines[j] != INF)) {
                        option1 = rowsOfCurrentProcess[i * size + j];
                        option2 = rowsOfCurrentProcess[i * size + lineNumber] + lines[j];
                        rowsOfCurrentProcess[i * size + j] = Math.min(option1, option2);
                    }
        }
    }

    static void linesDistribution(int[] rowsOfCurrentProcess, int size, int lineNumber, int[] lines) {
        int processRank;
        int currentLineNumber;
        int vocationLines = size;
        int index = 0;
        int num = size / commSize;

        for (processRank = 1; processRank <= commSize; processRank++) {
            if (lineNumber < index + num) {
                break;
            }
            vocationLines -= num;
            index += num;
            num = vocationLines / (commSize - processRank);
        }
        processRank = processRank - 1;
        currentLineNumber = lineNumber - index;
        if (processRank == currentProcessRank)
            for (int i = currentLineNumber * size, j = 0; i < (currentLineNumber + 1) * size; i++, j++) {
                lines[j] = rowsOfCurrentProcess[i];
            }

        MPI.COMM_WORLD.Bcast(lines, 0, size, MPI.INT, processRank);
    }

    /*
    sendCount - количество отправляемых элементов
    sendIndex - индекс начала отправления данных
    vocationLines - свободные строки
    */
    static void normalisationLines(int[] rows, int size) throws IOException {
        int[] sendCount = new int[commSize];
        int[] sendIndex = new int[commSize];
        int vocationLines = size;

        int RowNum = (size / commSize);
        sendCount[0] = RowNum * size;
        sendIndex[0] = 0;
        for (int i = 1; i < commSize; i++) {
            vocationLines -= RowNum;
            RowNum = vocationLines / (commSize - i);
            sendCount[i] = RowNum * size;
            sendIndex[i] = sendIndex[i - 1] + sendCount[i - 1];
        }

        int[] resultRowOfMatrix = new int[VERTICESNUMBER * VERTICESNUMBER];

        if (currentProcessRank == 0) {
            System.arraycopy(rows, 0, resultRowOfMatrix, 0, rows.length);
        }
        for (int i = 1; i < commSize; i++) {
            if (currentProcessRank == i) {
                MPI.COMM_WORLD.Send(rows, 0, rows.length, MPI.INT, 0, 0);
            }
            if (currentProcessRank == 0) {
                MPI.COMM_WORLD.Recv(rows, 0, rows.length, MPI.INT, i, 0);
                System.arraycopy(rows, 0, resultRowOfMatrix, sendIndex[i], rows.length);
            }
        }

        if (currentProcessRank == 0) {
            printInFile(resultRowOfMatrix, size, "matrix2.txt");
        }
    }
}
