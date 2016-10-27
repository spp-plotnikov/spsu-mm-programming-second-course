import mpi.MPI;
import java.io.File;
import java.io.IOException;
import java.io.PrintWriter;
import java.lang.Integer;
import java.util.Vector;
import java.util.Scanner;

public class Main {
    public final static int root = 0;
    public static void main(String[] args) throws IOException{
//        long startTime = System.currentTimeMillis();
        MPI.Init(args);
        File inputFile = null, outputFile = null;
        int me = MPI.COMM_WORLD.Rank();
        int size = MPI.COMM_WORLD.Size();
        Scanner sc = null;
        PrintWriter pw = null;
        int n,m = 0;
        if (me == root) {
            inputFile = new File(args[3]);
            outputFile = new File(args[4]);
            sc = new Scanner(inputFile);
            pw = new PrintWriter(outputFile);
            n = sc.nextInt();
            int[] buf = {n};
            for (int i = 1; i < MPI.COMM_WORLD.Size(); i++) {
                MPI.COMM_WORLD.Send(buf, 0, 1, MPI.INT, i, 99);
            }
        } else {
            int[] buf = new int[1];
            MPI.COMM_WORLD.Recv(buf, 0, 1, MPI.INT, root, 99);
            n = buf[0];
        }
        Vector<Integer> g[] = new Vector[n + 1];
        Vector<Integer> weight[] = new Vector[n + 1];
        boolean[] used = new boolean[n + 1];
        if (me == root) {
            for (int i = 1; i <= n; i++) {
                g[i] = new Vector<Integer>();
                weight[i] = new Vector<Integer>();
            }
            while (sc.hasNext()) {
                int a = sc.nextInt();
                int b = sc.nextInt();
                int c = sc.nextInt();
                g[a].add(b);
                g[b].add(a);
                weight[a].add(c);
                weight[b].add(c);
            }
            for (int i = 1; i <= n; i++) {
                for (int j = 1; j < size; j++) {
                    int[] buf1 = new int[]{g[i].size()};
                    MPI.COMM_WORLD.Send(buf1, 0, 1, MPI.INT, j, 4);
                    int[] buf2 = new int[g[i].size()];
                    for (int k = 0; k < g[i].size(); k++) {
                        buf2[k] = g[i].get(k);
                    }
                    MPI.COMM_WORLD.Send(buf2, 0, buf2.length, MPI.INT, j, 5);
                    for (int k = 0; k < g[i].size(); k++) {
                        buf2[k] = weight[i].elementAt(k);
                    }
                    MPI.COMM_WORLD.Send(buf2, 0, buf2.length, MPI.INT, j, 6);
                }
            }

        } else {
            for (int i = 1; i <= n; i++) {
                int[] buf1 = new int[1];
                MPI.COMM_WORLD.Recv(buf1, 0, 1, MPI.INT, root, 4);
                int len = buf1[0];
                int[] buf2 = new int[len];
                MPI.COMM_WORLD.Recv(buf2, 0, len, MPI.INT, root, 5);
                g[i] = new Vector<Integer> ();
                for (int j = 0; j < len; j++) {
                    g[i].add(buf2[j]);
                }
                weight[i] = new Vector<Integer>();
                MPI.COMM_WORLD.Recv(buf2, 0, len, MPI.INT, root, 6);
                for (int j = 0; j < len; j++) {
                    weight[i].add(buf2[j]);
                }
            }
        }
        used[1] = true;
        long sum = 0;
        for (int k = 0; k < n - 1; k++) {
            int min_weight = Integer.MAX_VALUE;
            int to = 0;
            for (int i = me; i < g.length; i += size) {
                if (used[i]) {
                    for (int j = 0; j < g[i].size(); j++) {
                        if (weight[i].elementAt(j) < min_weight && !used[g[i].elementAt(j)]) {
                            min_weight = weight[i].get(j);
                            to = g[i].get(j);
                        }
                    }
                }
            }
            if(me == root) {
                for (int i = 1; i < size; i++) {
                    int[] buf = new int[2];
                    MPI.COMM_WORLD.Recv(buf, 0 , 2, MPI.INT, i, i);
                    if (min_weight > buf[1]) {
                        min_weight = buf[1];
                        to = buf[0];
                    }
                }
            } else {
                int[] buf = new int[]{to, min_weight};
                MPI.COMM_WORLD.Send(buf, 0, buf.length, MPI.INT, root, me);
            }
            if (me == root) {
                used[to] = true;
                sum += min_weight;
                int[] buf = new int[]{to};
                for (int i = 1; i < size; i++) {
                    MPI.COMM_WORLD.Send(buf, 0, 1, MPI.INT, i, 20);
                }
            } else {
                int[] buf = new int[1];
                MPI.COMM_WORLD.Recv(buf, 0, 1, MPI.INT, root, 20);
                to = buf[0];
                used[to] = true;
            }
        }
        if (me == root) {
            pw.println(n);
            pw.println(sum);
            pw.close();
//            System.out.println((System.currentTimeMillis() - startTime) / 1000.0);
        }
    }
}