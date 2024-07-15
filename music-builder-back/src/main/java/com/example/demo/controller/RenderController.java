package com.example.demo.controller;

import com.example.demo.common.R;
import com.example.demo.model.*;
import com.example.demo.service.RenderService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.web.bind.annotation.*;
import org.springframework.web.multipart.MultipartFile;

@RestController
@RequestMapping("/render")
public class RenderController {
    @Autowired
    RenderService renderService;
    @PostMapping ("/one")
    public R submit(@RequestParam("midi") MultipartFile file,
                    @RequestParam("songword") String songword,
                    @RequestParam("region") String region,
                    @RequestParam("wavname") String wavname
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
    public R request2(@RequestParam("wavname") MultipartFile file, @RequestParam("name") String name){
        System.out.println(name);
        System.out.println(file.getName());

        return R.ok().message("生成成功");
    }

    @PostMapping ("/three")
    public R request3(@RequestParam("keyword") String keyword, @RequestParam("name") String name){
        System.out.println(keyword);
        System.out.println(name);

        return R.ok().message("生成成功");
    }

    @PostMapping ("/four")
    public R request4(@RequestParam("instrument") String instrument, @RequestParam("name") String name){
        System.out.println(instrument);
        System.out.println(name);

        return R.ok().message("生成成功");
    }

    @PostMapping ("/five")
    public R request5(@RequestParam("keyword") String keyword, @RequestParam("name") String name){
        System.out.println(keyword);
        System.out.println(name);

        return R.ok().message("生成成功");
    }
}
