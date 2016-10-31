import mpi.MPI;
import java.io.File;
import java.io.IOException;
import java.io.PrintWriter;
import java.io.*;
import java.lang.Integer;
import java.util.Arrays;
import java.util.Scanner;

public class Main {
    public final static int root = 0;
    public final static int INF = Integer.MAX_VALUE / 2;
    public static void main(String[] args) throws IOException{
        long startTime = System.currentTimeMillis();
        MPI.Init(args);
        //File inputFile = null,
		File outputFile = null;
		//FileInputStream fileInputStream = null;        
		int me = MPI.COMM_WORLD.Rank();
        int size = MPI.COMM_WORLD.Size();
        //Scanner sc = null;
		BufferedReader bi = null;
        PrintWriter pw = null;
        int n = 0;
        if (me == root) {
            //inputFile = new File(args[3]);
			//fileInputStream = new FileInputStream(args[3]);
            outputFile = new File(args[4]);
            //sc = new Scanner(inputFile); 						//долгое считывает
			bi = new BufferedReader(new FileReader(args[3])); 	//так быстрее
            pw = new PrintWriter(outputFile);
            //n = sc.nextInt();
			n = Integer.parseInt(bi.readLine());            
			int[] buf = {n};
            for (int i = 1; i < MPI.COMM_WORLD.Size(); i++) {
                MPI.COMM_WORLD.Send(buf, 0, 1, MPI.INT, i, 99);
            }
        } else {
            int[] buf = new int[1];
            MPI.COMM_WORLD.Recv(buf, 0, 1, MPI.INT, root, 99);
            n = buf[0];
        }
        int[][] graph = new int[n][n];
        boolean[] used = new boolean[n];
        int[] dist = new int[n];
        Arrays.fill(dist, INF);
        dist[0] = 0;
        if (me == root) {
            /*while (sc.hasNext()) {		
                int a = sc.nextInt();
                int b = sc.nextInt();
                int c = sc.nextInt();
                graph[a][b] = graph[b][a] = c;
            }*/
			String line;
			while ((line = bi.readLine()) != null) {
				int[] mas = new int[3];
				int i = 0;
    			for (String numStr: line.split("\\s")) {
        			mas[i++] = Integer.parseInt(numStr);
				}
				int a = mas[0];
				int b = mas[1];
				int 	c = mas[2];
				graph[a][b] = graph[b][a] = c;
			}	

			for (int i = 0; i < n; i++) {
                int[] buf = new int[n];
                for (int j = 0; j < n; j++) {
                    buf[j] = graph[i][j];
                }
                for (int j = 1; j < size; j++) {	
                    MPI.COMM_WORLD.Send(buf, 0, buf.length, MPI.INT, j, 10);
			    }
            }
			//System.out.println("adadada");
        } else {
			for (int i = 0 ; i < n; i++) {
                int[] buf = new int[n];
                MPI.COMM_WORLD.Recv(buf, 0, n, MPI.INT, root, 10);
                for (int k = 0; k < n; k++) {
                	graph[i][k] = buf[k];
            	}
            }
        }
	//System.out.println(me);
        // закончили пересылку входных  данных
		if (me == root) {
            System.out.println((System.currentTimeMillis() - startTime) / 1000.0);
        }
        for (int i = 0; i < n; i++) {
			//System.out.println(n + " " + size + " " + me);
            int v = -1;
            for (int nv = me; nv < n; nv += size) {
                if (!used[nv] && dist[nv] < INF && (v == -1 || dist[v] > dist[nv])) {
                    v = nv;
                }
            }
            if (me == root) {
                for (int j = 1; j < MPI.COMM_WORLD.Size(); j++) {
                    int[] buf = new int[1];
                    MPI.COMM_WORLD.Recv(buf, 0 , 1, MPI.INT, j, 11);
                    int vv = buf[0];
                    if (v == -1) {
                        v = vv;
                    } else if (vv != -1 && dist[v] > dist[vv]) {
                        v = vv;
                    }
                }
                int[] buf = new int[]{v};
                for (int j = 1; j < MPI.COMM_WORLD.Size(); j++) {
                    MPI.COMM_WORLD.Send(buf, 0, buf.length, MPI.INT, j, 12);
                }
            } else {
                int[] buf  = new int[]{v};
                MPI.COMM_WORLD.Send(buf, 0, buf.length, MPI.INT, root, 11);
                MPI.COMM_WORLD.Recv(buf, 0, 1, MPI.INT, root, 12);
                v = buf[0];
            }
            if (v != -1) {
                used[v] = true;
                for (int nv = 0; nv < n; nv++) {
                    if (!used[nv] && graph[v][nv] != 0) {
                        if (dist[nv] > graph[v][nv]) {
                            dist[nv] = graph[v][nv];
                        }
                    }
                }
            }
        }
        if (me == root) {
            int sum = 0;
            //System.out.println(Arrays.toString(dist));
            for (int i = 0; i < n; i++) {
                sum += dist[i];
            }
            pw.println(n);
            pw.println(sum);
            pw.close();
            System.out.println((System.currentTimeMillis() - startTime) / 1000.0);
        }
	//System.out.println(me);
	MPI.Finalize();
    }
}

