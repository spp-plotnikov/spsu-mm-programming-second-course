import org.eclipse.jetty.server.*;
import org.eclipse.jetty.server.handler.ContextHandler;
import org.eclipse.jetty.websocket.server.WebSocketHandler;
import org.eclipse.jetty.websocket.servlet.WebSocketServletFactory;
import org.json.JSONArray;
import org.json.JSONObject;
import java.util.concurrent.ExecutorService;
import java.util.concurrent.Executors;


public class ImageProcessingServer {
    private ExecutorService workersPool;
    private ImageProcessingStorage storage;
    private Filter[] filters;

    public ImageProcessingServer(int httpPort, int wsPort) throws Exception {
        workersPool = Executors.newFixedThreadPool(5);
        filters = parseConfig("config.json");
        storage = new ImageProcessingStorage();

        Server httpServer = new Server(httpPort);
        ContextHandler httpContextHandler = new ContextHandler("/");
        httpContextHandler.setContextPath("/");
        httpContextHandler.setHandler(new MyHttpHandler(filters));
        httpServer.setHandler(httpContextHandler);

        WebSocketHandler wsHandler = new WebSocketHandler() {
            @Override
            public void configure(WebSocketServletFactory factory) {
                factory.setCreator(new MyWebSocketHandlerCreator(storage, workersPool, filters));
            }
        };
        Server wsServer = new Server(wsPort);
        wsServer.setHandler(wsHandler);

        wsServer.start();
        httpServer.start();
    }

    private Filter[] parseConfig(String path) {
        String config = FileIO.readFile(path);

        JSONObject configObj = new JSONObject(config);
        JSONArray filtersArray = configObj.getJSONArray("filters");

        Filter filters[] = new Filter[filtersArray.length()];

        for (int i = 0; i < filtersArray.length(); i++) {
            JSONObject oneFilter = filtersArray.getJSONObject(i);

            JSONArray kernelJson = oneFilter.getJSONArray("kernel");

            float[] kernel = new float[kernelJson.length()];
            for (int j = 0; j < kernelJson.length(); j++) {
                kernel[j] = (float)kernelJson.getDouble(j);
            }

            filters[i] = new Filter(oneFilter.getString("filterName"), oneFilter.getInt("width"), oneFilter.getInt("height"), kernel);
        }

        return filters;
    }
}