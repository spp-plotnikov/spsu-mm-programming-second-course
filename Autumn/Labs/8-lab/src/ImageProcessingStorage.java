import org.eclipse.jetty.websocket.api.Session;
import java.util.concurrent.ConcurrentHashMap;

public class ImageProcessingStorage {
    private ConcurrentHashMap<Session, ImageProcessing> storage;

    public ImageProcessingStorage() {
        this.storage = new ConcurrentHashMap();
    }

    public void removeImageProcessing(Session session) {
        storage.remove(session);
    }

    public void putImageProcessing(Session session, ImageProcessing ip) {
        storage.put(session, ip);
    }

    public ImageProcessing getImageProcessing(Session session) {
        return storage.get(session);
    }
}
