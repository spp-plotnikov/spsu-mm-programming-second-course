/**
 * Created by Katrin on 25.07.2016.
 */

import mpi.MPI;

import java.io.*;

public class Main {

    private static final int INF = 999999;
    private static int verticesNumber;
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

        long st, en;
        st = System.currentTimeMillis();
        MPI.Init(args);

        currentProcessRank = MPI.COMM_WORLD.Rank();
        commSize = MPI.COMM_WORLD.Size();

        if (currentProcessRank == 0) {
            for (int i = 0; i < args.length; i++) {
                System.out.println(args[i]);
            }
        }

        String pathToListOfVerticles = args[args.length - 2];
        String pathToMatrixGraph = args[args.length - 1];
        if (new File(pathToListOfVerticles).exists()) {

            int[] rowsOfMatrix = initializeArray(pathToListOfVerticles);
            verticesNumber = (int) Math.sqrt(rowsOfMatrix.length);
            int size = verticesNumber;

        /*if (currentProcessRank == 0) {
            printFile(rowsOfMatrix, size, args[args.length - 1]);
        }*/

            int linesNum = size / commSize;

            int[] rowsOfCurrentProcess = new int[linesNum * size];

            distributeData(rowsOfMatrix, rowsOfCurrentProcess, size);
            doParallelFloyd(rowsOfCurrentProcess, size);
            normalizeLine(rowsOfCurrentProcess, size, pathToMatrixGraph);

            MPI.Finalize();
            en = System.currentTimeMillis();
            if (currentProcessRank == 0) {
                System.out.println("Time: " + (en - st));
            }
        } else {
            if (currentProcessRank == 0) {
                System.err.println("Wrong path to graph");
            }
        }

    }


    private static int[] initializeArray(String pathToFile) throws IOException {

        FileInputStream fis = new FileInputStream(pathToFile);
        BufferedReader br = new BufferedReader(new InputStreamReader(fis));
        String lineOfFile;
        verticesNumber = Integer.parseInt(br.readLine());

        int[] rowsOfMatrix = new int[verticesNumber * verticesNumber];
        for (int i = 0; i < rowsOfMatrix.length; i++) {
            rowsOfMatrix[i] = 0;
        }
        if (currentProcessRank == 0) {
            int k = 0;
            int number1;
            int number2;
            int number3;
            //System.out.println("size: " + verticesNumber);
            while ((lineOfFile = br.readLine()) != null) {
                String[] strs = lineOfFile.split(" ");
                number1 = Integer.parseInt(strs[0]);
                number2 = Integer.parseInt(strs[1]);
                number3 = Integer.parseInt(strs[2]);
                //проверка правильности считывания
                /*if (k != 10) {
                    System.out.println(number1 + " " + number2 + " " + number3);
                    k++;
                }*/
                rowsOfMatrix[number2 + number1 * verticesNumber] = number3;
                rowsOfMatrix[number1 + number2 * verticesNumber] = number3;
            }
            for (int i = 0; i < verticesNumber * verticesNumber; i++) {
                if (rowsOfMatrix[i] == 0) {
                    rowsOfMatrix[i] = INF;
                }
            }
            for (int i = 1; i < commSize; i++) {
                MPI.COMM_WORLD.Send(rowsOfMatrix, 0, rowsOfMatrix.length, MPI.INT, i, 0);
            }

        } else {
            MPI.COMM_WORLD.Recv(rowsOfMatrix, 0, rowsOfMatrix.length, MPI.INT, 0, 0);

        }

        br.close();
        fis.close();

        return rowsOfMatrix;
    }

    private static void printFile(int[] arrayOfRibs, int size, String path) throws IOException {
        if (currentProcessRank == 0) {
            File file = new File(path);
            PrintWriter out = new PrintWriter(new BufferedWriter(new FileWriter(file)));
            int allSize = size * size;
            int i = 0;
            int j;
            int k;
            while (i < allSize) {
                if (arrayOfRibs[i] != INF) {
                    j = (int) (i / size);
                    k = i - j * size;
                    out.println(j + " " + k + " " + arrayOfRibs[i]);
                }
                i++;
            }
            out.flush();
        }
    }

    private static void writeToFile(int[] arrayOfRibs, int size, String path) throws IOException {
        if (currentProcessRank == 0) {
            File file = new File(path);
            PrintWriter out = new PrintWriter(new BufferedWriter(new FileWriter(file)));
            int allSize = size * size;
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
    static void distributeData(int[] rowsOfMatrix, int[] rowsOfCurrentProcess, int size) {
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

    private static void doParallelFloyd(int[] rowsOfCurrentProcess, int size) {
        int rowsCount = size / commSize;
        int[] lines = new int[size];
        int option1, option2;
        for (int lineNumber = 0; lineNumber < size; lineNumber++) {
            distributeLine(rowsOfCurrentProcess, size, lineNumber, lines);
            for (int i = 0; i < rowsCount; i++)
                for (int j = 0; j < size; j++) {
                    //if ((rowsOfCurrentProcess[i * size + lineNumber] != INF) && (lines[j] != INF)) {
                    option1 = rowsOfCurrentProcess[i * size + j];
                    option2 = rowsOfCurrentProcess[i * size + lineNumber] + lines[j];
                    rowsOfCurrentProcess[i * size + j] = Math.min(option1, option2);
                    //}
                }
        }
    }

    static void distributeLine(int[] rowsOfCurrentProcess, int size, int lineNumber, int[] lines) {
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
    static void normalizeLine(int[] rows, int size, String pathToOut) throws IOException {
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

        int[] resultRowOfMatrix = new int[verticesNumber * verticesNumber];

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
            writeToFile(resultRowOfMatrix, size, pathToOut);
        }
    }
}


