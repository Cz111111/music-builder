package com.example.demo.service;

import com.example.demo.model.RenderInfo;
import org.springframework.stereotype.Service;
import org.springframework.web.client.RestTemplate;

import java.net.URLEncoder;
import java.nio.charset.StandardCharsets;
import java.util.StringJoiner;

@Service
public class RenderServiceImpl implements  RenderService{
    private RestTemplate restTemplate;
    private String baseUrl;
    @Override
    public String submit(RenderInfo renderInfo) {
        try {
            // 使用StringJoiner构建查询字符串
            StringJoiner queryJoiner = new StringJoiner("&");
            queryJoiner.add(URLEncoder.encode("lyrics", StandardCharsets.UTF_8) + "=" + URLEncoder.encode(renderInfo.getLyrics(), StandardCharsets.UTF_8));
            queryJoiner.add(URLEncoder.encode("midi", StandardCharsets.UTF_8) + "=" + URLEncoder.encode(renderInfo.getMidi(), StandardCharsets.UTF_8));
            queryJoiner.add(URLEncoder.encode("singer", StandardCharsets.UTF_8) + "=" + URLEncoder.encode(renderInfo.getSinger(), StandardCharsets.UTF_8));
            queryJoiner.add(URLEncoder.encode("wavName", StandardCharsets.UTF_8) + "=" + URLEncoder.encode(renderInfo.getWavname(), StandardCharsets.UTF_8));

            // 将查询参数附加到基础URL
            String urlWithQuery = baseUrl + "?" + queryJoiner.toString();

            // 发送GET请求并获取响应
            String response = restTemplate.getForObject(urlWithQuery, String.class);
            return response;
        } catch (Exception e) {
            e.printStackTrace();
            return "error"; // 根据需要进行错误处理
        }
    }
}
