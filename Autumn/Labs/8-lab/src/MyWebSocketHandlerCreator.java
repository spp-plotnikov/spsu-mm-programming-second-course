import org.eclipse.jetty.websocket.servlet.ServletUpgradeRequest;
import org.eclipse.jetty.websocket.servlet.ServletUpgradeResponse;
import org.eclipse.jetty.websocket.servlet.WebSocketCreator;
import java.util.concurrent.ExecutorService;

public class MyWebSocketHandlerCreator implements WebSocketCreator {
    private ExecutorService workersPool;
    private Filter[] filters;
    private ImageProcessingStorage storage;

    public MyWebSocketHandlerCreator(ImageProcessingStorage storage, ExecutorService workersPool, Filter[] filters) {
        this.workersPool = workersPool;
        this.filters = filters;
        this.storage = storage;
    }

    @Override
    public Object createWebSocket(ServletUpgradeRequest request, ServletUpgradeResponse response) {
        return new MyWebSocketHandler(storage, workersPool, filters);
    }
}
