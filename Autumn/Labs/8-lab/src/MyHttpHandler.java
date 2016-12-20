import org.eclipse.jetty.server.Request;
import org.eclipse.jetty.server.handler.AbstractHandler;
import javax.servlet.ServletException;
import javax.servlet.http.HttpServletRequest;
import javax.servlet.http.HttpServletResponse;
import java.io.IOException;


public class MyHttpHandler extends AbstractHandler {
    private Filter[] filters;

    MyHttpHandler(Filter[] filters) {
        this.filters = filters;
    }

    public void handle(String target, Request baseRequest, HttpServletRequest request, HttpServletResponse response)
            throws IOException, ServletException {
        response.setContentType("text/html; charset=utf-8");
        response.setStatus(HttpServletResponse.SC_OK);
        baseRequest.setHandled(true);
        response.getWriter().println(createIndexHtml());
    }

    private String createIndexHtml() {
        String file = FileIO.readFile("index.html");
        String fileWithFilters = file.replace("{{$filters}}", createFiltersSelectList());
        return fileWithFilters;
    }

    private String createFiltersSelectList() {
        String selectList = "";
        for (int i = 0; i < filters.length; i++) {
            selectList += "<option value=\"" + i + "\">" + filters[i].getName() + "</option>";
        }

        return selectList;
    }

}
