import mpi.*;

import java.io.File;
import java.io.FileNotFoundException;
import java.util.*;


public class QsortByRegularSampling {
    private static int processesCount;
    private static int rootRank = 0;
    private static int rank;

    private static Boolean isRoot(int rank) {
        return rank == rootRank;
    }

    private static int[] splitOnAlmostEqualSize(int length, int count) {
        int rest = length % count;
        int[] counts = new int[count];
        for (int i = 0; i < count; i++) {
            counts[i] = length/count;
            if (rest != 0) {
                counts[i]++;
                rest--;
            }
        }

        return counts;
    }

    /*
     * _mergeSortedArrays[1 and 2] makes 2D array from 1D array using lengths of subarrays
     *
    */

    private static int[] _mergeSortedArrays1(int[] array, int count) { //count = processesCount = subarrayLength
        int[][] _array = new int[count][count];
        for (int i = 0; i < count; i++) {
            _array[i] = Arrays.copyOfRange(array, i * count, (i + 1) * count);
        }

        return mergeSortedArrays(_array);
    }

    private static int[] _mergeSortedArrays2(int[] array, int[] lengths) {
        int[][] _array = new int[lengths.length][];
        int start = 0;
        for (int i = 0; i < lengths.length; i++) {
            _array[i] = new int[lengths[i]];
            _array[i] = Arrays.copyOfRange(array, start, start + lengths[i]);
            start += lengths[i];
        }

        return mergeSortedArrays(_array);
    }

    private static int[] mergeSortedArrays(int[][] array) {
        PriorityQueue<ArrayContainer> queue = new PriorityQueue<>();
        int total = 0;

        //add arrays to heap
        for (int i = 0; i < array.length; i++) {
            if (array[i].length != 0) {
                queue.add(new ArrayContainer(array[i], 0));
                total = total + array[i].length;
            }
        }

        int m = 0;
        int result[] = new int[total];

        while (!queue.isEmpty()) {
            ArrayContainer ac = queue.poll();
            result[m++] = ac.array[ac.index];

            if (ac.index < ac.array.length - 1) {
                queue.add(new ArrayContainer(ac.array, ac.index + 1));
            }
        }

        return result;
    }

    //returns the smallest index of which the number is bigger than or equal to the key
    private static int binarySearch(int key, int[] array, int start, int end) {
        int length = array.length;

        if (end > length) {
            end = length;
        }

        if (start < 0) {
            start = 0;
        }

        if (key > array[end - 1]) {
            return end;
        }
        if (key < array[start]) {
            return start;
        }

        int pos = start + (end - start)/2;
        while (start < end) {
            if (key > array[pos]) {
                start = pos + 1;
            } else {
                if (key < array[pos]) {
                    end = pos;
                } else {
                    break;
                }
            }
            pos = start + (end - start)/2;
        }

        return pos;
    }

    private static int[] sort(int[] array) {
        /*
         * Sorry for variables' names; Even I will forget what they mean after a short period of time;
         * That link will help you in trying to find out that these names really mean and what is going on here:
         * http://www.intuit.ru/studies/courses/1156/190/lecture/4958?page=7
         *
         * In that implementation I've tried to avoid Point-to-Point communication operations such as Send and Receive
         * and used Collective Communication operations such as Scatter[v], Gather[v], Broadcast, Alltoall[v]
         *
        */

        /*
         * 1 Phase
         * Broadcasting array.length (because array only stored on process with rank rootRank)
         * Scattering blocks' length to each process,
         * Sending parts of array to processes, sorting on them
        */

        int[] _arrayLength = new int[]{array.length};
        MPI.COMM_WORLD.Bcast(
                _arrayLength,
                0,
                1,
                MPI.INT,
                rootRank
        );
        int arrayLength = _arrayLength[0];

        int[] processBlocksLengths = new int[processesCount];
        int[] _processBlockLength = new int[1];
        if (isRoot(rank)) {
            processBlocksLengths = splitOnAlmostEqualSize(array.length, processesCount); //returns counts
        }

        MPI.COMM_WORLD.Scatter(
                processBlocksLengths,
                0,
                1,
                MPI.INT,
                _processBlockLength,
                0,
                1,
                MPI.INT,
                rootRank
        );
        int processBlockLength = _processBlockLength[0];

        int[] displacements = new int[processesCount];
        int[] processBlock = new int[processBlockLength];
        if (isRoot(rank)) {
            displacements[0] = 0; //displacements[0] already equals to 0 after initialisation, but...
            for (int i = 1; i < processesCount; i++) {
                displacements[i] = displacements[i - 1] + processBlocksLengths[i - 1];
            }
        }
        MPI.COMM_WORLD.Scatterv(
                array,
                0,
                processBlocksLengths,
                displacements,
                MPI.INT,
                processBlock,
                0,
                processBlockLength,
                MPI.INT,
                rootRank
        );

        Arrays.sort(processBlock);

        int m = arrayLength/(processesCount * processesCount);
        int[] set = new int[processesCount];
        for (int i = 0; i < processesCount; i++) {
            set[i] = processBlock[i * m];
        }

        /*
         * 2 Phase
         * Gathering sets from each process, putting them into recvSets on rootRank process;
         * Forming newLeadingSet using elements in sorted recvSets;
         * Broadcasting newLeadingSet
        */

        int[] recvSets = new int[processesCount * processesCount];
        MPI.COMM_WORLD.Gather(
                set,
                0,
                processesCount,
                MPI.INT,
                recvSets,
                0,
                processesCount,
                MPI.INT,
                rootRank
        );

        int[] newLeadingSet = new int[processesCount - 1];
        if (isRoot(rank)) {
            int[] sortedSets = _mergeSortedArrays1(recvSets, processesCount);

            for (int i = 0; i < processesCount - 1; i++) {
                int j = i + 1;
                newLeadingSet[i] = sortedSets[j * processesCount + processesCount/2 - 1];
            }
        }

        MPI.COMM_WORLD.Bcast(
                newLeadingSet,
                0,
                newLeadingSet.length,
                MPI.INT,
                rootRank
        );

        /*
         * 3 Phase
         * Finding processBlockPartsLengths (lengths of processBlock parts when split by newLeadingSet elements)
         * Sending these lengths alltoall to finalProcessBlockPartsLength
         * Calculating finalProcessBlockLength, displacements
         * Sending parts alltoall
        */

        int[] offsets = new int[processesCount];
        int[] processBlockPartsLengths = new int[processesCount];
        offsets[0] = 0;
        for (int i = 1; i < processesCount; i++) {
            offsets[i] = binarySearch(newLeadingSet[i - 1], processBlock, offsets[i - 1], processBlockLength);
            processBlockPartsLengths[i - 1] = offsets[i] - offsets[i - 1];
        }
        processBlockPartsLengths[processesCount - 1] = processBlockLength - offsets[processesCount - 1];

        int[] finalProcessBlockPartsLength = new int[processesCount];
        MPI.COMM_WORLD.Alltoall(
                processBlockPartsLengths,
                0,
                1,
                MPI.INT,
                finalProcessBlockPartsLength,
                0,
                1,
                MPI.INT
        );

        int finalProcessBlockLength = 0;
        for (int i = 0; i < processesCount; i++) {
            finalProcessBlockLength += finalProcessBlockPartsLength[i];
        }

        for (int i = 1; i < processesCount; i++) {
            displacements[i] = displacements[i - 1] + processBlockPartsLengths[i - 1];
        }

        int[] rdisplacements = new int[processesCount];
        for (int i = 1; i < processesCount; i++) {
            rdisplacements[i] = rdisplacements[i - 1] + finalProcessBlockPartsLength[i - 1];
        }

        int[] finalProcessBlock = new int[finalProcessBlockLength];
        MPI.COMM_WORLD.Alltoallv(
                processBlock,
                0,
                processBlockPartsLengths,
                displacements,
                MPI.INT,
                finalProcessBlock,
                0,
                finalProcessBlockPartsLength,
                rdisplacements,
                MPI.INT
        );

        /*
         * 4 Phase
         * Sorting finalProcessBlock on each process
         * Gathering finalProcessBlocksLengths to rootRank process (root needs to know each final process block size)
         * Calculating rdisplacements
         * Gathering finalProcessBlock to rootRank process into result
         * END
        */

        int[] finalProcessBlockSorted = _mergeSortedArrays2(finalProcessBlock, finalProcessBlockPartsLength);
        int[] finalProcessBlocksLengths = new int[processesCount];
        int[] _finalProcessBlockLength = new int[]{finalProcessBlockLength};
        MPI.COMM_WORLD.Gather(
                _finalProcessBlockLength,
                0,
                1,
                MPI.INT,
                finalProcessBlocksLengths,
                0,
                1,
                MPI.INT,
                rootRank
        );

        for (int i = 1; i < processesCount; i++) {
            rdisplacements[i] = rdisplacements[i - 1] + finalProcessBlocksLengths[i - 1];
        }

        int[] result = new int[0];
        if (isRoot(rank)) {
            result = new int[arrayLength];
        }

        MPI.COMM_WORLD.Gatherv(
                finalProcessBlockSorted,
                0,
                finalProcessBlockLength,
                MPI.INT,
                result,
                0,
                finalProcessBlocksLengths,
                rdisplacements,
                MPI.INT,
                rootRank
        );

        return result;
    }

    public static void main(String args[]) throws MPIException, FileNotFoundException {
        String appArgs[] = MPI.Init(args);

        /*
         * appArgs[0] have to contain input file path
         * appArgs[1] have to contain output file path
         * so launch like this: ./mpjrun.sh  -np 3 QsortByRegularSampling unsorted2.txt sorted2.txt
         *
        */

        processesCount = MPI.COMM_WORLD.Size();
        rank = MPI.COMM_WORLD.Rank();

        int[] array = new int[0];

        if (isRoot(rank)) {
            if (new File(appArgs[0]).exists()) {
                array = FilesIO.readArrayFromFile(appArgs[0]);
            } else {
                throw new FileNotFoundException();
            }

        }

        if (isRoot(rank) && array.length < processesCount) {
            Arrays.sort(array); //too small array, sort only on root using built-in sort method
        } else {
            array = sort(array);
        }

        if (isRoot(rank)) {
            FilesIO.writeArrayToFile(appArgs[1], array);
        }

        MPI.Finalize();
    }
}