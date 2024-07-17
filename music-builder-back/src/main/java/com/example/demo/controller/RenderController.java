package com.example.demo.controller;

import com.example.demo.common.R;
import com.example.demo.model.Song;
import com.example.demo.service.FileUploadService;
import com.example.demo.service.JpaSongService;
import com.example.demo.service.RenderService;
import jakarta.servlet.ServletContext;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.core.io.ByteArrayResource;
import org.springframework.core.io.FileSystemResource;
import org.springframework.http.client.SimpleClientHttpRequestFactory;
import org.springframework.http.converter.ByteArrayHttpMessageConverter;
import org.springframework.http.converter.HttpMessageConverter;
import org.springframework.http.converter.StringHttpMessageConverter;
import org.springframework.security.core.annotation.AuthenticationPrincipal;
import org.springframework.security.core.userdetails.UserDetails;
import org.springframework.util.LinkedMultiValueMap;
import org.springframework.util.MultiValueMap;
import org.springframework.web.bind.annotation.*;
import org.springframework.web.client.RestTemplate;
import org.springframework.web.multipart.MultipartFile;

import org.springframework.http.*;
import org.springframework.web.util.UriComponentsBuilder;

import javax.imageio.ImageIO;
import java.awt.image.BufferedImage;
import java.io.*;
import java.net.URI;
import java.net.URLEncoder;
import java.nio.ByteBuffer;
import java.nio.channels.FileChannel;
import java.nio.charset.StandardCharsets;
import java.nio.file.Files;
import java.nio.file.Path;
import java.nio.file.StandardCopyOption;
import java.nio.file.StandardOpenOption;
import java.util.List;

@RestController
@RequestMapping("/render")
public class RenderController {
    @Autowired
    RenderService renderService;

    @Autowired
    JpaSongService jpaSongService;


    @PostMapping ("/one")
    public R submit(@RequestParam("midi") MultipartFile file,
                    @RequestParam("songword") String songword,
                    @RequestParam("region") String region,
                    @RequestParam("wavname") String wavname,
                    @AuthenticationPrincipal UserDetails userDetails
                    ) throws IOException {
 //       renderService.submit(renderInfo);
        System.out.println(file.getName());
        System.out.println(songword);
        System.out.println(region);
        System.out.println(wavname);

        RestTemplate restTemplate = new RestTemplate();
        List<HttpMessageConverter<?>> messageConverters = restTemplate.getMessageConverters();
        ByteArrayHttpMessageConverter byteArrayConverter = new ByteArrayHttpMessageConverter();
        messageConverters.add(byteArrayConverter);
        restTemplate.setMessageConverters(messageConverters);
        String url = "http://192.168.43.212:8888/GetBackBackControllers/render";
        String uriStr = String.format("%s?singer=%s&lyrics=%s&wavName=%s", url, region, songword, wavname);
        // 创建请求头
        HttpHeaders headers = new HttpHeaders();
        headers.setContentType(MediaType.MULTIPART_FORM_DATA);
        headers.set("User-Agent", "Apifox/1.0.0 (https://apifox.com)");
        headers.set("Accept", "application/octet-stream"); // 假设响应内容类型为二进制流
        headers.set("Host", "localhost:8888");
        headers.set("Connection", "keep-alive");

        // 创建请求体
        MultiValueMap<String, Object> body = new LinkedMultiValueMap<>();
        body.add("midiFile", new ByteArrayResource(file.getBytes()) {
            @Override
            public String getFilename() {
                return file.getOriginalFilename();
            }
        }); // 添加文件
        System.out.println(uriStr);
        // 创建请求实体
        HttpEntity<MultiValueMap<String, Object>> requestEntity = new HttpEntity<>(body, headers);
        // 发送请求并接收响应
        ResponseEntity<byte[]> response = restTemplate.postForEntity(uriStr, requestEntity, byte[].class);

        // 处理响应
        if (response.getStatusCode().is2xxSuccessful()) {
            byte[] fileContent = response.getBody();
            if (fileContent != null) {
                String relativePath = "uploads/" + userDetails.getUsername() + "/";
                String relativePathToRoot = Path.of("").normalize().toString();
                String projectRootPath = new File(relativePathToRoot).getAbsolutePath();
                String absolutePath = new File(projectRootPath, relativePath).getAbsolutePath();
                // 创建目录
                File directory = new File(absolutePath);
                if (!directory.exists()) {
                    directory.mkdirs();
                }

                // 构建文件路径
                File outputFile = new File(directory, wavname+".wav");

                // 使用FileOutputStream写入数据
                try (FileOutputStream fos = new FileOutputStream(outputFile)) {
                    fos.write(fileContent);
                }
                System.out.println("文件已保存到: " + outputFile.getAbsolutePath());
                Song song = new Song(userDetails.getUsername(),wavname,
                        songword,
                        "uploads/"+userDetails.getUsername()+"/"+wavname+".wav");
                jpaSongService.insertSong(song);
                return R.ok().message("文件接收并保存成功");
            }
        }
        return R.error().message("文件接收失败");
    }

    @PostMapping ("/two")
    public R request2(@RequestParam("wavname") MultipartFile file,
                      @RequestParam("name") String name,
                      @AuthenticationPrincipal UserDetails userDetails
    ) throws IOException {
        System.out.println(name);
        System.out.println(file.getName());

        RestTemplate restTemplate = new RestTemplate();
        List<HttpMessageConverter<?>> messageConverters = restTemplate.getMessageConverters();
        ByteArrayHttpMessageConverter byteArrayConverter = new ByteArrayHttpMessageConverter();
        messageConverters.add(byteArrayConverter);
        restTemplate.setMessageConverters(messageConverters);
        String url = "http://192.168.43.212:8888/GetBackBackControllers/some";

        // 创建请求头
        HttpHeaders headers = new HttpHeaders();
        headers.setContentType(MediaType.MULTIPART_FORM_DATA);
        headers.set("User-Agent", "Your User Agent");
        headers.set("Accept", "application/octet-stream"); // 假设响应内容类型为二进制流
        headers.set("Host", "localhost:8888");
        headers.set("Connection", "keep-alive");

        // 创建请求体
        MultiValueMap<String, Object> body = new LinkedMultiValueMap<>();
        body.add("wavFile", new ByteArrayResource(file.getBytes()) {
            @Override
            public String getFilename() {
                return file.getOriginalFilename();
            }
        }); // 添加文件

        // 创建请求实体
        HttpEntity<MultiValueMap<String, Object>> requestEntity = new HttpEntity<>(body, headers);

        // 发送请求并接收响应
        ResponseEntity<byte[]> response = restTemplate.postForEntity(url, requestEntity, byte[].class);

        // 处理响应
        if (response.getStatusCode().is2xxSuccessful()) {
            byte[] fileContent = response.getBody();
            if (fileContent != null) {
                String relativePath = "uploads/" + userDetails.getUsername() + "/";
                String relativePathToRoot = Path.of("").normalize().toString();
                String projectRootPath = new File(relativePathToRoot).getAbsolutePath();
                String absolutePath = new File(projectRootPath, relativePath).getAbsolutePath();

                // 创建目录
                File directory = new File(absolutePath);
                if (!directory.exists()) {
                    directory.mkdirs();
                }
                long timestamp = System.currentTimeMillis();
                String newFilename = name + "_" + timestamp;
                // 构建文件路径
                File outputFile = new File(directory, newFilename+".midi");

                // 使用FileOutputStream写入数据
                try (FileOutputStream fos = new FileOutputStream(outputFile)) {
                    fos.write(fileContent);
                }
                System.out.println("文件已保存到: " + outputFile.getAbsolutePath());

                Song song = new Song(userDetails.getUsername(),name,
                        "无",
                        "uploads/"+userDetails.getUsername()+"/"+newFilename+".midi");
                jpaSongService.insertSong(song);
                return R.ok().message("文件接收并保存成功");
            }
        }
        return R.error().message("文件接收失败");
    }

    @PostMapping ("/three")
    public R request3(@RequestParam("keyword") String keyword,
                      @RequestParam("name") String name,
                      @AuthenticationPrincipal UserDetails userDetails) throws  IOException{
        System.out.println(keyword);
        System.out.println(name);
        RestTemplate restTemplate = new RestTemplate();

        // 使用UriComponentsBuilder构建URL
        String url = "http://192.168.43.212:8888/GetBackBackControllers/paint";
        URI uri = UriComponentsBuilder.fromUriString(url)
                .queryParam("keyWord", keyword)
                .queryParam("Pname", name)
                .encode()
                .build()
                .toUri();

        // 发送GET请求，直接获取响应体为byte数组
        ResponseEntity<byte[]> response = restTemplate.getForEntity(uri, byte[].class);

        if (response.getStatusCode().is2xxSuccessful()) {
            // 获取响应体的字节数据
            byte[] responseBody = response.getBody();

            // 指定保存图片的路径和文件名
            String relativePath = "uploads/"+userDetails.getUsername()+"/"; // 替换为实际路径
            String relativePathToRoot = Path.of("").normalize().toString();
            String projectRootPath = new File(relativePathToRoot).getAbsolutePath();
            String absolutePath = new File(projectRootPath, relativePath).getAbsolutePath();
            System.out.println(absolutePath);
            File outputFile = new File(absolutePath, name+".png"); // 可以指定文件名和格式


            // 将字节数据写入文件
            try (FileOutputStream fos = new FileOutputStream(outputFile)) {
                fos.write(responseBody);
            }
            Song song = new Song(userDetails.getUsername(),name,
                    "这是一个图片",
                    "uploads/"+userDetails.getUsername()+"/"+name+".png");
            jpaSongService.insertSong(song);
            return R.ok().message("图片保存成功");
        } else {
            return R.error().message("请求失败，状态码：" + response.getStatusCode());
        }
    }

    @PostMapping ("/four")
    public R request4(@RequestParam("instrument") String instrument,
                      @RequestParam("name") String name,
                      @AuthenticationPrincipal UserDetails userDetails) throws FileNotFoundException {
        System.out.println(instrument);
        System.out.println(name);
        RestTemplate restTemplate = new RestTemplate();

        // 使用UriComponentsBuilder构建URL
        String url = "http://192.168.43.212:8888/GetBackBackControllers/arrange";
        URI uri = UriComponentsBuilder.fromUriString(url)
                .queryParam("instrument", instrument)
                .encode()
                .build()
                .toUri();

        // 发送GET请求，直接获取响应体为byte数组
        ResponseEntity<byte[]> response = restTemplate.getForEntity(uri, byte[].class);

        // 处理响应
        if (response.getStatusCode().is2xxSuccessful()) {
            byte[] fileContent = response.getBody();
            if (fileContent != null) {
                String relativePath = "uploads/" + userDetails.getUsername() + "/";
                String relativePathToRoot = Path.of("").normalize().toString();
                String projectRootPath = new File(relativePathToRoot).getAbsolutePath();
                String absolutePath = new File(projectRootPath, relativePath).getAbsolutePath();

                // 创建目录
                File directory = new File(absolutePath);
                if (!directory.exists()) {
                    directory.mkdirs();
                }

                // 构建文件路径
                File outputFile = new File(directory, name+".midi");

                // 使用FileOutputStream写入数据
                try (FileOutputStream fos = new FileOutputStream(outputFile)) {
                    fos.write(fileContent);
                } catch (IOException e) {
                    throw new RuntimeException(e);
                }
                System.out.println("文件已保存到: " + outputFile.getAbsolutePath());
                Song song = new Song(userDetails.getUsername(),name,
                        "无",
                        "uploads/"+userDetails.getUsername()+"/"+name+".midi");
                jpaSongService.insertSong(song);
                return R.ok().message("文件接收并保存成功");
            }
        }
        return R.error().message("文件接收失败");
    }

    @PostMapping ("/five")
    public R request5(@RequestParam("keyword") String keyword,
                      @RequestParam("name") String name,
                      @AuthenticationPrincipal UserDetails userDetails){
        System.out.println(keyword);
        System.out.println(name);

        RestTemplate restTemplate = new RestTemplate();

        // 使用UriComponentsBuilder构建URL
        String url = "http://192.168.43.212:8888/GetBackBackControllers/music";
        URI uri = UriComponentsBuilder.fromUriString(url)
                .queryParam("description", keyword)
                .encode()
                .build()
                .toUri();

        // 发送GET请求，直接获取响应体为byte数组
        ResponseEntity<byte[]> response = restTemplate.getForEntity(uri, byte[].class);

        // 处理响应
        if (response.getStatusCode().is2xxSuccessful()) {
            byte[] fileContent = response.getBody();
            if (fileContent != null) {
                String relativePath = "uploads/" + userDetails.getUsername() + "/";
                String relativePathToRoot = Path.of("").normalize().toString();
                String projectRootPath = new File(relativePathToRoot).getAbsolutePath();
                String absolutePath = new File(projectRootPath, relativePath).getAbsolutePath();

                // 创建目录
                File directory = new File(absolutePath);
                if (!directory.exists()) {
                    directory.mkdirs();
                }

                // 构建文件路径
                File outputFile = new File(directory, name+".wav");

                // 使用FileOutputStream写入数据
                try (FileOutputStream fos = new FileOutputStream(outputFile)) {
                    fos.write(fileContent);
                } catch (IOException e) {
                    throw new RuntimeException(e);
                }
                System.out.println("文件已保存到: " + outputFile.getAbsolutePath());
                Song song = new Song(userDetails.getUsername(),name,
                        "无",
                        "uploads/"+userDetails.getUsername()+"/"+name+".wav");
                jpaSongService.insertSong(song);
                return R.ok().message("文件接收并保存成功");
            }
        }
        return R.error().message("文件接收失败");
    }

}
