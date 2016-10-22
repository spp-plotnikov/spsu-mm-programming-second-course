import mpi.*;

import java.util.Scanner;
import java.io.File;
import java.io.BufferedWriter;
import java.io.FileWriter;
import java.util.Arrays;

public class Main {
    static int root = 0;

    static void doPairwiseStuff(int n, int[] local_arr, int sender, int receiver) {
        int[] remote = new int[n + 1];
        int[] all = new int[2 * n + 1];
        int mergeTag = 1;
        int sortedTag = 2;

        int me = MPI.COMM_WORLD.Rank();
        if (me == sender) {
            // TODO: is there any function that does both?
            MPI.COMM_WORLD.Send(local_arr, 0, n, MPI.INT, receiver, mergeTag);
            MPI.COMM_WORLD.Recv(local_arr, 0, n, MPI.INT, receiver, sortedTag);
        } else {
            MPI.COMM_WORLD.Recv(remote, 0, n, MPI.INT, sender, mergeTag);

            // Merge remote and local
            int i, j;
            int cnt = 0;
            for (i = 0, j = 0; i < n; i++) {
                while ((remote[j] < local_arr[i]) && j < n) {
                    all[cnt++] = remote[j++];
                }
                all[cnt++] = local_arr[i];
            }
            while (j < n)
                all[cnt++] = remote[j++];

            int theirStart = 0, myStart = n;
            if (sender > me) {
                theirStart = n;
                myStart = 0;
            }
            MPI.COMM_WORLD.Send(all, theirStart, n, MPI.INT, sender, sortedTag);
            for (int k = myStart; k < myStart + n; k++)
                local_arr[k - myStart] = all[k];
        }
    }

    public static void main(String args[]) throws Exception {
        MPI.Init(args);
        int me = MPI.COMM_WORLD.Rank();
        String input_name = null, output_name = null;
        if (me == root) {
            // for some reason it uses first 3 arguments for its internal purposes
            input_name = args[3];
            output_name = args[4];
        }
        int nodes = MPI.COMM_WORLD.Size();
        int n = 0;
        double start_time = 0;
        if (me == root) {
            System.out.println("Running on " + MPI.COMM_WORLD.Size() + " thread(s)");
            start_time = System.currentTimeMillis();
        }

        // Read & sync n
        Scanner sc = null;
        if (me == root) {
            sc = new Scanner(new File(input_name));
            int tmp;
            while (sc.hasNextInt()) {
                tmp = sc.nextInt();
                n++;
            }
            sc.close();
            sc = new Scanner(new File(input_name));
        }
        int[] buf = {n};
        MPI.COMM_WORLD.Bcast(buf, 0, 1, MPI.INT, root);
        n = buf[0];

        // Allocate the array
        int per_node = n / nodes + 1, fakes = per_node * nodes - n;
        int[] arr = new int[per_node * nodes];
        if (me == root) {
            for (int i = 0; i < n; i++)
                arr[i] = sc.nextInt();
            for (int i = 0; i < fakes; i++)
                arr[n + i] = Integer.MAX_VALUE;
            sc.close();
        }

        // Scatter the data to slaves
        int[] local_arr = new int[per_node];
        MPI.COMM_WORLD.Scatter(arr, 0, per_node, MPI.INT, local_arr, 0, per_node, MPI.INT, root);

        // In each slave first sort the local
        Arrays.sort(local_arr);

        // ... and here it comes
        for (int i = 0; i < nodes; i++) {
            if ((i + me + 1) % 2 == 0) {
                if (me < nodes - 1) {
                    doPairwiseStuff(per_node, local_arr, me, me + 1);
                }
            } else if (me != root) {
                doPairwiseStuff(per_node, local_arr, me - 1, me);
            }
        }

        MPI.COMM_WORLD.Gather(local_arr, 0, per_node, MPI.INT, arr, 0, per_node, MPI.INT, root);
        if (me == root) {
            BufferedWriter outputWriter = new BufferedWriter(new FileWriter(output_name));
            for (int i = 0; i < n; i++)
                outputWriter.write(arr[i] + " ");
            outputWriter.flush();
            outputWriter.close();
            System.out.println("Elapsed: " + (System.currentTimeMillis() - start_time) / 1000.0 + " seconds");
        }
        MPI.Finalize();
    }
}
