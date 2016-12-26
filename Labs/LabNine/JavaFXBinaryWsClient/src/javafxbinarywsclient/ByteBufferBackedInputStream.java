/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package javafxbinarywsclient;

import java.io.IOException;
import java.io.InputStream;
import java.nio.ByteBuffer;

/**
 *
 * @author Katrin
 */
public class ByteBufferBackedInputStream extends InputStream {

    ByteBuffer buf;

    public ByteBufferBackedInputStream(ByteBuffer buf) {
        this.buf = buf;
    }

    public synchronized int read() throws IOException {
        if (!buf.hasRemaining()) {
            return -1;
        }
        return buf.get();
    }

    public synchronized int read(byte[] bytes, int off, int len) throws IOException {
        len = Math.min(len, buf.remaining());
        buf.get(bytes, off, len);
        return len;
    }
}
