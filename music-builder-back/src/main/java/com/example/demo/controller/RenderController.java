package com.example.demo.controller;

import com.example.demo.common.R;
import com.example.demo.model.RenderInfo;
import com.example.demo.service.RenderService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RestController;

@RestController
@RequestMapping("/render")
public class RenderController {
    @Autowired
    RenderService renderService;
    @PostMapping ("/submit")
    public R submit(@RequestBody RenderInfo renderInfo){
        renderService.submit(renderInfo);
        /*
        if(true){
            return R.ok().message("生成成功");
        }
        return R.error().message("生成失败");
        */
        return R.ok().message("生成成功");
    }
}
