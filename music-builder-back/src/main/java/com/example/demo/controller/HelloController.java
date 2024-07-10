package com.example.demo.controller;

import com.example.demo.common.R;
import com.example.demo.model.User;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RestController;

@RestController
public class HelloController {
    @RequestMapping("/hello")
    //  @PreAuthorize("hasAnyAuthority('select')")
    public String hello() {
        return "Hello World";
    }

    @PostMapping("/createsong")
    public R createSong(User user) {
        return R.ok().message("114514");
    }
}
