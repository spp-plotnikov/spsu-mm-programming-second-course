using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using System.Text.RegularExpressions;
using System.IO;
using System.Security.Cryptography;
using System.Linq;
using System.Web.Script.Serialization;
using System.Drawing;
using Fleck;

class Client
{

    /*
     *
     *  This class handles only static requests - GET
     *
     */

    // Server root directory
    private const string rootPath = "../../www";

    // Name of index file, need for redirecting from "/" 
    private const string indexFile = "index.html";

    // socket object
    private TcpClient client;


    public Client(TcpClient client)
    {
        this.client = client;
        string httpHeaders;


        var tcp = new TcpTransfer();

        // Skip frames without HTTP 1.1
        if (!IsHTTP(httpHeaders = Encoding.ASCII.GetString(tcp.Receive(client))))
            return;

        // Debug
        //Console.WriteLine("-----------------------\n" + httpHeaders + "\n---------------------------\n");


        // Matching request url for HTTP headers
        string requestUrl = GetReuqestUrl(httpHeaders);

        // Append index file 
        if (requestUrl.EndsWith("/"))
            requestUrl = Path.Combine(requestUrl, indexFile);

        // Checking request and building file path
        string filePath;
        if (СheckRequestUrl(requestUrl))
            filePath = rootPath + requestUrl;
        else
            return;


        // Open file
        byte[] fileByBytes;
        try
        {
            fileByBytes = File.ReadAllBytes(filePath);
        }
        catch (Exception)
        {
            SendError(502);
            return;
        }

        // Send response
        tcp.Send(client, BuildHeaders("200 OK", GetContentType(filePath), fileByBytes.Length.ToString()));
        tcp.Send(client, fileByBytes);


        client.Close();
    }

    private void SendError(int status)
    {
        string codeStr = status.ToString() + " " + ((HttpStatusCode)status).ToString();
        string html = "<html><body><h1>" + codeStr + "</h1></body></html>";
        var tcp = new TcpTransfer();
        tcp.Send(client, BuildHeaders(codeStr, "text/html", html.Length.ToString()));
        tcp.Send(client, Encoding.ASCII.GetBytes(html));
        client.Close();
    }

    private byte[] BuildHeaders(string status, string contentType, string contentLength)
    {
        string headers = "HTTP/1.1 " + status + "\r\n"
            + "Content-Type: " + contentType + "\r\n"
            + "Content-Length: " + contentLength + "\r\n"
            + "Server: PSHOST\r\n"
            + "\r\n";
        byte[] headersEncoded = Encoding.ASCII.GetBytes(headers);

        return headersEncoded;
    }

    private bool СheckRequestUrl(string requestUrl)
    {
        if (requestUrl.IndexOf("..") >= 0)
        {
            SendError(400);
            return false;
        }

        if (!File.Exists(rootPath + requestUrl))
        {
            SendError(404);
            return false;
        }

        return true;
    }

    private string GetContentType(string filePath)
    {
        string extension = Path.GetExtension(filePath);

        string contentType = "";
        switch (extension)
        {
            case ".htm":
            case ".html":
                contentType = "text/html";
                break;
            case ".css":
                contentType = "text/css";
                break;
            case ".js":
                contentType = "text/javascript";
                break;
            case ".jpg":
                contentType = "image/jpeg";
                break;
            case ".jpeg":
            case ".png":
            case ".gif":
                contentType = "image/" + extension.Substring(1);
                break;
            default:
                if (extension.Length > 1)
                    contentType = "application/" + extension.Substring(1);
                else
                    contentType = "application/unknown";
                break;
        }

        return contentType;
    }

    private bool IsHTTP(string tcpRAW)
    {
        Match tryHttp = Regex.Match(tcpRAW, @"^GET(.*)HTTP\/1.1\r\n");

        if (tryHttp == Match.Empty)
            return false;

        return true;
    }

    private string GetReuqestUrl(string httpHeaders)
    {
        Match getReuqest = Regex.Match(httpHeaders, @"^GET(.*)HTTP\/1.1\r\n");
        return Uri.UnescapeDataString(getReuqest.Groups[1].Value).Trim();
    }
}