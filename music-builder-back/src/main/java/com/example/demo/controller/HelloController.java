package com.example.demo.controller;

import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RestController;

@RestController
public class HelloController {
    @RequestMapping("/hello")
    //  @PreAuthorize("hasAnyAuthority('select')")
    public String hello() {
        return "Hello World";
    }
}
