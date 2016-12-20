import org.eclipse.jetty.websocket.api.Session;
import org.eclipse.jetty.websocket.api.annotations.*;
import org.json.JSONException;
import org.json.JSONObject;
import java.io.IOException;
import java.util.concurrent.ExecutorService;

@WebSocket(maxTextMessageSize = 15 * 1024 * 1024, maxBinaryMessageSize = 15 * 1024 * 1024, maxIdleTime=1000000)
public class MyWebSocketHandler {
    private ExecutorService workersPool;
    private Filter[] filters;
    private ImageProcessingStorage storage;

    public MyWebSocketHandler(ImageProcessingStorage storage, ExecutorService workersPool, Filter[] filters) {
        this.workersPool = workersPool;
        this.filters = filters;
        this.storage = storage;
    }

    @OnWebSocketClose
    public void onClose(int statusCode, String reason) {
        System.out.println("Close: statusCode=" + statusCode + ", reason=" + reason);
    }

    @OnWebSocketError
    public void onError(Throwable t) {
        System.out.println("Error: " + t.getMessage());
    }

    @OnWebSocketConnect
    public void onConnect(Session session) {
        System.out.println("Connect: " + session.getRemoteAddress().getAddress());

        JSONObject obj = new JSONObject();
        obj.put("commandType", 0);
        obj.put("greeting", "Hello WebBrowser");
        try {
            session.getRemote().sendString(obj.toString());
        } catch (IOException e) {
            e.printStackTrace();
        }
    }

    @OnWebSocketMessage
    public void onMessage(Session session, String message) {
        try {
            JSONObject obj = new JSONObject(message);
            JSONObject objReturn = new JSONObject();

            switch (obj.getInt("commandType")) {
                case 0:
                    System.out.println("Greeting: " + obj.getString("greeting"));
                    break;

                case 1:
                    int filterId = obj.getInt("filter");
                    if (filterId >= filters.length) {
                        objReturn.put("commandType", 1);
                        objReturn.put("status", 0);
                        session.getRemote().sendString(objReturn.toString());

                        return;
                    }

                    if (storage.getImageProcessing(session) != null) {
                        objReturn.put("commandType", 1);
                        objReturn.put("status", 6);
                        session.getRemote().sendString(objReturn.toString());

                        return;
                    }

                    ImageProcessing ip = new ImageProcessing(storage, session, obj.getString("imgB64"), filters[filterId]);

                    storage.putImageProcessing(session, ip);
                    workersPool.submit(ip);

                    objReturn.put("commandType", 1);
                    objReturn.put("status", 1);
                    session.getRemote().sendString(objReturn.toString());

                    break;

                case 2:
                    ImageProcessing ipToCancel = storage.getImageProcessing(session);
                    if (ipToCancel == null) {
                        objReturn.put("commandType", 2);
                        objReturn.put("status", 2);
                        session.getRemote().sendString(objReturn.toString());
                    } else {
                        ipToCancel.cancel();
                    }

                    break;
            }
        } catch (JSONException | IOException e2) {
            JSONObject objReturn = new JSONObject();
            objReturn.put("commandType", 1);
            objReturn.put("status", 5);
            try {
                session.getRemote().sendString(objReturn.toString());
            } catch (IOException es) {
                es.printStackTrace();
            }
        }
    }
}