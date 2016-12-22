import com.sun.org.apache.xerces.internal.impl.dv.util.Base64;
import org.json.JSONObject;

import java.awt.image.BufferedImage;
import java.io.ByteArrayOutputStream;
import java.io.File;
import java.io.IOException;
import java.net.URI;
import javax.imageio.ImageIO;
import javax.websocket.ClientEndpoint;
import javax.websocket.CloseReason;
import javax.websocket.ContainerProvider;
import javax.websocket.OnClose;
import javax.websocket.OnMessage;
import javax.websocket.OnOpen;
import javax.websocket.Session;
import javax.websocket.WebSocketContainer;


@ClientEndpoint
public class WebSocketClientEndpoint {
    private Session userSession;
    private Client client;

    public WebSocketClientEndpoint(URI endpointURI, Client client) {
        this.userSession = null;
        this.client = client;

        try {
            WebSocketContainer container = ContainerProvider.getWebSocketContainer();
            container.connectToServer(this, endpointURI);
        } catch (Exception e) {
            throw new RuntimeException(e);
        }
    }


    @OnOpen
    public void onOpen(Session userSession) {
        //System.out.println("Opening Websocket");
        this.userSession = userSession;


        //greeting
        JSONObject greetingObj = new JSONObject();
        greetingObj.put("commandType", 0);
        greetingObj.put("greeting", "Hello, WebServer. I'm test client #" + client.getId());
        sendMessage(greetingObj.toString());

        //reading image from file

        try {
            BufferedImage img = client.getTest().getImageSrc();
            ByteArrayOutputStream out = new ByteArrayOutputStream();
            ImageIO.write(img, "PNG", out);
            byte[] bytes = out.toByteArray();
            String base64bytes = Base64.encode(bytes);
            String imgB64 = "data:image/png;base64," + base64bytes;

            //sending image
            JSONObject imageObj = new JSONObject();
            imageObj.put("commandType", 1);
            imageObj.put("imgB64", imgB64);
            imageObj.put("filter", 3);
            sendMessage(imageObj.toString());

            client.timeStart();
        } catch (IOException e) {
            e.printStackTrace();
        }
    }

    @OnClose
    public void onClose(Session userSession, CloseReason reason) {
        this.userSession = null;
    }

    @OnMessage
    public void onMessage(String message) {
        JSONObject obj = new JSONObject(message);

        //ignore everything except {commandType: 1, status: 0 or 4 or 5}
        if (obj.getInt("commandType") == 1) {
            if (obj.getInt("status") == 4) {
                client.timeStop();
            }

            if (obj.getInt("status") == 0 || obj.getInt("status") == 5) {
                client.errorOccured();
            }
        }

    }


    public void sendMessage(String message) {
        userSession.getAsyncRemote().sendText(message);
    }

}




