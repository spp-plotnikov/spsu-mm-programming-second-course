/* ConvolveOp.java --
    Copyright (C) 2004, 2005, 2006, Free Software Foundation -- ConvolveOp

    This file is part of GNU Classpath.

    GNU Classpath is free software; you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation; either version 2, or (at your option)
    any later version.

  GNU Classpath is distributed in the hope that it will be useful, but
  WITHOUT ANY WARRANTY; without even the implied warranty of
  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
  General Public License for more details.

  You should have received a copy of the GNU General Public License
  along with GNU Classpath; see the file COPYING.  If not, write to the
  Free Software Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA
  02110-1301 USA.

  Linking this library statically or dynamically with other modules is
  making a combined work based on this library.  Thus, the terms and
  conditions of the GNU General Public License cover the whole
  combination.

  As a special exception, the copyright holders of this library give you
  permission to link this library with independent modules to produce an
  executable, regardless of the license terms of these independent
  modules, and to copy and distribute the resulting executable under
  terms of your choice, provided that you also meet, for each linked
  independent module, the terms and conditions of the license of that
  module.  An independent module is a module which is not derived from
  or based on this library.  If you modify this library, you may extend
  this exception to your version of the library, but you are not
  obligated to do so.  If you do not wish to do so, delete this
   exception statement from your version. */

import java.awt.RenderingHints;
import java.awt.geom.Point2D;
import java.awt.geom.Rectangle2D;
import java.awt.image.*;

// Copy-pasted from GNU-classpath
// Original ConvolveOp implementation
public class MyConvolveOp implements BufferedImageOp, RasterOp {
    private Kernel kernel;
    private RenderingHints hints;
    private ProgressSender progressSender;

    public MyConvolveOp(Kernel kernel, ProgressSender ps) {
        this.kernel = kernel;
        hints = new RenderingHints(
                RenderingHints.KEY_RENDERING,
                RenderingHints.VALUE_RENDER_QUALITY);
        progressSender = ps;
    }

    public BufferedImage createCompatibleDestImage(BufferedImage src, ColorModel dstCM) {
         if (dstCM != null)
             return new BufferedImage(dstCM,
                                   src.getRaster().createCompatibleWritableRaster(),
                                   src.isAlphaPremultiplied(), null);
        return new BufferedImage(src.getWidth(), src.getHeight(), src.getType());
    }

    public final WritableRaster filter(Raster src, WritableRaster dest)  {
        if (src == dest)
            throw new IllegalArgumentException("src == dest is not allowed.");
        if (kernel.getWidth() > src.getWidth()
                || kernel.getHeight() > src.getHeight())
            throw new ImagingOpException("The kernel is too large.");
        if (dest == null)
            dest = createCompatibleDestRaster(src);
        else if (src.getNumBands() != dest.getNumBands())
            throw new ImagingOpException("src and dest have different band counts.");

        // calculate the borders that the op can't reach...
        int kWidth = kernel.getWidth();
        int kHeight = kernel.getHeight();
        int left = kernel.getXOrigin();
        int right = Math.max(kWidth - left - 1, 0);
        int top = kernel.getYOrigin();
        int bottom = Math.max(kHeight - top - 1, 0);

        // Calculate max sample values for clipping
        int[] maxValue = src.getSampleModel().getSampleSize();
        for (int i = 0; i < maxValue.length; i++)
            maxValue[i] = (int)Math.pow(2, maxValue[i]) - 1;

        // process the region that is reachable...
        int regionW = src.getWidth() - left - right;
        int regionH = src.getHeight() - top - bottom;
        float[] kvals = kernel.getKernelData(null);
        float[] tmp = new float[kWidth * kHeight];
        double perStep = 100.0 / regionW;
        double progress = 0;
        int percents = 0;

        for (int x = 0; x < regionW; x++)  {
            for (int y = 0; y < regionH; y++) {
                for (int b = 0; b < src.getNumBands(); b++) {
                    float v = 0;
                    src.getSamples(x, y, kWidth, kHeight, b, tmp);
                    for (int i = 0; i < tmp.length; i++)
                        v += tmp[tmp.length - i - 1] * kvals[i];
                    if (v > maxValue[b])
                        v = maxValue[b];
                    else if (v < 0)
                        v = 0;
                    dest.setSample(x + kernel.getXOrigin(), y + kernel.getYOrigin(), b, v);
                }
            }
            progress += perStep;
            try {
                Thread.sleep(3);
            } catch (InterruptedException e) {

            }
            if (progress > 1.0) {
                percents += 1;
                progressSender.send(percents);
                progress -= 1.0;
            }
        }

        // fill in the top border
        fillEdge(src, dest, 0, 0, src.getWidth(), top);

        fillEdge(src, dest, 0, src.getHeight() - bottom, src.getWidth(), bottom);

        // fill in the left border
        fillEdge(src, dest, 0, top, left, regionH);

        // fill in the right border
        fillEdge(src, dest, src.getWidth() - right, top, right, regionH);

        return dest;
    }

    private void fillEdge(Raster src, WritableRaster dest, int x, int y, int w, int h) {
        if (w <= 0)
            return;
        if (h <= 0)
            return;

        // set borders to zeros
        float[] zeros = new float[src.getNumBands() * w * h];
        dest.setPixels(x, y, w, h, zeros);
    }

    public final BufferedImage filter(BufferedImage src, BufferedImage dst)  {
        if (src == dst)
            throw new IllegalArgumentException("Source and destination images " +
                    "cannot be the same.");
        if (dst == null)
            dst = createCompatibleDestImage(src, (ColorModel) src.getColorModel());

        // Make sure source image is premultiplied
        BufferedImage src1 = src;

        BufferedImage dst1 = dst;
        if (src1.getColorModel().getColorSpace().getType() != dst.getColorModel().getColorSpace().getType())
            dst1 = createCompatibleDestImage(src, src.getColorModel());

        filter(src1.getRaster(), dst1.getRaster());
        // Convert between color models if needed
        if (dst1 != dst)
            new ColorConvertOp(hints).filter(dst1, dst);
        return dst;
    }

    public final RenderingHints getRenderingHints()  {
        return hints;
    }

    public WritableRaster createCompatibleDestRaster(Raster src) {
        return src.createCompatibleWritableRaster();
    }

    public final Rectangle2D getBounds2D(BufferedImage src) {
        return src.getRaster().getBounds();
    }

    public final Rectangle2D getBounds2D(Raster src) {
        return src.getBounds();
    }

    public final Point2D getPoint2D(Point2D src, Point2D dst) {
        if (dst == null)
            return (Point2D)src.clone();
        dst.setLocation(src);
        return dst;
    }
 }