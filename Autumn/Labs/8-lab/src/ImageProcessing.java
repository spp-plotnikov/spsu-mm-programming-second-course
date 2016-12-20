import com.sun.org.apache.xerces.internal.impl.dv.util.Base64;
import org.eclipse.jetty.websocket.api.Session;
import org.json.JSONObject;
import javax.imageio.ImageIO;
import java.awt.image.BufferedImage;
import java.awt.image.BufferedImageOp;
import java.awt.image.Kernel;
import java.io.ByteArrayInputStream;
import java.io.ByteArrayOutputStream;
import java.io.IOException;

public class ImageProcessing implements Runnable {
    private Session session;
    private String imgB64;
    private Filter filter;
    private volatile Boolean isCancelled;
    private long prevTime;
    private ImageProcessingStorage storage;


    ImageProcessing(ImageProcessingStorage storage, Session session, String imgB64, Filter filter) {
        this.storage = storage;
        this.session = session;
        this.imgB64 = imgB64;
        this.filter = filter;
        this.isCancelled = false;
        this.prevTime = System.currentTimeMillis();
    }

    public Session getSession() {
        return session;
    }

    public Boolean isCancelled() {
        return isCancelled;
    }

    public void cancel() {
        isCancelled = true;
    }

    public void tryToSendProgress(double progress) {
        long now = System.currentTimeMillis();

        if (now - prevTime > 1000) {
            sendProgress(progress);
            prevTime = now;
        }
    }

    public void sendProgress(double progress) {
        JSONObject obj = new JSONObject();
        obj.put("commandType", 1);
        obj.put("status", 2);
        obj.put("progress", progress);
        try {
            session.getRemote().sendString(obj.toString());
        } catch (IOException e) {
            e.printStackTrace();
        }
    }

    private void sendImage(String imgB64) {
        JSONObject obj = new JSONObject();
        obj.put("commandType", 1);
        obj.put("status", 3);
        try {
            session.getRemote().sendString(obj.toString());
        } catch (IOException e) {
            e.printStackTrace();
        }


        JSONObject objResult = new JSONObject();
        objResult.put("commandType", 1);
        objResult.put("imgB64", imgB64);
        objResult.put("status", 4);
        try {
            session.getRemote().sendString(objResult.toString());
        } catch (IOException e) {
            e.printStackTrace();
        }

    }

    @Override
    public void run() {
        try {
            String imgB64Splitted = imgB64.split(",")[1];
            byte[] imageBytes = javax.xml.bind.DatatypeConverter.parseBase64Binary(imgB64Splitted);
            BufferedImage imgSource = ImageIO.read(new ByteArrayInputStream(imageBytes));;

            Kernel kernel = new Kernel(filter.getWidth(), filter.getHeight(), filter.getKernel());
            BufferedImageOp blur = new ModifiedConvolveOp(kernel, ModifiedConvolveOp.EDGE_NO_OP, null, this);
            BufferedImage imgDest = blur.filter(imgSource, null);

            if (imgDest != null && !isCancelled()) {
                ByteArrayOutputStream out = new ByteArrayOutputStream();

                ImageIO.write(imgDest, "PNG", out);

                byte[] bytes = out.toByteArray();

                String base64bytes = Base64.encode(bytes);
                String result = "data:image/png;base64," + base64bytes;

                sendProgress(100.0d);
                sendImage(result);

            } else if (isCancelled()) {
                JSONObject objReturn = new JSONObject();
                objReturn.put("commandType", 2);
                objReturn.put("status", 1);

                session.getRemote().sendString(objReturn.toString());

            }  else {
                JSONObject objReturn = new JSONObject();
                objReturn.put("commandType", 1);
                objReturn.put("status", 5);

                session.getRemote().sendString(objReturn.toString());
            }
        } catch (IOException e) {
            e.printStackTrace();

            JSONObject objReturn = new JSONObject();
            objReturn.put("commandType", 1);
            objReturn.put("status", 5);
            try {
                session.getRemote().sendString(objReturn.toString());
            } catch (IOException es) {
                es.printStackTrace();
            }
        }

        storage.removeImageProcessing(session);
    }
}
