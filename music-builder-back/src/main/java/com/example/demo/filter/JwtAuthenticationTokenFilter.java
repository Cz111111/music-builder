package com.example.demo.filter;


import com.alibaba.fastjson.JSON;
import com.example.demo.model.LoginUser;
import com.example.demo.util.JWTUtil;
import io.jsonwebtoken.Claims;
import jakarta.servlet.FilterChain;
import jakarta.servlet.ServletException;
import jakarta.servlet.http.HttpServletRequest;
import jakarta.servlet.http.HttpServletResponse;
import org.springframework.security.authentication.UsernamePasswordAuthenticationToken;
import org.springframework.security.core.context.SecurityContextHolder;
import org.springframework.stereotype.Component;
import org.springframework.util.StringUtils;
import org.springframework.web.filter.OncePerRequestFilter;

import java.io.IOException;

@Component
public class JwtAuthenticationTokenFilter extends OncePerRequestFilter {

    @Override
    protected void doFilterInternal(HttpServletRequest request, HttpServletResponse response, FilterChain filterChain) throws ServletException , IOException {
        String uri = request.getRequestURI();
        if(uri.equals("/user/login")){
            filterChain.doFilter(request,response);
            return;
        }
        if(uri.equals("/user/register")){
            filterChain.doFilter(request,response);
            return;
        }
        String token = request.getHeader("Authorization");
        if(!StringUtils.hasText(token)){
            throw new RuntimeException("token为空");
        }
        LoginUser loginUser = null;
        try{
            Claims claims = JWTUtil.parseJWT(token);
            String loginUserString = claims.getSubject();
            loginUser = JSON.parseObject(loginUserString,LoginUser.class);
        } catch (Exception e){
            throw new RuntimeException("token校验失败");
        }
        UsernamePasswordAuthenticationToken authentication = new UsernamePasswordAuthenticationToken(loginUser,null,loginUser.getAuthorities());
        SecurityContextHolder.getContext().setAuthentication(authentication);


        filterChain.doFilter(request,response);
    }

    // doFilterInternal 方法和其他代码
}