﻿using Microsoft.IO;
using Serilog;
using Simpra.Core.Logger;
using Simpra.Service.Exceptions;
using Simpra.Service.Response;
using System.Text.Json;

namespace Simpra.Api.Middleware;

public class RequestLoggingMiddleware
{
    private readonly RequestDelegate next;
    private readonly RecyclableMemoryStreamManager recyclableMemoryStreamManager;
    private readonly Action<RequestProfilerModel> requestResponseHandler;
    private const int ReadChunkBufferLength = 4096;
    public RequestLoggingMiddleware(RequestDelegate next,Action<RequestProfilerModel> requestResponseHandler)
    {
        this.next = next;
        this.requestResponseHandler = requestResponseHandler;
        this.recyclableMemoryStreamManager = new RecyclableMemoryStreamManager();
    }

    public async Task Invoke(HttpContext context)
    {
        Log.Information("LogRequestLoggingMiddleware.Invoke");

        var model = new RequestProfilerModel
        {
            RequestTime = new DateTimeOffset(),
            Context = context,
            Request = await FormatRequest(context)
        };

        Stream originalBody = context.Response.Body;

        using (MemoryStream newResponseBody = recyclableMemoryStreamManager.GetStream())
        {
            context.Response.Body = newResponseBody;

            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                HandleException(ex, context);
            }

            newResponseBody.Seek(0, SeekOrigin.Begin);
            await newResponseBody.CopyToAsync(originalBody);

            newResponseBody.Seek(0, SeekOrigin.Begin);
            model.Response = FormatResponse(context, newResponseBody);
            model.ResponseTime = new DateTimeOffset();
            requestResponseHandler(model);
        }    
    }

    private string FormatResponse(HttpContext context, MemoryStream newResponseBody)
    {
        HttpRequest request = context.Request;
        HttpResponse response = context.Response;

        return $"Http Response Information: {Environment.NewLine}" +
                $"Schema:{request.Scheme} {Environment.NewLine}" +
                $"Host: {request.Host} {Environment.NewLine}" +
                $"Path: {request.Path} {Environment.NewLine}" +
                $"QueryString: {request.QueryString} {Environment.NewLine}" +
                $"StatusCode: {response.StatusCode} {Environment.NewLine}" +
                $"Response Body: {ReadStreamInChunks(newResponseBody)}";
    }
    private async Task<string> FormatRequest(HttpContext context)
    {
        HttpRequest request = context.Request;

        return $"Http Request Information: {Environment.NewLine}" +
                    $"Schema:{request.Scheme} {Environment.NewLine}" +
                    $"Host: {request.Host} {Environment.NewLine}" +
                    $"Path: {request.Path} {Environment.NewLine}" +
                    $"QueryString: {request.QueryString} {Environment.NewLine}" +
                    $"Request Body: {await GetRequestBody(request)}";
    }
    public async Task<string> GetRequestBody(HttpRequest request)
    {
        request.EnableBuffering();
        using (var requestStream = recyclableMemoryStreamManager.GetStream())
        {
            await request.Body.CopyToAsync(requestStream);
            request.Body.Seek(0, SeekOrigin.Begin);
            return ReadStreamInChunks(requestStream);
        }
    }

    private static string ReadStreamInChunks(Stream stream)
    {
        stream.Seek(0, SeekOrigin.Begin);
        string result;
        using (var textWriter = new StringWriter())
        using (var reader = new StreamReader(stream))
        {
            var readChunk = new char[ReadChunkBufferLength];
            int readChunkLength;

            do
            {
                readChunkLength = reader.ReadBlock(readChunk, 0, ReadChunkBufferLength);
                textWriter.Write(readChunk, 0, readChunkLength);
            } while (readChunkLength > 0);

            result = textWriter.ToString();
        }

        return result;
    }

    private void HandleException(Exception ex, HttpContext context)
    {
        Log.Error(ex, "An unhandled exception occurred");

        context.Response.ContentType = "application/json";

        var statusCode = ex switch
        {
            ClientSideException => 400,
            NotFoundException => 404,
            _ => 500
        };
        context.Response.StatusCode = statusCode;

        var response = CustomResponse<NoContent>.Fail(statusCode, ex.Message);

        context.Response.WriteAsync(JsonSerializer.Serialize(response));
    }


}
