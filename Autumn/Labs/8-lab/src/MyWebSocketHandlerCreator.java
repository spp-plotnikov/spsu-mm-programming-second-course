import org.eclipse.jetty.websocket.servlet.ServletUpgradeRequest;
import org.eclipse.jetty.websocket.servlet.ServletUpgradeResponse;
import org.eclipse.jetty.websocket.servlet.WebSocketCreator;
import java.util.concurrent.ExecutorService;

public class MyWebSocketHandlerCreator implements WebSocketCreator {
    private ExecutorService workersPool;
    private Filter[] filters;

    public MyWebSocketHandlerCreator(ExecutorService workersPool, Filter[] filters) {
        this.workersPool = workersPool;
        this.filters = filters;
    }

    @Override
    public Object createWebSocket(ServletUpgradeRequest request, ServletUpgradeResponse response) {
        return new MyWebSocketHandler(workersPool, filters);
    }
}
