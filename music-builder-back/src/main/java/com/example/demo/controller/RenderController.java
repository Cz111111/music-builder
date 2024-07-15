package com.example.demo.controller;

import com.example.demo.common.R;
import com.example.demo.service.FileUploadService;
import com.example.demo.service.RenderService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.security.core.annotation.AuthenticationPrincipal;
import org.springframework.security.core.userdetails.UserDetails;
import org.springframework.util.LinkedMultiValueMap;
import org.springframework.util.MultiValueMap;
import org.springframework.web.bind.annotation.*;
import org.springframework.web.client.RestTemplate;
import org.springframework.web.multipart.MultipartFile;

import org.springframework.http.*;

import java.io.ByteArrayOutputStream;
import java.io.IOException;
import java.io.InputStream;

@RestController
@RequestMapping("/render")
public class RenderController {
    @Autowired
    RenderService renderService;

    @Autowired
    private FileUploadService fileUploadService;

    private final RestTemplate restTemplate = new RestTemplate();
    @PostMapping ("/one")
    public R submit(@RequestParam("midi") MultipartFile file,
                    @RequestParam("songword") String songword,
                    @RequestParam("region") String region,
                    @RequestParam("wavname") String wavname,
                    @AuthenticationPrincipal UserDetails userDetails
                    ){
 //       renderService.submit(renderInfo);
        System.out.println(file.getName());
        System.out.println(songword);
        System.out.println(region);
        System.out.println(wavname);
        /*
        if(true){
            return R.ok().message("生成成功");
        }
        return R.error().message("生成失败");
        */
        return R.ok().message("生成成功");
    }

    @PostMapping ("/two")
    public R request2(@RequestParam("wavname") MultipartFile file,
                      @RequestParam("name") String name,
                      @AuthenticationPrincipal UserDetails userDetails
    ) throws IOException {
        System.out.println(name);
        System.out.println(file.getName());

            // 将文件转换为MultipartFile
            // 创建请求头
        HttpHeaders headers = new HttpHeaders();
        headers.setContentType(MediaType.MULTIPART_FORM_DATA);
        headers.set("User-Agent", "Apifox/1.0.0 (https://apifox.com)");
        headers.set("Accept", "*/*");
        headers.set("Host", "localhost:7014");
        headers.set("Connection", "keep-alive");

        // 创建请求体
        MultiValueMap<String, Object> body = new LinkedMultiValueMap<>();
        body.add("wavFile", file);

            // 创建请求
        HttpEntity<MultiValueMap<String, Object>> requestEntity = new HttpEntity<>(body, headers);

            // 发送请求
        ResponseEntity<InputStream> response = restTemplate.postForEntity("http://192.168.56.85:8080/GetBackBackControllers/some", requestEntity, InputStream.class);

            // 处理响应
        if (response.getStatusCode().is2xxSuccessful()) {
            // 从响应中获取InputStream
            InputStream inputStream = response.getBody();

            // 将InputStream写入文件
            // 使用ByteArrayOutputStream来存储InputStream中的数据
            ByteArrayOutputStream byteArrayOutputStream = new ByteArrayOutputStream();
            byte[] buffer = new byte[4096];
            int bytesRead;
            while ((bytesRead = inputStream.read(buffer)) != -1) {
                byteArrayOutputStream.write(buffer, 0, bytesRead);
            }

            // 获取字节数据
            byte[] fileData = byteArrayOutputStream.toByteArray();

            boolean b = fileUploadService.uploadFile(fileData,name, userDetails.getUsername());
            // 指定保存文件的路径
            if(b){
                return R.ok().message("生成成功");
            }
        } else {
            System.out.println("Failed to get file: " + response.getStatusCode());
        }
        return R.error().message("生成失败");

    }

    @PostMapping ("/three")
    public R request3(@RequestParam("keyword") String keyword,
                      @RequestParam("name") String name,
                      @AuthenticationPrincipal UserDetails userDetails){
        System.out.println(keyword);
        System.out.println(name);

        return R.ok().message("生成成功");
    }

    @PostMapping ("/four")
    public R request4(@RequestParam("instrument") String instrument,
                      @RequestParam("name") String name,
                      @AuthenticationPrincipal UserDetails userDetails){
        System.out.println(instrument);
        System.out.println(name);

        return R.ok().message("生成成功");
    }

    @PostMapping ("/five")
    public R request5(@RequestParam("keyword") String keyword,
                      @RequestParam("name") String name,
                      @AuthenticationPrincipal UserDetails userDetails){
        System.out.println(keyword);
        System.out.println(name);

        return R.ok().message("生成成功");
    }

}
