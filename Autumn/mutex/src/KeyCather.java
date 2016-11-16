import java.awt.event.KeyEvent;
import java.awt.event.KeyListener;
import java.awt.*;

class KeyCatcher implements KeyListener {
    Frame frame;
    boolean[] flag;
    public KeyCatcher(boolean flag[]) {
        frame = new Frame("Window for detecting key pressed ;)");
        frame.setSize(300, 200);
        frame.setVisible(true);
        frame.addKeyListener(this);
        this.flag = flag;
//        this.callBack = callBack;
    }

    public void keyPressed(KeyEvent e) {
        System.out.println("Key pressed");
        flag[0] = true;
        frame.dispose();
    }

    public void keyReleased(KeyEvent e) {

    }

    public void keyTyped(KeyEvent e) {

    }
}