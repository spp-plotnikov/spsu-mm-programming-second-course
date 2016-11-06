import java.awt.event.KeyEvent;
import java.awt.event.KeyListener;
import java.awt.*;

public class KeyCatcher implements KeyListener {
    Frame frame;
    Runnable callBack;
    public KeyCatcher(Runnable callBack) {
        frame = new Frame("Window for detecting key pressed ;)");
        frame.setSize(300, 200);
        frame.setVisible(true);
        frame.addKeyListener(this);
        this.callBack = callBack;
    }

    public void keyPressed(KeyEvent e) {
        System.out.println("Key pressed");
        callBack.run();
        frame.dispose();
    }

    public void keyReleased(KeyEvent e) {

    }

    public void keyTyped(KeyEvent e) {

    }
}
