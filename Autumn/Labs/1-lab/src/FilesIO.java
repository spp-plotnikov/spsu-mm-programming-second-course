import java.io.FileNotFoundException;
import java.io.FileReader;
import java.io.PrintWriter;
import java.util.ArrayList;
import java.util.Arrays;
import java.util.List;
import java.util.Scanner;
import java.util.stream.Collectors;

public class FilesIO {
    static int[] readArrayFromFile(String filename) throws FileNotFoundException {
        Scanner scanner = new Scanner(new FileReader(filename));
        List<Integer> list = new ArrayList<>();
        while (scanner.hasNextInt()) {
            list.add(scanner.nextInt());
        }
        return convertIntegers(list);
    }

    private static int[] convertIntegers(List<Integer> list) {
        return list.stream().mapToInt(i -> i).toArray();
    }

    static void writeArrayToFile(String filename, int[] array) throws FileNotFoundException {
        String result = Arrays.stream(array).mapToObj(String::valueOf).collect(Collectors.joining(" "));

        try (PrintWriter out = new PrintWriter(filename)) {
            out.print(result );
        }
    }
}
