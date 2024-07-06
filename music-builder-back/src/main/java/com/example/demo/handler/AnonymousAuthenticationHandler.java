package com.example.demo.handler;

import com.alibaba.fastjson.JSON;
import com.alibaba.fastjson.serializer.SerializerFeature;
import com.example.demo.common.R;
import jakarta.servlet.ServletException;
import jakarta.servlet.ServletOutputStream;
import jakarta.servlet.http.HttpServletRequest;
import jakarta.servlet.http.HttpServletResponse;
import org.springframework.security.authentication.BadCredentialsException;
import org.springframework.security.authentication.CredentialsExpiredException;
import org.springframework.security.authentication.InternalAuthenticationServiceException;
import org.springframework.security.core.AuthenticationException;
import org.springframework.security.web.AuthenticationEntryPoint;
import org.springframework.stereotype.Component;

import java.io.IOException;
import java.nio.charset.StandardCharsets;

@Component
public class AnonymousAuthenticationHandler implements AuthenticationEntryPoint {
    @Override
    public void commence(HttpServletRequest request, HttpServletResponse response, AuthenticationException authException) throws IOException, ServletException {
        response.setContentType("application/json;charset=utf-8");

        ServletOutputStream outputStream = response.getOutputStream();
        String jsonString = "";


        if(authException instanceof BadCredentialsException) {
            jsonString = JSON.toJSONString(R.error().message(authException.getMessage()).code(HttpServletResponse.SC_UNAUTHORIZED), SerializerFeature.DisableCircularReferenceDetect);
        } else if(authException instanceof InternalAuthenticationServiceException){
            jsonString = JSON.toJSONString(R.error().message("用户名为空").code(HttpServletResponse.SC_UNAUTHORIZED), SerializerFeature.DisableCircularReferenceDetect);
//        } else if(authException instanceof Exce

        } else{
            jsonString = JSON.toJSONString(R.error().message("匿名用户无权访问").code(600), SerializerFeature.DisableCircularReferenceDetect);
        }

        outputStream.write(jsonString.getBytes(StandardCharsets.UTF_8));
        outputStream.flush();
        outputStream.close();
    }
}
