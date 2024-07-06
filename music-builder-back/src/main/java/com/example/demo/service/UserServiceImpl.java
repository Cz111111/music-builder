package com.example.demo.service;

import com.alibaba.fastjson.JSON;
import com.example.demo.model.LoginUser;
import com.example.demo.model.User;
import com.example.demo.util.JWTUtil;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.security.authentication.AuthenticationManager;
import org.springframework.security.authentication.UsernamePasswordAuthenticationToken;
import org.springframework.security.core.Authentication;
import org.springframework.stereotype.Service;

import java.util.Objects;

@Service
public class UserServiceImpl implements  UserService{

    @Autowired
    private AuthenticationManager authenticationManager;


    @Override
    public String login(User user) {
        UsernamePasswordAuthenticationToken authentication=new UsernamePasswordAuthenticationToken(user.getUsername(),user.getPassword());
        Authentication authenticate = authenticationManager.authenticate(authentication);
        if(Objects.isNull(authenticate)){
            throw new RuntimeException("登录失败");
        }
        LoginUser loginUser = (LoginUser) authenticate.getPrincipal();
        String loginUserString = JSON.toJSONString(loginUser);
        String jwt = JWTUtil.createJWT(loginUserString,null);
        return jwt;

    }
}
