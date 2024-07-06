package com.example.demo.EncodeTest;

import com.example.demo.util.JWTUtil;
import org.junit.jupiter.api.Test;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.boot.test.context.SpringBootTest;
import org.springframework.security.crypto.password.PasswordEncoder;

@SpringBootTest
public class EncodeTest {
    @Autowired
    private PasswordEncoder passwordEncoder;

    @Test
    void passwordTest(){
        String encode = passwordEncoder.encode("123");
        System.out.println(encode);
    }

    @Test
    void contextLoads(){
        String jwt =JWTUtil.createJWT("10086",null);
        System.out.println(jwt);
    }
}
