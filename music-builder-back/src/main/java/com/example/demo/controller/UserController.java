package com.example.demo.controller;

import com.example.demo.common.R;
import com.example.demo.model.User;
import com.example.demo.service.JpaUserService;
import com.example.demo.service.UserService;
import jakarta.annotation.Resource;
import org.springframework.security.access.prepost.PreAuthorize;
import org.springframework.util.StringUtils;
import org.springframework.web.bind.annotation.*;
import org.springframework.beans.factory.annotation.Autowired;
@RestController
@RequestMapping("/user")
public class UserController {
    @Autowired
    JpaUserService jpaUserService;

    @Autowired
    UserService userService;

    @RequestMapping("/hello")
  //  @PreAuthorize("hasAnyAuthority('select')")
    public String hello() {
        return "Hello World";
    }

    @PostMapping ("/login")
    public R login(@RequestBody User user){
        String jwt = userService.login(user);
        if(StringUtils.hasLength(jwt)){
            return R.ok().message("登录成功").data("token",jwt);
        }
        return R.error().message("登录失败");
    }

    @PostMapping("/register")
    public R register(@RequestBody User user){
        User user1 = jpaUserService.findByUsername(user.getUsername());
        if(user1 != null){
            return R.error().message("用户已存在");
   //         return "用户已存在";
        }
        else{
            jpaUserService.insertUser(user);
            if(jpaUserService.findByUsername(user.getUsername()) != null){
                return R.ok().message("注册成功");
    //            return "注册成功";
            }
            else{
                jpaUserService.deleteUser(user);
                return R.error().message("注册失败");
     //           return "注册失败";
            }
        }
    }
}
